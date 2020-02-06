import React from 'react';
import { ReactComponent as Error } from '../../../node_modules/uswds/dist/img/alerts/error.svg';
import { ReactComponent as Info } from '../../../node_modules/uswds/dist/img/alerts/info.svg';
import { ReactComponent as Success } from '../../../node_modules/uswds/dist/img/alerts/success.svg';
import cx from 'classnames';

type Icon = 'attentionNeeded' | 'complete' | 'incomplete';

export type InlineIconProps = {
	icon: Icon;
	provideScreenReaderFallback?: boolean;
};

export function InlineIcon({ icon, provideScreenReaderFallback = true }: InlineIconProps) {
	let text: string;
	let iconComponent;

	switch (icon) {
		case 'attentionNeeded':
			text = 'attention needed';
			iconComponent = <Error />;
			break;
		case 'complete':
			text = 'complete';
			iconComponent = <Success />;
			break;
		case 'incomplete':
			text = 'incomplete';
			iconComponent = <Error />;
			break;
		default:
			text = '';
			iconComponent = <Info />;
	}

	return (
		<span className={cx('oec-inline-icon', `oec-inline-icon--${icon}`)}>
			{iconComponent}
			{provideScreenReaderFallback && <span className={cx('usa-sr-only')}>({text})</span>}
		</span>
	);
}
