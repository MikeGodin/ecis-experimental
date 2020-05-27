import React, { createContext } from 'react';
import { DeepNonUndefineable } from '../../utils/types';
import {
	ReportingPeriod,
	ApiReportingPeriodsSourceGetRequest,
	FundingSource,
} from '../../generated';
import useApi from '../../hooks/useApi';
import { propertyDateSorter } from '../../utils/dateSorter';

export type ReportingPeriodContextType = {
	cdcReportingPeriods: DeepNonUndefineable<ReportingPeriod[]>;
};

const ReportingPeriodContext = createContext<ReportingPeriodContextType>({
	cdcReportingPeriods: [],
});

const { Provider, Consumer } = ReportingPeriodContext;

const ReportingPeriodProvider: React.FC<{}> = ({ children }) => {
	const cdcReportingPeriodParams: ApiReportingPeriodsSourceGetRequest = {
		source: FundingSource.CDC,
	};

	const { data } = useApi<ReportingPeriod[]>(api =>
		api.apiReportingPeriodsSourceGet(cdcReportingPeriodParams)
	);

	return (
		<Provider
			value={{
				cdcReportingPeriods: data.sort((a, b) => propertyDateSorter(a, b, period => period.period))
			}}
		>
			{children}
		</Provider>
	);
};

export { ReportingPeriodProvider };
export { Consumer as ReportingPeriodConsumer };
export default ReportingPeriodContext;
