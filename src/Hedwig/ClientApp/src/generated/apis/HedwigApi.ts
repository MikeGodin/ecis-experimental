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


import * as runtime from '../runtime';
import {
    CdcReport,
    CdcReportFromJSON,
    CdcReportToJSON,
    Enrollment,
    EnrollmentFromJSON,
    EnrollmentToJSON,
    FundingSource,
    FundingSourceFromJSON,
    FundingSourceToJSON,
    Organization,
    OrganizationFromJSON,
    OrganizationToJSON,
    ProblemDetails,
    ProblemDetailsFromJSON,
    ProblemDetailsToJSON,
    ReportingPeriod,
    ReportingPeriodFromJSON,
    ReportingPeriodToJSON,
    Site,
    SiteFromJSON,
    SiteToJSON,
    User,
    UserFromJSON,
    UserToJSON,
} from '../models';

export interface ApiOrganizationsIdGetRequest {
    id: number;
    include?: Array<string>;
}

export interface ApiOrganizationsOrgIdReportsGetRequest {
    orgId: number;
}

export interface ApiOrganizationsOrgIdReportsIdGetRequest {
    id: number;
    orgId: number;
    include?: Array<string>;
}

export interface ApiOrganizationsOrgIdReportsIdPutRequest {
    id: number;
    orgId: number;
    cdcReport?: CdcReport;
}

export interface ApiOrganizationsOrgIdSitesGetRequest {
    orgId: number;
}

export interface ApiOrganizationsOrgIdSitesIdGetRequest {
    id: number;
    orgId: number;
    include?: Array<string>;
}

export interface ApiOrganizationsOrgIdSitesSiteIdEnrollmentsGetRequest {
    orgId: number;
    siteId: number;
    include?: Array<string>;
    startDate?: Date;
    endDate?: Date;
}

export interface ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGetRequest {
    id: number;
    orgId: number;
    siteId: number;
    include?: Array<string>;
}

export interface ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPutRequest {
    id: number;
    orgId: number;
    siteId: number;
    enrollment?: Enrollment;
}

export interface ApiOrganizationsOrgIdSitesSiteIdEnrollmentsPostRequest {
    orgId: number;
    siteId: number;
    enrollment?: Enrollment;
}

export interface ApiReportingPeriodSourceGetRequest {
    source: FundingSource;
}

/**
 * no description
 */
export class HedwigApi extends runtime.BaseAPI {

