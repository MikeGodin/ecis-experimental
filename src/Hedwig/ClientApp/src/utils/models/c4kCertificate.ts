import { C4KCertificate, Enrollment } from '../../generated';
import { DateRange } from '../../components';
import { DeepNonUndefineable } from '../types';
import moment from 'moment';
import idx from 'idx';
import { dateSorter } from '../dateSorter';

/**
 * C4K funding does NOT overlap with range if:
 * - Certificate end date is before range start
 * - Certificate start date is after range ends
 * @param funding
 * @param range
 */
export function isCurrentToRangeC4K(cert: C4KCertificate, range?: DateRange): boolean {
	range = range ? range : { startDate: moment(), endDate: moment() };
	if (
		(range.startDate && cert.endDate && range.startDate.isAfter(cert.endDate)) ||
		(range.endDate && cert.startDate && range.endDate.isBefore(cert.startDate))
	) {
		return false;
	}
	return true;
}

export function c4kCertificateSorter(a: C4KCertificate, b: C4KCertificate) {
	return dateSorter(a.startDate, b.startDate);
}

export function currentC4kCertificate(
	enrollment: DeepNonUndefineable<Enrollment> | null
): DeepNonUndefineable<C4KCertificate> | undefined {
	if (!enrollment) {
		return undefined;
	}
	const c4kCerts =
		idx<Enrollment, DeepNonUndefineable<C4KCertificate[]>>(
			enrollment,
			_ => _.child.c4KCertificates as DeepNonUndefineable<C4KCertificate[]>
		) || [];
	// Sorts by the start of the certificate
	const sortedCerts = (c4kCerts || []).sort(c4kCertificateSorter);
	// returns the latest one
	return [...sortedCerts].pop();
}

export function activeC4kFundingAsOf(
	enrollment: DeepNonUndefineable<Enrollment> | null,
	asOf?: Date
) {
	if (!asOf) {
		return currentC4kCertificate(enrollment);
	} else {
		if (!enrollment) {
			return undefined;
		}
		const c4kCerts =
			idx<Enrollment, DeepNonUndefineable<C4KCertificate[]>>(
				enrollment,
				_ => _.child.c4KCertificates as DeepNonUndefineable<C4KCertificate[]>
			) || [];
		// Sorts by the start of the certificate
		const sortedCerts = (c4kCerts || []).sort(c4kCertificateSorter);
		// find cert with asOf between start and end dates
		return sortedCerts.find<DeepNonUndefineable<C4KCertificate>>(cert => {
			const startDateIsBeforeAsOf = !cert.startDate || moment(cert.startDate).isBefore(asOf);
			const endDateIsEmptyOrAfterAsOf = !cert.endDate || moment(cert.endDate).isAfter(asOf);
			return startDateIsBeforeAsOf && endDateIsEmptyOrAfterAsOf;
		});
	}
}