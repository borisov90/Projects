delete AcademicDegrees;
delete Qualification;
delete LecturerDiscipline;
delete LecturerPaymentDetail;
delete LecturerPayment;
delete LecturerTimeSheetDetail;
delete LecturerTimeSheet;
delete LecturerReport;
delete ContractLecturerDiscipline;
delete [Contract];
delete StreamGroupDetailsLecturers;
delete DisciplineNameLecturer;
delete DepartmentLecturer;
delete LanguageSchedule;
delete PreparationCourseLecturerLink;
delete StudentPractice;
--update PhDAdmission;

--disable the CK_Customer_CustomerType constraint 
ALTER TABLE Lecturer NOCHECK CONSTRAINT FK_Lecturer_Person;

delete UserRoleLink 
from UserRoleLink ul 
inner join [user] u on ul.idUser = u.idUser
inner join Lecturer l on u.idPerson = l.idPerson


delete [user] 
from [user] u 
inner join Lecturer l on u.idPerson = l.idPerson

update  d 
set d.idPerson = null
from Department d
inner join Person p on d.idPerson = p.idPerson
inner join Lecturer l on p.idPerson = l.idPerson

delete Person from Person p inner join Lecturer l on p.idPerson = l.idPerson;
delete from Lecturer;


ALTER TABLE Lecturer CHECK CONSTRAINT FK_Lecturer_Person;


