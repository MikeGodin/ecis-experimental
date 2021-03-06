import { Child } from '../../generated';
import { InlineIcon } from '../../components';

const RACES: (keyof Child)[] = [
	'americanIndianOrAlaskaNative',
	'asian',
	'blackOrAfricanAmerican',
	'nativeHawaiianOrPacificIslander',
	'white',
];

export function prettyRace(race: keyof Child) {
	switch (race) {
		case 'americanIndianOrAlaskaNative':
			return 'American Indian or Alaska Native';
		case 'asian':
			return 'Asian';
		case 'blackOrAfricanAmerican':
			return 'Black or African American';
		case 'nativeHawaiianOrPacificIslander':
			return 'Native Hawaiian Or Pacific Islander';
		case 'white':
			return 'White';
	}
}

export function prettyMultiRace(child: Child) {
	const selectedRaces = RACES.filter((race) => child[race]);

	if (selectedRaces.length === 0) {
		return '';
	} else if (selectedRaces.length === 1) {
		return prettyRace(selectedRaces[0]);
	} else {
		return 'Multiple races';
	}
}

export function prettyEthnicity(child: Child) {
	const ethnicity = child.hispanicOrLatinxEthnicity;
	let ethnicityStr;
	if (ethnicity == null) {
		ethnicityStr = '';
	} else {
		ethnicityStr = ethnicity ? 'Hispanic/Latinx' : 'Not Hispanic/Latinx';
	}
	return ethnicityStr;
}

export function birthCertPresent(child: Child) {
	return child.birthCertificateId && child.birthState && child.birthTown ? 'Yes' : null;
}

export function getSummaryLine(val: any) {
	if (val) return val;

	return InlineIcon({ icon: 'incomplete' });
}
