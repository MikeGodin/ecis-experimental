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
	Organization,
	OrganizationFromJSON,
	OrganizationFromJSONTyped,
	OrganizationToJSON,
	User,
	UserFromJSON,
	UserFromJSONTyped,
	UserToJSON,
} from './';

/**
 *
 * @export
 * @interface OrganizationPermission
 */
export interface OrganizationPermission {
	/**
	 *
	 * @type {number}
	 * @memberof OrganizationPermission
	 */
	organizationId: number;
	/**
	 *
	 * @type {Organization}
	 * @memberof OrganizationPermission
	 */
	organization?: Organization;
	/**
	 *
	 * @type {number}
	 * @memberof OrganizationPermission
	 */
	id: number;
	/**
	 *
	 * @type {number}
	 * @memberof OrganizationPermission
	 */
	userId: number;
	/**
	 *
	 * @type {User}
	 * @memberof OrganizationPermission
	 */
	user?: User;
}

export function OrganizationPermissionFromJSON(json: any): OrganizationPermission {
	return OrganizationPermissionFromJSONTyped(json, false);
}

export function OrganizationPermissionFromJSONTyped(
	json: any,
	ignoreDiscriminator: boolean
): OrganizationPermission {
	if (json === undefined || json === null) {
		return json;
	}
	return {
		organizationId: json['organizationId'],
		organization: !exists(json, 'organization')
			? undefined
			: OrganizationFromJSON(json['organization']),
		id: json['id'],
		userId: json['userId'],
		user: !exists(json, 'user') ? undefined : UserFromJSON(json['user']),
	};
}

export function OrganizationPermissionToJSON(value?: OrganizationPermission | null): any {
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
		userId: value.userId,
		user: UserToJSON(value.user),
	};
}
