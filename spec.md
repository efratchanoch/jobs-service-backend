בס"ד 04.03.2026
Jobs Service הנתונים בסיס אפיון
הנתונים בסיס טבלאות
1
. Job – טבלת משרו ת
אח משרה מייצגת שורה כל ,השירות של המרכזית הטבלה ת.
נתונים עמוד ה סוג הסב ר
JobId int (PK) למשרה ייחודי מזהה
Title string המשר ה כותר ת
Description string המשר ה של מפורט תיאור
Experience Int נדרשו ת ניסיון שנו ת
Requirements string חופש טקסט ) כלליות דרישות י (
CompanyName string המגייס ת החברה שם
Location string וכו ירושלים , אביב תל ) מיקום '(
IsRemote boolean מהבית העבודה האם ?
IsPrivate boolean למוזמנות רק גלויה המשרה האם ?
JobType Enum FullTime / PartTime / Internship Field Enum פיתוח / QA / DevOps : תחו ם
SalaryMIn int? שכ ר טווח מינמל י
SalaryMax
int?
טווחשכר מקסימלי
Status Enum Open / Closed / Pending
IsActive
boolean
רכ מחיקה ) ?פעילה המשרה האם ה (
CreatedAt DateTime פורסמה המשרה מתי
Deadline
DateTime
מועמדו ת להגשת אחרון תאריך
בס"ד 04.03.2026
2
. Tag - תגיות טכנולוגיה
רשימת הטכנולוגיות הקיימות במערכת.
עמוד ה נתונים סוג הסב ר
TagId int (PK) לתגי ת ייחודי מזהה
TagName string הטכנולוגיה שם (וכו React, C#, SQL)
3
. JobTags - טבלת קשר משרה -תגי ת
כל שורה מקשרת תגית אחת למשרה אחת. טבלת קשר רבים לרבים .
עמוד ה נתונים סוג הסב ר
JobId int (FK) למשרה זר מפתח
TagId int (FK) לתגי ת זר מפתח
4
. Application - טבלת מועמדויות
כל הגשת מועמדות של תלמידה למשרה. Unique Constraint על (JobId, StudentId) .
עמוד ה נתונים סוג הסב ר
ApplicationId int (PK) להגשה ייחודי מזהה
JobId int (FK) המועמדו ת הוגשה משרה לאיזו
StudentId int (FK) נלק ח ) התלמידה מזהה מה - (JWT
AppliedAt DateTime המועמדות הוגשה מתי
UpdatedAt DateTime לאחרונה הסטטוס עודכן מתי
Status Enum Pending / Interviewed / Accepted / Rejected CoverLetter string? אופציונ ל ) אישי פנייה מכתב י (
Notes string לתלמיד גלוי לא ) המנהלת של פנימיות הערות ה (
בס"ד 04.03.2026
5
. אישיות הזמנות טבלת - PrivateJobInvitation
הזמנות שהמנהלת שולחת לתלמידות ספציפיות למשרות פרטיות.
עמוד ה נתונים סוג הסב ר
InvitationId int (PK) להזמנה ייחודי מזהה
JobId int (FK) הפרטי ת המשרה מזהה
StudentId int (FK) שהוזמנ ה התלמידה מזהה
InvitedAt DateTime ההזמנה שנשלחה ושעה תאריך
IsViewed boolean המשרה את פתחה התלמידה האם ?
Enums
Enum אפשריים ערכים
JobType FullTime / PartTime / Internship Field Development / QA / DevOps JobStatus Open / Closed / Pending ApplicationStatus Pending / Interviewed / Accepted / Rejected
בס"ד 04.03.2026
פעולות ה - API ( Controllers )
1
. Jobs Controller – ניהול משרות
פעול ה Method Route הסב ר הרשא ה
שליפ ת
משרות
ציבוריות
GET /api/jobs? pageNumber=1&pageSize=10
פעילות משרות כל
פרטיות שאינן +
Pagination
כול ם
חיפוש
וסינון
GET /api/jobs/search? title=dev&pageNumber= 1&pageSize=10 תגיות לפי סינון ,
מיקום , שדה ,
חופש י טקסט +
Pagination
כול ם
שליפ ת
משרה
ספציפי ת
GET /api/jobs/{id} משרה פרטי כל
תגיו ת כול ל אחת
כול ם
יצירת
משרה
חדש ה
POST /api/jobs מזינה המנהלת
ומסמנת פרטים
IsPrivate אם
רלוונט י
מנהל ת
עדכון
פרטי
משרה
PUT /api/jobs/{id} תיאור עדכון ,
סטטוס , דרישות
מנהל ת
מחיקת
משרה
DELETE /api/jobs/{id} רכה מחיקה - IsActive=false מנהל ת
בס"ד 04.03.2026
2
. Invitations Controller - אישיו ת הזמנות
פעול ה Method Route הסב ר הרשא ה
שליחת
הזמנה
מרובה
POST /api/invitations/bulk מקבלת JobId + רשימת StudentIds, אח ת לכ ל שורה יוצרת
מנהל ת
שליפ ת
הזמנות
לתלמיד ה
GET /api/invitations/my הפרטיו ת המשרו ת כל
הוזמנה שהתלמידה
אליהן
תלמידה
-כ סימון
נקרא
PATCH /api/invitations/{id}/view מעדכנ ת
IsViewed=true
פותח ת כשהתלמידה
תלמידה
3
. Applications Controller – מועמדויות
פעול ה Method Route הסב ר הרשא ה
הגשת
מועמדות
POST /api/applications שורה יצירת
חדשה - StudentId נלקח
מה -JWT תלמידה
המועמדויות
שלי
GET /api/applications/my רואה התלמידה
משרות לאילו
ומה הגישה
הסטטוס
תלמידה
צפייה
במועמדות
למשרה
GET /api/jobs/{id}/applications כ ל רואה המנהלת
שהגישו הבנות
ספציפי ת למשרה
מנהל ת
עדכון
סטטוס
PATCH /api/applications/{id}/status ראיון עברה /
נדחת ה / התקבלה
מנהל ת
עריכת
הערות
מנהל ת
PATCH
/api/applications/{id}/notes
עדכון הערות
פנימיות של
המנהלת על
המועמדת .
מנהל ת
בס"ד 04.03.2026
פונקציות ה – Service
הלוגיקה העסקית – הפונקציות שה- Controllers קוראות להן.
פונקציה תיאו ר ל ה קורא מי
GetAllPublicJobs() פרטיות ולא פעילות משרות מחזירה
Jobs Controller - GET /api/jobs SearchJobs(filters) תגיו ת + שוני ם שדות לפי דינמי סינון
Jobs Controller - GET /api/jobs/search GetJobById(id) תגיותיה כל ע ם בודדת משרה שליפ ת
Jobs Controller - GET /api/jobs/{id} CreateJob(dto) תגיות שיוך + חדשה משרה יצירת
Jobs Controller - POST /api/jobs UpdateJob(id, dto) קיימ ת משרה שדו ת עדכון
Jobs Controller - PUT /api/jobs/{id} DeleteJob(id) רכה מחיקה - IsActive=false Jobs Controller - DELETE /api/jobs/{id} SendInvitations(jobId, studentIds) לשירו ת קריאה + הזמנות יצירת
ההודעות
Invitations Controller - POST bulk GetMyInvitations(studentId) ספציפי ת לתלמידה פרטיות הזמנות
Invitations Controller - GET my MarkInvitationViewed(id) עדכון IsViewed=true Invitations Controller - PATCH view ApplyToJob(jobId, studentId, dto) כפילות בדיקת + מועמדות הגשת +
מיי ל שליחת
Applications Controller - POST GetMyApplications(studentId) סטטוסים + תלמידה של מועמדויות
Applications Controller - GET my
בס"ד 04.03.2026
פונקציה תיאו ר ל ה קורא מי
GetApplicationsForJob(jobId) אח ת למשרה המועמדויות כל
Applications Controller - GET by job UpdateApplicationStatus(id, status) מועמדות סטטוס עדכון + UpdatedAt Applications Controller - PATCH status CheckDeadlines() Background Job - משרות סוגר
שעבר Deadline שלהן
Scheduled Task / Cron
חשובות הערות
נושא הער ה
אבטחה StudentId בהגשת מועמדות נלקח מה- JWT - לא מגוף הבקשה
כפילויו ת Unique Constraint על ) JobId, StudentId ( בטבלת Application
רכה מחיקה DELETE לא מוחק שורה אלא מעדכן IsActive=false Deadline Background Job יסגור אוטומטית משרות שעבר תאריך ה- Deadline מיילים
הגשת מועמדות ושליחת הזמנות שולחות HTTP לצוות 6 ( Notifications Service ( יש לתאם את מבנה ה -JSON מול צוות 6 לצורך שליחת המייל .

Jobs Service - חלוקת משימות לבניית השרת
צוות מספר 4 | .NET 8 + EF Core + SQL Server
Architecture: Logical 3-Tier | Controllers → Services → Repositories

מבנה התיקיות - Logical 3-Tier Architecture

📁 תיקייה	📝 תוכן	👩 אחראית
Data/	Entities (מודלים), AppDbContext, Migrations	בחורה 1
BLL/Repositories/	Interfaces + Implementations לכל Entity	בחורות 2, 3, 4
BLL/Services/	לוגיקה עסקית	בחורות 2, 3, 4
DTOs/	אובייקטי העברת נתונים (ללא קשר ל-DB)	בחורות 2, 3, 4
Controllers/	Endpoints - HTTP Requests/Responses	בחורות 2, 3, 4


🏗️ בחורה 1 - Infrastructure & Data Layer
הקמת הפרויקט + שכבת הנתונים
Task	שם המשימה	פירוט
Task 1.1	יצירת פרויקט .NET 8 Web API	יצירת הפרויקט + תיקיות: Data, BLL, DTOs, Controllers
Task 1.2	התקנת Packages	EF Core, AutoMapper, SQL Server, Swagger
Task 1.3	Entities - משרות ותגיות	כתיבת Job, Tag, JobTag עם כל השדות
Task 1.4	Entities - מועמדויות והזמנות	כתיבת Application, PrivateJobInvitation
Task 1.5	כתיבת כל ה-Enums	JobType, Field, JobStatus, ApplicationStatus
Task 1.6	AppDbContext	DbSets, Unique Constraint על (JobId, StudentId), Relationships
Task 1.7	Migration ראשונה	appsettings.json + Connection String + dotnet ef migrations add Init

 
💼 בחורה 2 - Jobs Module
כל מה שקשור למשרות
Task	שם המשימה	פירוט
Task 2.1	DTOs למשרות	JobDto, CreateJobDto, UpdateJobDto, JobSearchFiltersDto
Task 2.2	IJobRepository	הגדרת ה-Interface עם כל הפונקציות
Task 2.3	JobRepository - קריאה	GetAllPublicJobs(), GetJobById(id), SearchJobs(filters)
Task 2.4	JobRepository - כתיבה	CreateJob(dto), UpdateJob(id,dto), DeleteJob(id) - soft delete
Task 2.5	JobService	לוגיקה עסקית + קריאה ל-Repository
Task 2.6	JobsController	5 Endpoints: GET all, GET search, GET by id, POST, PUT, DELETE
Task 2.7	רישום ב-Program.cs + בדיקות	DI Registration, AutoMapper Profile, בדיקות ב-Postman

 
📋 בחורה 3 - Applications Module
כל מה שקשור למועמדויות
Task	שם המשימה	פירוט
Task 3.1	DTOs למועמדויות	ApplicationDto, CreateApplicationDto, UpdateStatusDto, UpdateNotesDto
Task 3.2	IApplicationRepository	הגדרת ה-Interface עם כל הפונקציות
Task 3.3	ApplicationRepository - קריאה	GetMyApplications(studentId), GetApplicationsForJob(jobId)
Task 3.4	ApplicationRepository - כתיבה	ApplyToJob (+ בדיקת כפילות), UpdateApplicationStatus, UpdateNotes
Task 3.5	ApplicationService	לוגיקה עסקית + שליפת StudentId מה-JWT
Task 3.6	ApplicationsController	5 Endpoints: POST apply, GET my, GET by job, PATCH status, PATCH notes
Task 3.7	בדיקות Postman	בדיקת כל ה-Endpoints כולל מקרי קצה (כפילות, JWT)

 
✉️ בחורה 4 - Invitations Module
כל מה שקשור להזמנות פרטיות
Task	שם המשימה	פירוט
Task 4.1	DTOs להזמנות	InvitationDto, BulkInvitationDto (JobId + רשימת StudentIds)
Task 4.2	IInvitationRepository	הגדרת ה-Interface עם כל הפונקציות
Task 4.3	InvitationRepository	SendInvitations(bulk), GetMyInvitations(studentId), MarkInvitationViewed(id)
Task 4.4	InvitationService	לוגיקה עסקית + קריאה ל-Repository
Task 4.5	InvitationsController	3 Endpoints: POST bulk, GET my, PATCH view
Task 4.6	רישום ב-Program.cs + בדיקות	DI Registration, AutoMapper Profile, בדיקות ב-Postman

 
