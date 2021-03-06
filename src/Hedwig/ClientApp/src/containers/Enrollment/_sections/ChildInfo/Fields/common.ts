import { ErrorAlertState } from '../../../../../hooks/useCatchAllErrorAlert';
import { ValidationResponse } from '../../../../../utils/validations/displayValidationStatus';

// Common helper type for supplying additional props to field components
export type ChildInfoFormFieldProps = {
	initialLoad?: boolean;
	error?: ValidationResponse | null;
	errorAlertState?: ErrorAlertState;
};
