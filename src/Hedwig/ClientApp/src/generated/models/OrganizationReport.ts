/* tslint:disable */
/* eslint-disable */
/**
 * Hedwig API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 *
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { exists, mapValues } from '../runtime';
import {
	Enrollment,
	EnrollmentFromJSON,
	EnrollmentFromJSONTyped,
	EnrollmentToJSON,
	FundingSource,
	FundingSourceFromJSON,
	FundingSourceFromJSONTyped,
	FundingSourceToJSON,
	Organization,
	OrganizationFromJSON,
	OrganizationFromJSONTyped,
	OrganizationToJSON,
	ReportingPeriod,
	ReportingPeriodFromJSON,
	ReportingPeriodFromJSONTyped,
	ReportingPeriodToJSON,
	ValidationError,
	ValidationErrorFromJSON,
	ValidationErrorFromJSONTyped,
	ValidationErrorToJSON,
} from './';

/**
 *
 * @export
 * @interface OrganizationReport
 */
export interface OrganizationReport {
	/**
	 *
	 * @type {number}
	 * @memberof OrganizationReport
	 */
	organizationId: number;
	/**
	 *
	 * @type {Organization}
	 * @memberof OrganizationReport
	 */
	organization?: Organization;
	/**
	 *
	 * @type {number}
	 * @memberof OrganizationReport
	 */
	id: number;
	/**
	 *
	 * @type {FundingSource}
	 * @memberof OrganizationReport
	 */
	type?: FundingSource;
	/**
	 *
	 * @type {number}
	 * @memberof OrganizationReport
	 */
	reportingPeriodId: number;
	/**
	 *
	 * @type {ReportingPeriod}
	 * @memberof OrganizationReport
	 */
	reportingPeriod: ReportingPeriod;
	/**
	 *
	 * @type {Date}
	 * @memberof OrganizationReport
	 */
	submittedAt?: Date | null;
	/**
	 *
	 * @type {Array<Enrollment>}
	 * @memberof OrganizationReport
	 */
	enrollments?: Array<Enrollment> | null;
	/**
	 *
	 * @type {Array<ValidationError>}
	 * @memberof OrganizationReport
	 */
	validationErrors?: Array<ValidationError> | null;
}

export function OrganizationReportFromJSON(json: any): OrganizationReport {
	return OrganizationReportFromJSONTyped(json, false);
}

export function OrganizationReportFromJSONTyped(
	json: any,
	ignoreDiscriminator: boolean
): OrganizationReport {
	if (json === undefined || json === null) {
		return json;
	}
	return {
		organizationId: json['organizationId'],
		organization: !exists(json, 'organization')
			? undefined
			: OrganizationFromJSON(json['organization']),
		id: json['id'],
		type: !exists(json, 'type') ? undefined : FundingSourceFromJSON(json['type']),
		reportingPeriodId: json['reportingPeriodId'],
		reportingPeriod: ReportingPeriodFromJSON(json['reportingPeriod']),
		submittedAt: !exists(json, 'submittedAt')
			? undefined
			: json['submittedAt'] === null
			? null
			: new Date(json['submittedAt']),
		enrollments: !exists(json, 'enrollments')
			? undefined
			: json['enrollments'] === null
			? null
			: (json['enrollments'] as Array<any>).map(EnrollmentFromJSON),
		validationErrors: !exists(json, 'validationErrors')
			? undefined
			: json['validationErrors'] === null
			? null
			: (json['validationErrors'] as Array<any>).map(ValidationErrorFromJSON),
	};
}

export function OrganizationReportToJSON(value?: OrganizationReport | null): any {
	if (value === undefined) {
		return undefined;
	}
	if (value === null) {
		return null;
	}
	return {
		organizationId: value.organizationId,
		organization: OrganizationToJSON(value.organization),
		id: value.id,
		type: FundingSourceToJSON(value.type),
		reportingPeriodId: value.reportingPeriodId,
		reportingPeriod: ReportingPeriodToJSON(value.reportingPeriod),
		submittedAt:
			value.submittedAt === undefined
				? undefined
				: value.submittedAt === null
				? null
				: value.submittedAt.toISOString(),
		enrollments:
			value.enrollments === undefined
				? undefined
				: value.enrollments === null
				? null
				: (value.enrollments as Array<any>).map(EnrollmentToJSON),
		validationErrors:
			value.validationErrors === undefined
				? undefined
				: value.validationErrors === null
				? null
				: (value.validationErrors as Array<any>).map(ValidationErrorToJSON),
	};
}
