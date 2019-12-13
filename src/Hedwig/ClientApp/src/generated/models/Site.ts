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
    Organization,
    OrganizationFromJSON,
    OrganizationFromJSONTyped,
    OrganizationToJSON,
    Region,
    RegionFromJSON,
    RegionFromJSONTyped,
    RegionToJSON,
} from './';

/**
 * 
 * @export
 * @interface Site
 */
export interface Site {
    /**
     * 
     * @type {number}
     * @memberof Site
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof Site
     */
    name: string;
    /**
     * 
     * @type {boolean}
     * @memberof Site
     */
    titleI?: boolean;
    /**
     * 
     * @type {Region}
     * @memberof Site
     */
    region?: Region;
    /**
     * 
     * @type {number}
     * @memberof Site
     */
    organizationId?: number | null;
    /**
     * 
     * @type {Organization}
     * @memberof Site
     */
    organization?: Organization;
    /**
     * 
     * @type {Array<Enrollment>}
     * @memberof Site
     */
    enrollments?: Array<Enrollment> | null;
}

export function SiteFromJSON(json: any): Site {
    return SiteFromJSONTyped(json, false);
}

export function SiteFromJSONTyped(json: any, ignoreDiscriminator: boolean): Site {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'name': json['name'],
        'titleI': !exists(json, 'titleI') ? undefined : json['titleI'],
        'region': !exists(json, 'region') ? undefined : RegionFromJSON(json['region']),
        'organizationId': !exists(json, 'organizationId') ? undefined : json['organizationId'],
        'organization': !exists(json, 'organization') ? undefined : OrganizationFromJSON(json['organization']),
        'enrollments': !exists(json, 'enrollments') ? undefined : (json['enrollments'] === null ? null : (json['enrollments'] as Array<any>).map(EnrollmentFromJSON)),
    };
}

export function SiteToJSON(value?: Site | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'name': value.name,
        'titleI': value.titleI,
        'region': RegionToJSON(value.region),
        'organizationId': value.organizationId,
        'organization': OrganizationToJSON(value.organization),
        'enrollments': value.enrollments === undefined ? undefined : (value.enrollments === null ? null : (value.enrollments as Array<any>).map(EnrollmentToJSON)),
    };
}