    /**
     */
    async apiOrganizationsIdGetRaw(requestParameters: ApiOrganizationsIdGetRequest): Promise<runtime.ApiResponse<Organization>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiOrganizationsIdGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        if (requestParameters.include) {
            queryParameters['include[]'] = requestParameters.include;
        }

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/Organizations/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => OrganizationFromJSON(jsonValue));
    }

    /**
     */
    async apiOrganizationsIdGet(requestParameters: ApiOrganizationsIdGetRequest): Promise<Organization> {
        const response = await this.apiOrganizationsIdGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdReportsGetRaw(requestParameters: ApiOrganizationsOrgIdReportsGetRequest): Promise<runtime.ApiResponse<Array<CdcReport>>> {
        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdReportsGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/Reports`.replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => jsonValue.map(CdcReportFromJSON));
    }

    /**
     */
    async apiOrganizationsOrgIdReportsGet(requestParameters: ApiOrganizationsOrgIdReportsGetRequest): Promise<Array<CdcReport>> {
        const response = await this.apiOrganizationsOrgIdReportsGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdReportsIdGetRaw(requestParameters: ApiOrganizationsOrgIdReportsIdGetRequest): Promise<runtime.ApiResponse<CdcReport>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiOrganizationsOrgIdReportsIdGet.');
        }

        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdReportsIdGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        if (requestParameters.include) {
            queryParameters['include[]'] = requestParameters.include;
        }

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/Reports/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))).replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => CdcReportFromJSON(jsonValue));
    }

    /**
     */
    async apiOrganizationsOrgIdReportsIdGet(requestParameters: ApiOrganizationsOrgIdReportsIdGetRequest): Promise<CdcReport> {
        const response = await this.apiOrganizationsOrgIdReportsIdGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdReportsIdPutRaw(requestParameters: ApiOrganizationsOrgIdReportsIdPutRequest): Promise<runtime.ApiResponse<CdcReport>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiOrganizationsOrgIdReportsIdPut.');
        }

        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdReportsIdPut.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json-patch+json';

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/Reports/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))).replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))),
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: CdcReportToJSON(requestParameters.cdcReport),
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => CdcReportFromJSON(jsonValue));
    }

    /**
     */
    async apiOrganizationsOrgIdReportsIdPut(requestParameters: ApiOrganizationsOrgIdReportsIdPutRequest): Promise<CdcReport> {
        const response = await this.apiOrganizationsOrgIdReportsIdPutRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdSitesGetRaw(requestParameters: ApiOrganizationsOrgIdSitesGetRequest): Promise<runtime.ApiResponse<Array<Site>>> {
        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdSitesGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/Sites`.replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => jsonValue.map(SiteFromJSON));
    }

    /**
     */
    async apiOrganizationsOrgIdSitesGet(requestParameters: ApiOrganizationsOrgIdSitesGetRequest): Promise<Array<Site>> {
        const response = await this.apiOrganizationsOrgIdSitesGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdSitesIdGetRaw(requestParameters: ApiOrganizationsOrgIdSitesIdGetRequest): Promise<runtime.ApiResponse<Site>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiOrganizationsOrgIdSitesIdGet.');
        }

        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdSitesIdGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        if (requestParameters.include) {
            queryParameters['include[]'] = requestParameters.include;
        }

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/Sites/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))).replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => SiteFromJSON(jsonValue));
    }

    /**
     */
    async apiOrganizationsOrgIdSitesIdGet(requestParameters: ApiOrganizationsOrgIdSitesIdGetRequest): Promise<Site> {
        const response = await this.apiOrganizationsOrgIdSitesIdGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsGetRaw(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsGetRequest): Promise<runtime.ApiResponse<Array<Enrollment>>> {
        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsGet.');
        }

        if (requestParameters.siteId === null || requestParameters.siteId === undefined) {
            throw new runtime.RequiredError('siteId','Required parameter requestParameters.siteId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        if (requestParameters.include) {
            queryParameters['include[]'] = requestParameters.include;
        }

        if (requestParameters.startDate !== undefined) {
            queryParameters['startDate'] = (requestParameters.startDate as any).toISOString();
        }

        if (requestParameters.endDate !== undefined) {
            queryParameters['endDate'] = (requestParameters.endDate as any).toISOString();
        }

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/sites/{siteId}/Enrollments`.replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))).replace(`{${"siteId"}}`, encodeURIComponent(String(requestParameters.siteId))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => jsonValue.map(EnrollmentFromJSON));
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsGet(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsGetRequest): Promise<Array<Enrollment>> {
        const response = await this.apiOrganizationsOrgIdSitesSiteIdEnrollmentsGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGetRaw(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGetRequest): Promise<runtime.ApiResponse<Enrollment>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGet.');
        }

        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGet.');
        }

        if (requestParameters.siteId === null || requestParameters.siteId === undefined) {
            throw new runtime.RequiredError('siteId','Required parameter requestParameters.siteId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        if (requestParameters.include) {
            queryParameters['include[]'] = requestParameters.include;
        }

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/sites/{siteId}/Enrollments/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))).replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))).replace(`{${"siteId"}}`, encodeURIComponent(String(requestParameters.siteId))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => EnrollmentFromJSON(jsonValue));
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGet(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGetRequest): Promise<Enrollment> {
        const response = await this.apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPutRaw(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPutRequest): Promise<runtime.ApiResponse<Enrollment>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPut.');
        }

        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPut.');
        }

        if (requestParameters.siteId === null || requestParameters.siteId === undefined) {
            throw new runtime.RequiredError('siteId','Required parameter requestParameters.siteId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPut.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json-patch+json';

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/sites/{siteId}/Enrollments/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))).replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))).replace(`{${"siteId"}}`, encodeURIComponent(String(requestParameters.siteId))),
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: EnrollmentToJSON(requestParameters.enrollment),
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => EnrollmentFromJSON(jsonValue));
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPut(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPutRequest): Promise<Enrollment> {
        const response = await this.apiOrganizationsOrgIdSitesSiteIdEnrollmentsIdPutRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsPostRaw(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsPostRequest): Promise<runtime.ApiResponse<Enrollment>> {
        if (requestParameters.orgId === null || requestParameters.orgId === undefined) {
            throw new runtime.RequiredError('orgId','Required parameter requestParameters.orgId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsPost.');
        }

        if (requestParameters.siteId === null || requestParameters.siteId === undefined) {
            throw new runtime.RequiredError('siteId','Required parameter requestParameters.siteId was null or undefined when calling apiOrganizationsOrgIdSitesSiteIdEnrollmentsPost.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json-patch+json';

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/organizations/{orgId}/sites/{siteId}/Enrollments`.replace(`{${"orgId"}}`, encodeURIComponent(String(requestParameters.orgId))).replace(`{${"siteId"}}`, encodeURIComponent(String(requestParameters.siteId))),
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: EnrollmentToJSON(requestParameters.enrollment),
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => EnrollmentFromJSON(jsonValue));
    }

    /**
     */
    async apiOrganizationsOrgIdSitesSiteIdEnrollmentsPost(requestParameters: ApiOrganizationsOrgIdSitesSiteIdEnrollmentsPostRequest): Promise<Enrollment> {
        const response = await this.apiOrganizationsOrgIdSitesSiteIdEnrollmentsPostRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiReportingPeriodSourceGetRaw(requestParameters: ApiReportingPeriodSourceGetRequest): Promise<runtime.ApiResponse<Array<ReportingPeriod>>> {
        if (requestParameters.source === null || requestParameters.source === undefined) {
            throw new runtime.RequiredError('source','Required parameter requestParameters.source was null or undefined when calling apiReportingPeriodSourceGet.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/ReportingPeriod/{source}`.replace(`{${"source"}}`, encodeURIComponent(String(requestParameters.source))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => jsonValue.map(ReportingPeriodFromJSON));
    }

    /**
     */
    async apiReportingPeriodSourceGet(requestParameters: ApiReportingPeriodSourceGetRequest): Promise<Array<ReportingPeriod>> {
        const response = await this.apiReportingPeriodSourceGetRaw(requestParameters);
        return await response.value();
    }

    /**
     */
    async apiUsersCurrentGetRaw(): Promise<runtime.ApiResponse<User>> {
        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/Users/current`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse(response, (jsonValue) => UserFromJSON(jsonValue));
    }

    /**
     */
    async apiUsersCurrentGet(): Promise<User> {
        const response = await this.apiUsersCurrentGetRaw();
        return await response.value();
    }

}
