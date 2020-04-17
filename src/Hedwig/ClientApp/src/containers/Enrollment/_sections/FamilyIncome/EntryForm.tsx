import React, { useContext, useState, useEffect } from 'react';
import { SectionProps } from '../../enrollmentTypes';
import AlertContext from '../../../../contexts/Alert/AlertContext';
import FamilyIncome from '.';
import useApi, { ApiError } from '../../../../hooks/useApi';
import {
	isBlockingValidationError,
	initialLoadErrorGuard,
	warningForFieldSet,
	hasValidationErrors,
} from '../../../../utils/validations';
import { validationErrorAlert } from '../../../../utils/stringFormatters/alertTextMakers';
import UserContext from '../../../../contexts/User/UserContext';
import { DeepNonUndefineable } from '../../../../utils/types';
import {
	Enrollment,
	ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPutRequest,
	FamilyDetermination,
} from '../../../../generated';
import idx from 'idx';
import { validatePermissions, getIdForUser, determinationSorter } from '../../../../utils/models';
import { FieldSet, Alert } from '../../../../components';
import Form from '../../../../components/Form/Form';
import {
	householdSizeField,
	incomeDisclosedField,
	annualHouseholdIncomeField,
	determinationDateField,
} from './Form';
import FormInset from '../../../../components/Form/FormInset';
import FormSubmitButton from '../../../../components/Form/FormSubmitButton';

const EntryForm: React.FC<SectionProps> = ({
	enrollment,
	updateEnrollment,
	siteId,
	error: inputError,
	loading,
	successCallback,
	visitSection,
	visitedSections,
}) => {
	if (!enrollment || !enrollment.child || !enrollment.child.family) {
		throw new Error('FamilyIncome rendered without enrollment.child.family');
	}

	// set up form state
	const { setAlerts } = useContext(AlertContext);
	const initialLoad = visitedSections ? !visitedSections[FamilyIncome.key] : false;
	if (initialLoad) {
		visitSection && visitSection(FamilyIncome);
	}
	const [error, setError] = useState<ApiError | null>(inputError);
	const [hasAlertedOnError, setHasAlertedOnError] = useState(false);
	useEffect(() => {
		if (error && !hasAlertedOnError) {
			if (!isBlockingValidationError(error)) {
				throw new Error(error.title || 'Unknown api error');
			}
			setAlerts([validationErrorAlert]);
		}
	}, [error, hasAlertedOnError]);

	const { user } = useContext(UserContext);

	const child = enrollment.child;
	const determinations = idx(child, _ => _.family.determinations as FamilyDetermination[]) || [];
	const sortedDeterminations = [...determinations].sort((a, b) => determinationSorter(a, b, true));
	const determination = sortedDeterminations[0];

	const [attemptingSave, setAttemptingSave] = useState(false);
	const defaultParams: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPutRequest = {
		id: enrollment.id || 0,
		siteId: validatePermissions(user, 'site', siteId) ? siteId : 0,
		orgId: getIdForUser(user, 'org'),
		enrollment: enrollment,
	};
	const { error: saveError, data: saveData } = useApi<Enrollment>(
		api => api.apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPut(defaultParams),
		{ skip: !attemptingSave, callback: () => setAttemptingSave(false) }
	);

	useEffect(() => {
		// If the request went through, then do the next steps
		if (!saveData && !saveError) {
			return;
		}
		// Set the new error regardless of whether there is one
		setError(saveError);
		if (saveData && !saveError) {
			if (successCallback) successCallback(saveData);
		}
	}, [saveData, saveError]);

	// To skip over family income section when "Lives with foster family" is selected
	if (child.foster && successCallback) {
		successCallback(enrollment);
		return <div className="FamilyIncome foster"></div>;
	}

	return (
		<Form<Enrollment>
			noValidate
			autoComplete="off"
			className="FamilyIncomeForm"
			data={enrollment}
			onSave={enrollment => {
				updateEnrollment(enrollment as DeepNonUndefineable<Enrollment>);
				setAttemptingSave(true);
			}}
			additionalInformation={{
				initialLoad,
			}}
		>
			<FormInset<Enrollment, { initialLoad: boolean }>
				render={({ containingData: enrollment, additionalInformation }) => {
					const determinationDate = determination && determination.determinationDate;
					const notDisclosed = determination && determination.notDisclosed;
					const { initialLoad } = additionalInformation;
					if (notDisclosed) {
						return null;
					}
					return (
						<FieldSet
							id="family-income"
							legend="Family income"
							status={initialLoadErrorGuard(
								initialLoad,
								// Missing information for determination warning
								warningForFieldSet(
									'family-income',
									// Only check for determinationDate errors if it does not have a value. Otherwise, the error is about the
									// value of determinationDate and should not trigger a missing information alert
									['numberOfPeople', 'income', !determinationDate ? 'determinationDate' : ''],
									determination ? determination : null,
									'This information is required for OEC reporting'
								) ||
									// Missing determination warning
									warningForFieldSet(
										'family-income',
										['determinations'],
										idx(enrollment, _ => _.child.family) || null,
										'Income must be determined or marked as not disclosed',
										true
									)
							)}
						>
							<div>{householdSizeField(0)}</div>
							<div>{annualHouseholdIncomeField(0)}</div>
							<div>{determinationDateField(0)}</div>
						</FieldSet>
					);
				}}
			/>
			{incomeDisclosedField(0)}
			{/*Only display the alert if:
			 - determination is notDisclosed
			 - there is no 'notDisclosed' validationError (which will result in warning on the notDisclosed field)
			*/}
			<FormInset<Enrollment>
				render={() => {
					const notDisclosed = determination ? determination.notDisclosed || null : null;
					if (
						notDisclosed &&
						!hasValidationErrors(determination ? determination : null, ['notDisclosed'])
					) {
						return (
							<Alert
								type="info"
								text="Income information is required to enroll a child in a CDC funded space. You will not be able to assign this child to a funding space without this information."
							/>
						);
					}
					return null;
				}}
			/>
			<div className="usa-form">
				{/* <FormSubmitButton text={attemptingSave ? 'Saving...' : 'Save'} disabled={attemptingSave} /> */}
				<FormSubmitButton text={loading ? 'Saving...' : 'Save'} disabled={loading} />
			</div>
		</Form>
	);
};

export default EntryForm;