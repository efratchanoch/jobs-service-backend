# Jobs Service — data and API specification

**Last updated:** March 2026  

This document describes the main persistence model and HTTP surface of the jobs backend API.

## Database tables

### Job
Central table for job postings. One row per job.

| Column | Type | Notes |
|--------|------|--------|
| JobId | int (PK) | Surrogate key |
| Title, Description, Requirements | string | Job content |
| Experience | int | Required experience (years) |
| CompanyName, Location | string | Employer and location |
| IsRemote, IsPrivate | bool | Remote flag; private jobs use invitations |
| JobType, Field, Status | enum | Classification and lifecycle |
| SalaryMin, SalaryMax | int? | Optional salary range |
| JobWebsiteUrl, ImageUrl | string? | Optional links |
| IsActive | bool | Soft-delete / visibility |
| CreatedAt | DateTime | When the job was published |
| Deadline | DateTime? | Application deadline (optional) |

### Tag
Technology or skill tags.

| Column | Type | Notes |
|--------|------|--------|
| TagId | int (PK) | |
| TagName | string | e.g. React, C#, SQL |

### JobTags (many-to-many)
Links jobs to tags.

| Column | Type |
|--------|------|
| JobId | FK → Job |
| TagId | FK → Tag |

### Application
One row per student application to a job. **Unique index on (JobId, StudentId).**

| Column | Type | Notes |
|--------|------|--------|
| ApplicationId | int (PK) | |
| JobId, StudentId | int (FK) | StudentId must match JWT in API |
| AppliedAt, UpdatedAt | DateTime | Submission and last status change |
| Status | enum | Pending / Interviewed / Accepted / Rejected |
| CoverLetter, Notes | string? | Optional; notes are manager-internal |
| ResumeUrl | string? | Optional attachment reference |

### PrivateJobInvitation
Invitations from managers to specific students for private jobs.

| Column | Type | Notes |
|--------|------|--------|
| InvitationId | int (PK) | |
| JobId, StudentId | int (FK) | |
| InvitedAt | DateTime | |
| IsViewed | bool | Whether the student acknowledged viewing |

## Enums (representative values)
- **JobType:** FullTime, PartTime, Internship  
- **Field:** (e.g. development / QA / DevOps — see `Field` enum in code)  
- **JobStatus:** Open, Closed, Pending (see `JobStatus` in code)  
- **ApplicationStatus:** Pending, Interviewed, Accepted, Rejected  

## API overview (controllers)

| Area | Typical routes | Auth |
|------|------------------|------|
| Jobs | `GET /api/jobs`, `GET /api/jobs/search`, `GET /api/jobs/{id}`, `POST/PUT/DELETE` (manager) | Mixed |
| Applications | `GET /api/applications/my`, `POST /api/applications`, `GET /api/applications/job/{jobId}` (manager), status/notes updates (manager) | JWT; manager where noted |
| Invitations | `POST /api/invitations/bulk` (manager), `GET .../my`, `my/all`, `my/new`, `PATCH .../view` | JWT |
| Tags | `GET /api/tags` (public), create/delete (manager) | Mixed |
| Health | `GET /api/health` | Anonymous |

## Security and business rules (summary)
- **StudentId** on applications must come from the JWT, not from the client body.  
- **Duplicate applications** are prevented by the unique constraint on (JobId, StudentId).  
- **Soft delete** for jobs typically uses `IsActive = false` rather than physical delete.  
- **Deadlines:** application submission should be rejected when the job deadline is in the past (enforced in application service).  

## Integrations (planned / external)
- Email or notification dispatch for applications and invitations may call a separate notifications service; align JSON contracts with that service.
