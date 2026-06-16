˙

^C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IUserService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IUserService !
{ 
Task 
< 
User 
? 
> 
GetUserByIdAsync $
($ %
int% (
id) +
)+ ,
;, -
Task

 
<

 
User

 
?

 
>

 
GetUserByEmailAsync

 '
(

' (
string

( .
email

/ 4
)

4 5
;

5 6
Task 
< 
IEnumerable 
< 
User 
> 
> 
GetUsersByRoleAsync  3
(3 4
UserRole4 <
role= A
)A B
;B C
Task 
RegisterAsync 
( 
User 
user  $
,$ %
string& ,
password- 5
)5 6
;6 7
Task 
< 
User 
? 
> 

LoginAsync 
( 
string %
email& +
,+ ,
string- 3
password4 <
)< =
;= >
} 
} ù

aC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IPatientService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IPatientService $
{ 
Task 
< 
IEnumerable 
< 
Patient  
>  !
>! "
GetAllPatientsAsync# 6
(6 7
)7 8
;8 9
Task		 
<		 
Patient		 
?		 
>		 
GetPatientByIdAsync		 *
(		* +
int		+ .
id		/ 1
)		1 2
;		2 3
Task 
< 
IEnumerable 
< 
Patient  
>  !
>! "
SearchPatientsAsync# 6
(6 7
string7 =
name> B
)B C
;C D
Task 
AddPatientAsync 
( 
Patient $
patient% ,
), -
;- .
Task 
UpdatePatientAsync 
(  
int  #
id$ &
,& '
Patient( /
patient0 7
)7 8
;8 9
} 
} ‘
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IHealthRecordService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface  
IHealthRecordService )
{ 
Task 
< 
HealthRecord 
? 
> 
GetByIdAsync (
(( )
int) ,
id- /
)/ 0
;0 1
Task		 
<		 
HealthRecord		 
?		 
>		 #
GetByAppointmentIdAsync		 3
(		3 4
int		4 7
appointmentId		8 E
)		E F
;		F G
Task  
AddHealthRecordAsync !
(! "
HealthRecord" .
record/ 5
)5 6
;6 7
Task #
UpdateHealthRecordAsync $
($ %
int% (
id) +
,+ ,
HealthRecord- 9
record: @
)@ A
;A B
} 
} ´
`C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IDoctorService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IDoctorService #
{ 
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !
GetAllDoctorsAsync" 4
(4 5
)5 6
;6 7
Task		 
<		 
IEnumerable		 
<		 
Doctor		 
>		  
>		  !+
GetDoctorsBySpecialisationAsync		" A
(		A B
int		B E
specialisation		F T
)		T U
;		U V
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !1
%GetActiveDoctorsBySpecialisationAsync" G
(G H
intH K
specialisationL Z
)Z [
;[ \
Task 
< 
Doctor 
? 
> 
GetDoctorByIdAsync (
(( )
int) ,
id- /
)/ 0
;0 1
Task 
AddDoctorAsync 
( 
Doctor "
doctor# )
)) *
;* +
Task 
UpdateDoctorAsync 
( 
int "
id# %
,% &
Doctor' -
doctor. 4
)4 5
;5 6
} 
} π
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IAppointmentService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IAppointmentService (
{ 
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &#
GetAllAppointmentsAsync' >
(> ?
)? @
;@ A
Task		 
<		 
Appointment		 
?		 
>		 #
GetAppointmentByIdAsync		 2
(		2 3
int		3 6
id		7 9
)		9 :
;		: ;
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &)
GetAppointmentsByPatientAsync' D
(D E
intE H
	patientIdI R
)R S
;S T
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &'
GetDoctorTodayScheduleAsync' B
(B C
intC F
doctorIdG O
)O P
;P Q
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &&
GetDoctorWeekScheduleAsync' A
(A B
intB E
doctorIdF N
)N O
;O P
Task  
BookAppointmentAsync !
(! "
Appointment" -
appointment. 9
)9 :
;: ;
Task "
CancelAppointmentAsync #
(# $
int$ '
appointmentId( 5
,5 6
string7 =
reason> D
)D E
;E F
Task $
CompleteAppointmentAsync %
(% &
int& )
appointmentId* 7
)7 8
;8 9
Task "
UpdateAppointmentAsync #
(# $
int$ '
appointmentId( 5
,5 6
Appointment7 B
appointmentC N
)N O
;O P
} 
} –
cC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Interface\IUserRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IUserRepository $
{ 
Task 
< 
User 
? 
> 
GetByIdAsync  
(  !
int! $
id% '
)' (
;( )
Task		 
<		 
User		 
?		 
>		 
GetByEmailAsync		 #
(		# $
string		$ *
email		+ 0
)		0 1
;		1 2
Task

 
<

 
IEnumerable

 
<

 
User

 
>

 
>

 
GetByRoleAsync

  .
(

. /
UserRole

/ 7
role

8 <
)

< =
;

= >
Task 
AddAsync 
( 
User 
user 
)  
;  !
Task 
UpdateAsync 
( 
User 
user "
)" #
;# $
Task 
< 
bool 
> 
EmailExistsAsync #
(# $
string$ *
email+ 0
)0 1
;1 2
Task 
SaveChangesAsync 
( 
) 
;  
} 
} ﬁ
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Interface\IPatientRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IPatientRepository '
{ 
Task 
< 
IEnumerable 
< 
Patient  
>  !
>! "
GetAllAsync# .
(. /
)/ 0
;0 1
Task 
< 
Patient 
? 
> 
GetByIdAsync #
(# $
int$ '
id( *
)* +
;+ ,
Task		 
<		 
IEnumerable		 
<		 
Patient		  
>		  !
>		! "
SearchByNameAsync		# 4
(		4 5
string		5 ;
name		< @
)		@ A
;		A B
Task

 
AddAsync

 
(

 
Patient

 
patient

 %
)

% &
;

& '
Task 
UpdateAsync 
( 
Patient  
patient! (
)( )
;) *
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
SaveChangesAsync 
( 
) 
;  
} 
} Û
kC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Interface\IHealthRecordRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface #
IHealthRecordRepository ,
{ 
Task 
< 
HealthRecord 
? 
> 
GetByIdAsync (
(( )
int) ,
id- /
)/ 0
;0 1
Task 
< 
HealthRecord 
? 
> #
GetByAppointmentIdAsync 3
(3 4
int4 7
appointmentId8 E
)E F
;F G
Task		 
AddAsync		 
(		 
HealthRecord		 "
record		# )
)		) *
;		* +
Task

 
UpdateAsync

 
(

 
HealthRecord

 %
record

& ,
)

, -
;

- .
Task 
SaveChangesAsync 
( 
) 
;  
} 
} Î
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Interface\IDoctorRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IDoctorRepository &
{ 
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !
GetAllAsync" -
(- .
string. 4
?4 5
sortBy6 <
,< =
int> A
?A B
specialisationC Q
)Q R
;R S
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !*
GetActiveBySpecialisationAsync" @
(@ A
intA D
specialisationE S
)S T
;T U
Task		 
<		 
Doctor		 
?		 
>		 
GetByIdAsync		 "
(		" #
int		# &
id		' )
)		) *
;		* +
Task

 
AddAsync

 
(

 
Doctor

 
doctor

 #
)

# $
;

$ %
Task 
UpdateAsync 
( 
Doctor 
doctor  &
)& '
;' (
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
SaveChangesAsync 
( 
) 
;  
} 
} ’
jC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Interface\IAppointmentRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface "
IAppointmentRepository +
{ 
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &
GetAllAsync' 2
(2 3
)3 4
;4 5
Task 
< 
Appointment 
? 
> 
GetByIdAsync '
(' (
int( +
id, .
). /
;/ 0
Task		 
<		 
IEnumerable		 
<		 
Appointment		 $
>		$ %
>		% &
GetByPatientIdAsync		' :
(		: ;
int		; >
	patientId		? H
)		H I
;		I J
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &'
GetDoctorTodayScheduleAsync' B
(B C
intC F
doctorIdG O
,O P
DateOnlyQ Y
todayZ _
)_ `
;` a
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &&
GetDoctorWeekScheduleAsync' A
(A B
intB E
doctorIdF N
,N O
DateOnlyP X
	startDateY b
,b c
DateOnlyd l
endDatem t
)t u
;u v
Task 
< 
bool 
> 4
(ExistsSamePatientSameDoctorSameDateAsync ;
(; <
int< ?
	patientId@ I
,I J
intK N
doctorIdO W
,W X
DateOnlyY a
dateb f
)f g
;g h
Task 
< 
bool 
> 2
&ExistsSamePatientSameSlotSameDateAsync 9
(9 :
int: =
	patientId> G
,G H
DateOnlyI Q
dateR V
,V W
intX [
timeSlot\ d
)d e
;e f
Task 
< 
bool 
> 1
%ExistsSameDoctorSameSlotSameDateAsync 8
(8 9
int9 <
doctorId= E
,E F
DateOnlyG O
dateP T
,T U
intV Y
timeSlotZ b
)b c
;c d
Task 
AddAsync 
( 
Appointment !
appointment" -
)- .
;. /
Task 
UpdateAsync 
( 
Appointment $
appointment% 0
)0 1
;1 2
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
SaveChangesAsync 
( 
) 
;  
} 
} Á%
jC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Implementation\PatientRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public 

class 
PatientRepository "
:# $
IPatientRepository% 7
{		 
private

 
readonly

 
HealthAxisDbContext

 ,
_context

- 5
;

5 6
public 
PatientRepository  
(  !
HealthAxisDbContext! 4
context5 <
)< =
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Patient& -
>- .
>. /
GetAllAsync0 ;
(; <
)< =
{ 	
return 
await 
_context !
.! "
Patients" *
. 
OrderBy 
( 
p 
=> 
p 
.  
	PatientId  )
)) *
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
Patient !
?! "
>" #
GetByIdAsync$ 0
(0 1
int1 4
id5 7
)7 8
{ 	
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
	PatientId, 5
==6 8
id9 ;
); <
;< =
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Patient& -
>- .
>. /
SearchByNameAsync0 A
(A B
stringB H
nameI M
)M N
{ 	
if   
(   
string   
.   
IsNullOrWhiteSpace   )
(  ) *
name  * .
)  . /
)  / 0
return!! 
new!! 
List!! 
<!!  
Patient!!  '
>!!' (
(!!( )
)!!) *
;!!* +
name## 
=## 
name## 
.## 
Trim## 
(## 
)## 
;## 
return%% 
await%% 
_context%% !
.%%! "
Patients%%" *
.&& 
Where&& 
(&& 
p&& 
=>&& 
p&& 
.&& 
IsActive&& &
&&&&' )
p&&* +
.&&+ ,
FullName&&, 4
.&&4 5
Contains&&5 =
(&&= >
name&&> B
)&&B C
)&&C D
.'' 
OrderBy'' 
('' 
p'' 
=>'' 
p'' 
.''  
FullName''  (
)''( )
.(( 
ToListAsync(( 
((( 
)(( 
;(( 
})) 	
public++ 
async++ 
Task++ 
AddAsync++ "
(++" #
Patient++# *
patient+++ 2
)++2 3
{,, 	
await-- 
_context-- 
.-- 
Patients-- #
.--# $
AddAsync--$ ,
(--, -
patient--- 4
)--4 5
;--5 6
}.. 	
public00 
Task00 
UpdateAsync00 
(00  
Patient00  '
patient00( /
)00/ 0
{11 	
_context22 
.22 
Patients22 
.22 
Update22 $
(22$ %
patient22% ,
)22, -
;22- .
return33 
Task33 
.33 
CompletedTask33 %
;33% &
}44 	
public66 
async66 
Task66 
<66 
bool66 
>66 
ExistsAsync66  +
(66+ ,
int66, /
id660 2
)662 3
{77 	
return88 
await88 
_context88 !
.88! "
Patients88" *
.88* +
AnyAsync88+ 3
(883 4
p884 5
=>886 8
p889 :
.88: ;
	PatientId88; D
==88E G
id88H J
)88J K
;88K L
}99 	
public;; 
async;; 
Task;; 
SaveChangesAsync;; *
(;;* +
);;+ ,
{<< 	
await== 
_context== 
.== 
SaveChangesAsync== +
(==+ ,
)==, -
;==- .
}>> 	
}?? 
}@@ ”
gC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Implementation\UserRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public		 

class		 
UserRepository		 
:		  !
IUserRepository		" 1
{

 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
UserRepository 
( 
HealthAxisDbContext 1
context2 9
)9 :
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
User 
? 
>  
GetByIdAsync! -
(- .
int. 1
id2 4
)4 5
{ 	
return 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync $
($ %
u% &
=>' )
u* +
.+ ,
UserId, 2
==3 5
id6 8
)8 9
;9 :
} 	
public 
async 
Task 
< 
User 
? 
>  
GetByEmailAsync! 0
(0 1
string1 7
email8 =
)= >
{ 	
return 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync $
($ %
u% &
=>' )
u* +
.+ ,
Email, 1
==2 4
email5 :
): ;
;; <
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
User& *
>* +
>+ ,
GetByRoleAsync- ;
(; <
UserRole< D
roleE I
)I J
{ 	
return   
await   
_context   !
.  ! "
Users  " '
.!! 
Where!! 
(!! 
u!! 
=>!! 
u!! 
.!! 
Role!! "
==!!# %
role!!& *
)!!* +
."" 
ToListAsync"" 
("" 
)"" 
;"" 
}## 	
public%% 
async%% 
Task%% 
AddAsync%% "
(%%" #
User%%# '
user%%( ,
)%%, -
{&& 	
await'' 
_context'' 
.'' 
Users''  
.''  !
AddAsync''! )
('') *
user''* .
)''. /
;''/ 0
}(( 	
public** 
Task** 
UpdateAsync** 
(**  
User**  $
user**% )
)**) *
{++ 	
_context,, 
.,, 
Users,, 
.,, 
Update,, !
(,,! "
user,," &
),,& '
;,,' (
return-- 
Task-- 
.-- 
CompletedTask-- %
;--% &
}.. 	
public00 
async00 
Task00 
<00 
bool00 
>00 
EmailExistsAsync00  0
(000 1
string001 7
email008 =
)00= >
{11 	
return22 
await22 
_context22 !
.22! "
Users22" '
.33 
AnyAsync33 
(33 
u33 
=>33 
u33  
.33  !
Email33! &
==33' )
email33* /
)33/ 0
;330 1
}44 	
public66 
async66 
Task66 
SaveChangesAsync66 *
(66* +
)66+ ,
{77 	
await88 
_context88 
.88 
SaveChangesAsync88 +
(88+ ,
)88, -
;88- .
}99 	
}:: 
};; —
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Implementation\HealthRecord.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public 

class "
HealthRecordRepository '
:( )#
IHealthRecordRepository* A
{		 
private

 
readonly

 
HealthAxisDbContext

 ,
_context

- 5
;

5 6
public "
HealthRecordRepository %
(% &
HealthAxisDbContext& 9
context: A
)A B
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
HealthRecord &
?& '
>' (
GetByIdAsync) 5
(5 6
int6 9
id: <
)< =
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Include 
( 
hr 
=> 
hr !
.! "
Patient" )
)) *
. 
Include 
( 
hr 
=> 
hr !
.! "
Doctor" (
)( )
. 
Include 
( 
hr 
=> 
hr !
.! "
Appointment" -
)- .
. 
FirstOrDefaultAsync $
($ %
hr% '
=>( *
hr+ -
.- .
HealthRecordId. <
=== ?
id@ B
)B C
;C D
} 	
public 
async 
Task 
< 
HealthRecord &
?& '
>' (#
GetByAppointmentIdAsync) @
(@ A
intA D
appointmentIdE R
)R S
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Include 
( 
hr 
=> 
hr !
.! "
Patient" )
)) *
. 
Include 
( 
hr 
=> 
hr !
.! "
Doctor" (
)( )
. 
Include 
( 
hr 
=> 
hr !
.! "
Appointment" -
)- .
.   
FirstOrDefaultAsync   $
(  $ %
hr  % '
=>  ( *
hr  + -
.  - .
AppointmentId  . ;
==  < >
appointmentId  ? L
)  L M
;  M N
}!! 	
public## 
async## 
Task## 
AddAsync## "
(##" #
HealthRecord### /
record##0 6
)##6 7
{$$ 	
await%% 
_context%% 
.%% 
HealthRecords%% (
.%%( )
AddAsync%%) 1
(%%1 2
record%%2 8
)%%8 9
;%%9 :
}&& 	
public(( 
Task(( 
UpdateAsync(( 
(((  
HealthRecord((  ,
record((- 3
)((3 4
{)) 	
_context** 
.** 
HealthRecords** "
.**" #
Update**# )
(**) *
record*** 0
)**0 1
;**1 2
return++ 
Task++ 
.++ 
CompletedTask++ %
;++% &
},, 	
public.. 
async.. 
Task.. 
SaveChangesAsync.. *
(..* +
)..+ ,
{// 	
await00 
_context00 
.00 
SaveChangesAsync00 +
(00+ ,
)00, -
;00- .
}11 	
}22 
}33 £7
iC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Implementation\DoctorRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public		 

class		 
DoctorRepository		 !
:		" #
IDoctorRepository		$ 5
{

 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
DoctorRepository 
(  
HealthAxisDbContext  3
context4 ;
); <
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Doctor& ,
>, -
>- .
GetAllAsync/ :
(: ;
string; A
?A B
sortByC I
,I J
intK N
?N O
specialisationP ^
)^ _
{ 	

IQueryable 
< 
Doctor 
> 
query $
=% &
_context' /
./ 0
Doctors0 7
;7 8
if 
( 
specialisation 
. 
HasValue '
)' (
{ 
if 
( 
! 
Enum 
. 
	IsDefined #
(# $
typeof$ *
(* + 
DoctorSpecialisation+ ?
)? @
,@ A
specialisationB P
.P Q
ValueQ V
)V W
)W X
throw 
new 
ArgumentException /
(/ 0
$str0 P
)P Q
;Q R
var  
doctorSpecialisation (
=) *
(+ , 
DoctorSpecialisation, @
)@ A
specialisationA O
.O P
ValueP U
;U V
query 
= 
query 
. 
Where #
(# $
d$ %
=>& (
d) *
.* +
Specialisation+ 9
==: < 
doctorSpecialisation= Q
)Q R
;R S
} 
query 
= 
sortBy 
? 
. 
ToLower #
(# $
)$ %
switch& ,
{   
$str!! 
=>!! 
query!! 
.!!  
OrderBy!!  '
(!!' (
d!!( )
=>!!* ,
d!!- .
.!!. /
FullName!!/ 7
)!!7 8
,!!8 9
$str"" 
=>"" 
query"" $
.""$ %
OrderByDescending""% 6
(""6 7
d""7 8
=>""9 ;
d""< =
.""= >
FullName""> F
)""F G
,""G H
$str## 
=>## 
query## 
.## 
OrderBy## %
(##% &
d##& '
=>##( *
d##+ ,
.##, -
DoctorId##- 5
)##5 6
,##6 7
_$$ 
=>$$ 
query$$ 
.$$ 
OrderBy$$ "
($$" #
d$$# $
=>$$% '
d$$( )
.$$) *
DoctorId$$* 2
)$$2 3
}%% 
;%% 
return'' 
await'' 
query'' 
.'' 
ToListAsync'' *
(''* +
)''+ ,
;'', -
}(( 	
public** 
async** 
Task** 
<** 
IEnumerable** %
<**% &
Doctor**& ,
>**, -
>**- .*
GetActiveBySpecialisationAsync**/ M
(**M N
int**N Q
specialisation**R `
)**` a
{++ 	
if,, 
(,, 
!,, 
Enum,, 
.,, 
	IsDefined,, 
(,,  
typeof,,  &
(,,& ' 
DoctorSpecialisation,,' ;
),,; <
,,,< =
specialisation,,> L
),,L M
),,M N
throw-- 
new-- 
ArgumentException-- +
(--+ ,
$str--, L
)--L M
;--M N
var//  
doctorSpecialisation// $
=//% &
(//' ( 
DoctorSpecialisation//( <
)//< =
specialisation//= K
;//K L
return11 
await11 
_context11 !
.11! "
Doctors11" )
.22 
Where22 
(22 
d22 
=>22 
d22 
.22 
IsActive22 &
&&22' )
d22* +
.22+ ,
Specialisation22, :
==22; = 
doctorSpecialisation22> R
)22R S
.33 
OrderBy33 
(33 
d33 
=>33 
d33 
.33  
FullName33  (
)33( )
.44 
ToListAsync44 
(44 
)44 
;44 
}55 	
public77 
async77 
Task77 
<77 
Doctor77  
?77  !
>77! "
GetByIdAsync77# /
(77/ 0
int770 3
id774 6
)776 7
{88 	
return99 
await99 
_context99 !
.99! "
Doctors99" )
.99) *
	FindAsync99* 3
(993 4
id994 6
)996 7
;997 8
}:: 	
public<< 
async<< 
Task<< 
AddAsync<< "
(<<" #
Doctor<<# )
doctor<<* 0
)<<0 1
{== 	
await>> 
_context>> 
.>> 
Doctors>> "
.>>" #
AddAsync>># +
(>>+ ,
doctor>>, 2
)>>2 3
;>>3 4
}?? 	
publicAA 
TaskAA 
UpdateAsyncAA 
(AA  
DoctorAA  &
doctorAA' -
)AA- .
{BB 	
_contextCC 
.CC 
DoctorsCC 
.CC 
UpdateCC #
(CC# $
doctorCC$ *
)CC* +
;CC+ ,
returnDD 
TaskDD 
.DD 
CompletedTaskDD %
;DD% &
}EE 	
publicGG 
asyncGG 
TaskGG 
<GG 
boolGG 
>GG 
ExistsAsyncGG  +
(GG+ ,
intGG, /
idGG0 2
)GG2 3
{HH 	
returnII 
awaitII 
_contextII !
.II! "
DoctorsII" )
.II) *
AnyAsyncII* 2
(II2 3
dII3 4
=>II5 7
dII8 9
.II9 :
DoctorIdII: B
==IIC E
idIIF H
)IIH I
;III J
}JJ 	
publicLL 
asyncLL 
TaskLL 
SaveChangesAsyncLL *
(LL* +
)LL+ ,
{MM 	
awaitNN 
_contextNN 
.NN 
SaveChangesAsyncNN +
(NN+ ,
)NN, -
;NN- .
}OO 	
}PP 
}QQ ·_
nC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repository\Implementation\AppointmentRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{		 
public

 

class

 !
AppointmentRepository

 &
:

' ("
IAppointmentRepository

) ?
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public !
AppointmentRepository $
($ %
HealthAxisDbContext% 8
context9 @
)@ A
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Appointment& 1
>1 2
>2 3
GetAllAsync4 ?
(? @
)@ A
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Include 
( 
a 
=> 
a 
.  
Patient  '
)' (
. 
Include 
( 
a 
=> 
a 
.  
Doctor  &
)& '
. 
OrderByDescending "
(" #
a# $
=>% '
a( )
.) *
ScheduledDate* 7
)7 8
. 
ThenBy 
( 
a 
=> 
a 
. 
TimeSlot '
)' (
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
Appointment %
?% &
>& '
GetByIdAsync( 4
(4 5
int5 8
id9 ;
); <
{ 	
return 
await 
_context !
.! "
Appointments" .
.   
Include   
(   
a   
=>   
a   
.    
Patient    '
)  ' (
.!! 
Include!! 
(!! 
a!! 
=>!! 
a!! 
.!!  
Doctor!!  &
)!!& '
."" 
FirstOrDefaultAsync"" $
(""$ %
a""% &
=>""' )
a""* +
.""+ ,
AppointmentId"", 9
=="": <
id""= ?
)""? @
;""@ A
}## 	
public%% 
async%% 
Task%% 
<%% 
IEnumerable%% %
<%%% &
Appointment%%& 1
>%%1 2
>%%2 3
GetByPatientIdAsync%%4 G
(%%G H
int%%H K
	patientId%%L U
)%%U V
{&& 	
return'' 
await'' 
_context'' !
.''! "
Appointments''" .
.(( 
Include(( 
((( 
a(( 
=>(( 
a(( 
.((  
Doctor((  &
)((& '
.)) 
Where)) 
()) 
a)) 
=>)) 
a)) 
.)) 
	PatientId)) '
==))( *
	patientId))+ 4
)))4 5
.** 
OrderByDescending** "
(**" #
a**# $
=>**% '
a**( )
.**) *
ScheduledDate*** 7
)**7 8
.++ 
ThenBy++ 
(++ 
a++ 
=>++ 
a++ 
.++ 
TimeSlot++ '
)++' (
.,, 
ToListAsync,, 
(,, 
),, 
;,, 
}-- 	
public// 
async// 
Task// 
<// 
IEnumerable// %
<//% &
Appointment//& 1
>//1 2
>//2 3'
GetDoctorTodayScheduleAsync//4 O
(//O P
int//P S
doctorId//T \
,//\ ]
DateOnly//^ f
today//g l
)//l m
{00 	
return11 
await11 
_context11 !
.11! "
Appointments11" .
.22 
Include22 
(22 
a22 
=>22 
a22 
.22  
Patient22  '
)22' (
.33 
Where33 
(33 
a33 
=>33 
a33 
.33 
DoctorId33 &
==33' )
doctorId33* 2
&&333 5
a336 7
.337 8
ScheduledDate338 E
==33F H
today33I N
)33N O
.44 
OrderBy44 
(44 
a44 
=>44 
a44 
.44  
TimeSlot44  (
)44( )
.55 
ToListAsync55 
(55 
)55 
;55 
}66 	
public88 
async88 
Task88 
<88 
IEnumerable88 %
<88% &
Appointment88& 1
>881 2
>882 3&
GetDoctorWeekScheduleAsync884 N
(88N O
int88O R
doctorId88S [
,88[ \
DateOnly88] e
	startDate88f o
,88o p
DateOnly88q y
endDate	88z Å
)
88Å Ç
{99 	
return:: 
await:: 
_context:: !
.::! "
Appointments::" .
.;; 
Include;; 
(;; 
a;; 
=>;; 
a;; 
.;;  
Patient;;  '
);;' (
.<< 
Where<< 
(<< 
a<< 
=><< 
a<< 
.<< 
DoctorId<< &
==<<' )
doctorId<<* 2
&&<<3 5
a== 
.== 
ScheduledDate== +
>===, .
	startDate==/ 8
&&==9 ;
a>> 
.>> 
ScheduledDate>> +
<=>>, .
endDate>>/ 6
)>>6 7
.?? 
OrderBy?? 
(?? 
a?? 
=>?? 
a?? 
.??  
ScheduledDate??  -
)??- .
.@@ 
ThenBy@@ 
(@@ 
a@@ 
=>@@ 
a@@ 
.@@ 
TimeSlot@@ '
)@@' (
.AA 
ToListAsyncAA 
(AA 
)AA 
;AA 
}BB 	
publicDD 
asyncDD 
TaskDD 
<DD 
boolDD 
>DD 4
(ExistsSamePatientSameDoctorSameDateAsyncDD  H
(DDH I
intDDI L
	patientIdDDM V
,DDV W
intDDX [
doctorIdDD\ d
,DDd e
DateOnlyDDf n
dateDDo s
)DDs t
{EE 	
returnFF 
awaitFF 
_contextFF !
.FF! "
AppointmentsFF" .
.FF. /
AnyAsyncFF/ 7
(FF7 8
aFF8 9
=>FF: <
aGG 
.GG 
	PatientIdGG 
==GG 
	patientIdGG (
&&GG) +
aHH 
.HH 
DoctorIdHH 
==HH 
doctorIdHH &
&&HH' )
aII 
.II 
ScheduledDateII 
==II  "
dateII# '
&&II( *
aJJ 
.JJ 
StatusJJ 
!=JJ 
AppointmentStatusJJ -
.JJ- .
	CancelledJJ. 7
)JJ7 8
;JJ8 9
}KK 	
publicMM 
asyncMM 
TaskMM 
<MM 
boolMM 
>MM 2
&ExistsSamePatientSameSlotSameDateAsyncMM  F
(MMF G
intMMG J
	patientIdMMK T
,MMT U
DateOnlyMMV ^
dateMM_ c
,MMc d
intMMe h
timeSlotMMi q
)MMq r
{NN 	
ifOO 
(OO 
!OO 
EnumOO 
.OO 
	IsDefinedOO 
(OO  
typeofOO  &
(OO& '
AppointmentTimeSlotOO' :
)OO: ;
,OO; <
timeSlotOO= E
)OOE F
)OOF G
throwPP 
newPP 
ArgumentExceptionPP +
(PP+ ,
$strPP, L
)PPL M
;PPM N
varRR 
slotEnumRR 
=RR 
(RR 
AppointmentTimeSlotRR /
)RR/ 0
timeSlotRR0 8
;RR8 9
returnTT 
awaitTT 
_contextTT !
.TT! "
AppointmentsTT" .
.TT. /
AnyAsyncTT/ 7
(TT7 8
aTT8 9
=>TT: <
aUU 
.UU 
	PatientIdUU 
==UU 
	patientIdUU (
&&UU) +
aVV 
.VV 
ScheduledDateVV 
==VV  "
dateVV# '
&&VV( *
aWW 
.WW 
TimeSlotWW 
==WW 
slotEnumWW &
&&WW' )
aXX 
.XX 
StatusXX 
!=XX 
AppointmentStatusXX -
.XX- .
	CancelledXX. 7
)XX7 8
;XX8 9
}YY 	
public[[ 
async[[ 
Task[[ 
<[[ 
bool[[ 
>[[ 1
%ExistsSameDoctorSameSlotSameDateAsync[[  E
([[E F
int[[F I
doctorId[[J R
,[[R S
DateOnly[[T \
date[[] a
,[[a b
int[[c f
timeSlot[[g o
)[[o p
{\\ 	
if]] 
(]] 
!]] 
Enum]] 
.]] 
	IsDefined]] 
(]]  
typeof]]  &
(]]& '
AppointmentTimeSlot]]' :
)]]: ;
,]]; <
timeSlot]]= E
)]]E F
)]]F G
throw^^ 
new^^ 
ArgumentException^^ +
(^^+ ,
$str^^, L
)^^L M
;^^M N
var`` 
slotEnum`` 
=`` 
(`` 
AppointmentTimeSlot`` /
)``/ 0
timeSlot``0 8
;``8 9
returnbb 
awaitbb 
_contextbb !
.bb! "
Appointmentsbb" .
.bb. /
AnyAsyncbb/ 7
(bb7 8
abb8 9
=>bb: <
acc 
.cc 
DoctorIdcc 
==cc 
doctorIdcc &
&&cc' )
add 
.dd 
ScheduledDatedd 
==dd  "
datedd# '
&&dd( *
aee 
.ee 
TimeSlotee 
==ee 
slotEnumee &
&&ee' )
aff 
.ff 
Statusff 
!=ff 
AppointmentStatusff -
.ff- .
	Cancelledff. 7
)ff7 8
;ff8 9
}gg 	
publicii 
asyncii 
Taskii 
AddAsyncii "
(ii" #
Appointmentii# .
appointmentii/ :
)ii: ;
{jj 	
awaitkk 
_contextkk 
.kk 
Appointmentskk '
.kk' (
AddAsynckk( 0
(kk0 1
appointmentkk1 <
)kk< =
;kk= >
}ll 	
publicnn 
Tasknn 
UpdateAsyncnn 
(nn  
Appointmentnn  +
appointmentnn, 7
)nn7 8
{oo 	
_contextpp 
.pp 
Appointmentspp !
.pp! "
Updatepp" (
(pp( )
appointmentpp) 4
)pp4 5
;pp5 6
returnqq 
Taskqq 
.qq 
CompletedTaskqq %
;qq% &
}rr 	
publictt 
asynctt 
Tasktt 
<tt 
booltt 
>tt 
ExistsAsynctt  +
(tt+ ,
inttt, /
idtt0 2
)tt2 3
{uu 	
returnvv 
awaitvv 
_contextvv !
.vv! "
Appointmentsvv" .
.vv. /
AnyAsyncvv/ 7
(vv7 8
avv8 9
=>vv: <
avv= >
.vv> ?
AppointmentIdvv? L
==vvM O
idvvP R
)vvR S
;vvS T
}ww 	
publicyy 
asyncyy 
Taskyy 
SaveChangesAsyncyy *
(yy* +
)yy+ ,
{zz 	
await{{ 
_context{{ 
.{{ 
SaveChangesAsync{{ +
({{+ ,
){{, -
;{{- .
}|| 	
}}} 
}~~ ˜
FC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. 

AddOpenApi 
( 
) 
; 
builder		 
.		 
Services		 
.		 
AddDbContext		 
<		 
HealthAxisDbContext		 1
>		1 2
(		2 3
options		3 :
=>		; =
options

 
.

 
UseSqlServer

 
(

 
builder 
. 
Configuration 
. 
GetConnectionString 1
(1 2
$str2 E
)E F
)F G
)G H
;H I
var 
app 
= 	
builder
 
. 
Build 
( 
) 
; 
if 
( 
app 
. 
Environment 
. 
IsDevelopment !
(! "
)" #
)# $
{ 
app 
. 

MapOpenApi 
( 
) 
; 
} 
app 
. 
UseHttpsRedirection 
( 
) 
; 
app 
. 
MapControllers 
( 
) 
; 
app 
. 
Run 
( 
) 	
;	 
∞
JC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\User.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
User 
{ 
[ 	
Key	 
] 
public		 
int		 
UserId		 
{		 
get		 
;		  
set		! $
;		$ %
}		& '
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
public 
string 
PasswordHash "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
[ 	
Required	 
] 
public 
UserRole 
Role 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
? 
ReferenceId 
{  !
get" %
;% &
set' *
;* +
}, -
public 
DateTime 
CreatedDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
DateTime4 <
.< =
UtcNow= C
;C D
} 
} ˆ!
MC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\Patient.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
Patient 
{ 
[ 	
Key	 
] 
public		 
int		 
	PatientId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) T
)T U
]U V
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 6
)6 7
]7 8
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
Phone	 
( 
ErrorMessage 
= 
$str <
)< =
]= >
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 5
)5 6
]6 7
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% =
)= >
]> ?
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
public   
InsuranceStatus   
InsuranceStatus   .
{  / 0
get  1 4
;  4 5
set  6 9
;  9 :
}  ; <
["" 	
StringLength""	 
("" 
$num"" 
)"" 
]"" 
public## 
string## 
?## 
InsuranceNumber## &
{##' (
get##) ,
;##, -
set##. 1
;##1 2
}##3 4
public%% 
bool%% 
IsActive%% 
{%% 
get%% "
;%%" #
set%%$ '
;%%' (
}%%) *
=%%+ ,
true%%- 1
;%%1 2
public(( 
ICollection(( 
<(( 
Appointment(( &
>((& '
Appointments((( 4
{((5 6
get((7 :
;((: ;
set((< ?
;((? @
}((A B
=((C D
new((E H
List((I M
<((M N
Appointment((N Y
>((Y Z
(((Z [
)(([ \
;((\ ]
public** 
ICollection** 
<** 
HealthRecord** '
>**' (
HealthRecords**) 6
{**7 8
get**9 <
;**< =
set**> A
;**A B
}**C D
=**E F
new**G J
List**K O
<**O P
HealthRecord**P \
>**\ ]
(**] ^
)**^ _
;**_ `
}++ 
},,  
RC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\HealthRecord.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
HealthRecord 
{ 
[ 	
Key	 
] 
public 
int 
HealthRecordId !
{" #
get$ '
;' (
set) ,
;, -
}. /
[

 	
Required

	 
]

 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
Appointment 
Appointment &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
null7 ;
!; <
;< =
[ 	
Required	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
Patient 
Patient 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
Doctor 
Doctor 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 9
)9 :
]: ;
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
	Diagnosis 
{  !
get" %
;% &
set' *
;* +
}, -
=. /
string0 6
.6 7
Empty7 <
;< =
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Prescription "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
[!! 	
StringLength!!	 
(!! 
$num!! 
)!! 
]!! 
public"" 
string"" 
?"" 
Notes"" 
{"" 
get"" "
;""" #
set""$ '
;""' (
}"") *
public$$ 
DateTime$$ 
	CreatedOn$$ !
{$$" #
get$$$ '
;$$' (
set$$) ,
;$$, -
}$$. /
=$$0 1
DateTime$$2 :
.$$: ;
UtcNow$$; A
;$$A B
}%% 
}&& Ù
LC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\Doctor.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
Doctor 
{ 
[ 	
Key	 
] 
public		 
int		 
DoctorId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ;
); <
]< =
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* U
)U V
]V W
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! >
)> ?
]? @
public  
DoctorSpecialisation #
Specialisation$ 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
[ 	
Required	 
( 
ErrorMessage 
=  
$str! C
)C D
]D E
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage "
=# $
$str% Q
)Q R
]R S
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
( 
ErrorMessage 
=  
$str! @
)@ A
]A B
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage )
=* +
$str, Y
)Y Z
]Z [
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
true- 1
;1 2
public 
ICollection 
< 
Appointment &
>& '
Appointments( 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
=C D
newE H
ListI M
<M N
AppointmentN Y
>Y Z
(Z [
)[ \
;\ ]
public 
ICollection 
< 
HealthRecord '
>' (
HealthRecords) 6
{7 8
get9 <
;< =
set> A
;A B
}C D
=E F
newG J
ListK O
<O P
HealthRecordP \
>\ ]
(] ^
)^ _
;_ `
}   
}!! …
QC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\Appointment.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
Appointment 
{ 
[		 	
Key			 
]		 
public

 
int

 
AppointmentId

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

- .
[ 	
Required	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
Patient 
Patient 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
Doctor 
Doctor 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
[ 	
Required	 
] 
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
public 
AppointmentTimeSlot "
TimeSlot# +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
AppointmentStatus  
Status! '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
= 
AppointmentStatus 
.  
Pending  '
;' (
[ 	
StringLength	 
( 
$num 
) 
] 
public   
string   
?   
CancellationReason   )
{  * +
get  , /
;  / 0
set  1 4
;  4 5
}  6 7
public"" 
HealthRecord"" 
?"" 
HealthRecord"" )
{""* +
get"", /
;""/ 0
set""1 4
;""4 5
}""6 7
}## 
}$$ Å∑
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260615042816_InitialCreate.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{		 
public 

partial 
class 
InitialCreate &
:' (
	Migration) 2
{ 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str 
,  
columns 
: 
table 
=> !
new" %
{ 
DoctorId 
= 
table $
.$ %
Column% +
<+ ,
int, /
>/ 0
(0 1
type1 5
:5 6
$str7 <
,< =
nullable> F
:F G
falseH M
)M N
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
FullName 
= 
table $
.$ %
Column% +
<+ ,
string, 2
>2 3
(3 4
type4 8
:8 9
$str: I
,I J
	maxLengthK T
:T U
$numV Y
,Y Z
nullable[ c
:c d
falsee j
)j k
,k l
Specialisation "
=# $
table% *
.* +
Column+ 1
<1 2
int2 5
>5 6
(6 7
type7 ;
:; <
$str= B
,B C
nullableD L
:L M
falseN S
)S T
,T U
YearsOfExperience %
=& '
table( -
.- .
Column. 4
<4 5
int5 8
>8 9
(9 :
type: >
:> ?
$str@ E
,E F
nullableG O
:O P
falseQ V
)V W
,W X
ConsultationFee #
=$ %
table& +
.+ ,
Column, 2
<2 3
decimal3 :
>: ;
(; <
type< @
:@ A
$strB Q
,Q R
	precisionS \
:\ ]
$num^ `
,` a
scaleb g
:g h
$numi j
,j k
nullablel t
:t u
falsev {
){ |
,| }
IsActive 
= 
table $
.$ %
Column% +
<+ ,
bool, 0
>0 1
(1 2
type2 6
:6 7
$str8 =
,= >
nullable? G
:G H
falseI N
)N O
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 1
,1 2
x3 4
=>5 7
x8 9
.9 :
DoctorId: B
)B C
;C D
} 
) 
; 
migrationBuilder!! 
.!! 
CreateTable!! (
(!!( )
name"" 
:"" 
$str""  
,""  !
columns## 
:## 
table## 
=>## !
new##" %
{$$ 
	PatientId%% 
=%% 
table%%  %
.%%% &
Column%%& ,
<%%, -
int%%- 0
>%%0 1
(%%1 2
type%%2 6
:%%6 7
$str%%8 =
,%%= >
nullable%%? G
:%%G H
false%%I N
)%%N O
.&& 

Annotation&& #
(&&# $
$str&&$ 8
,&&8 9
$str&&: @
)&&@ A
,&&A B
FullName'' 
='' 
table'' $
.''$ %
Column''% +
<''+ ,
string'', 2
>''2 3
(''3 4
type''4 8
:''8 9
$str'': H
,''H I
	maxLength''J S
:''S T
$num''U W
,''W X
nullable''Y a
:''a b
false''c h
)''h i
,''i j
DateOfBirth(( 
=((  !
table((" '
.((' (
Column((( .
<((. /
DateOnly((/ 7
>((7 8
(((8 9
type((9 =
:((= >
$str((? E
,((E F
nullable((G O
:((O P
false((Q V
)((V W
,((W X
Gender)) 
=)) 
table)) "
.))" #
Column))# )
<))) *
int))* -
>))- .
()). /
type))/ 3
:))3 4
$str))5 :
,)): ;
nullable))< D
:))D E
false))F K
)))K L
,))L M
PhoneNumber** 
=**  !
table**" '
.**' (
Column**( .
<**. /
string**/ 5
>**5 6
(**6 7
type**7 ;
:**; <
$str**= K
,**K L
	maxLength**M V
:**V W
$num**X Z
,**Z [
nullable**\ d
:**d e
false**f k
)**k l
,**l m
Email++ 
=++ 
table++ !
.++! "
Column++" (
<++( )
string++) /
>++/ 0
(++0 1
type++1 5
:++5 6
$str++7 F
,++F G
	maxLength++H Q
:++Q R
$num++S V
,++V W
nullable++X `
:++` a
false++b g
)++g h
,++h i
InsuranceStatus,, #
=,,$ %
table,,& +
.,,+ ,
Column,,, 2
<,,2 3
int,,3 6
>,,6 7
(,,7 8
type,,8 <
:,,< =
$str,,> C
,,,C D
nullable,,E M
:,,M N
false,,O T
),,T U
,,,U V
InsuranceNumber-- #
=--$ %
table--& +
.--+ ,
Column--, 2
<--2 3
string--3 9
>--9 :
(--: ;
type--; ?
:--? @
$str--A O
,--O P
	maxLength--Q Z
:--Z [
$num--\ ^
,--^ _
nullable--` h
:--h i
true--j n
)--n o
,--o p
IsActive.. 
=.. 
table.. $
...$ %
Column..% +
<..+ ,
bool.., 0
>..0 1
(..1 2
type..2 6
:..6 7
$str..8 =
,..= >
nullable..? G
:..G H
false..I N
)..N O
}// 
,// 
constraints00 
:00 
table00 "
=>00# %
{11 
table22 
.22 

PrimaryKey22 $
(22$ %
$str22% 2
,222 3
x224 5
=>226 8
x229 :
.22: ;
	PatientId22; D
)22D E
;22E F
}33 
)33 
;33 
migrationBuilder55 
.55 
CreateTable55 (
(55( )
name66 
:66 
$str66 $
,66$ %
columns77 
:77 
table77 
=>77 !
new77" %
{88 
AppointmentId99 !
=99" #
table99$ )
.99) *
Column99* 0
<990 1
int991 4
>994 5
(995 6
type996 :
:99: ;
$str99< A
,99A B
nullable99C K
:99K L
false99M R
)99R S
.:: 

Annotation:: #
(::# $
$str::$ 8
,::8 9
$str::: @
)::@ A
,::A B
	PatientId;; 
=;; 
table;;  %
.;;% &
Column;;& ,
<;;, -
int;;- 0
>;;0 1
(;;1 2
type;;2 6
:;;6 7
$str;;8 =
,;;= >
nullable;;? G
:;;G H
false;;I N
);;N O
,;;O P
DoctorId<< 
=<< 
table<< $
.<<$ %
Column<<% +
<<<+ ,
int<<, /
><</ 0
(<<0 1
type<<1 5
:<<5 6
$str<<7 <
,<<< =
nullable<<> F
:<<F G
false<<H M
)<<M N
,<<N O
ScheduledDate== !
===" #
table==$ )
.==) *
Column==* 0
<==0 1
DateOnly==1 9
>==9 :
(==: ;
type==; ?
:==? @
$str==A G
,==G H
nullable==I Q
:==Q R
false==S X
)==X Y
,==Y Z
TimeSlot>> 
=>> 
table>> $
.>>$ %
Column>>% +
<>>+ ,
int>>, /
>>>/ 0
(>>0 1
type>>1 5
:>>5 6
$str>>7 <
,>>< =
nullable>>> F
:>>F G
false>>H M
)>>M N
,>>N O
Status?? 
=?? 
table?? "
.??" #
Column??# )
<??) *
int??* -
>??- .
(??. /
type??/ 3
:??3 4
$str??5 :
,??: ;
nullable??< D
:??D E
false??F K
)??K L
,??L M
CancellationReason@@ &
=@@' (
table@@) .
.@@. /
Column@@/ 5
<@@5 6
string@@6 <
>@@< =
(@@= >
type@@> B
:@@B C
$str@@D S
,@@S T
	maxLength@@U ^
:@@^ _
$num@@` c
,@@c d
nullable@@e m
:@@m n
true@@o s
)@@s t
}AA 
,AA 
constraintsBB 
:BB 
tableBB "
=>BB# %
{CC 
tableDD 
.DD 

PrimaryKeyDD $
(DD$ %
$strDD% 6
,DD6 7
xDD8 9
=>DD: <
xDD= >
.DD> ?
AppointmentIdDD? L
)DDL M
;DDM N
tableEE 
.EE 

ForeignKeyEE $
(EE$ %
nameFF 
:FF 
$strFF @
,FF@ A
columnGG 
:GG 
xGG  !
=>GG" $
xGG% &
.GG& '
DoctorIdGG' /
,GG/ 0
principalTableHH &
:HH& '
$strHH( 1
,HH1 2
principalColumnII '
:II' (
$strII) 3
,II3 4
onDeleteJJ  
:JJ  !
ReferentialActionJJ" 3
.JJ3 4
RestrictJJ4 <
)JJ< =
;JJ= >
tableKK 
.KK 

ForeignKeyKK $
(KK$ %
nameLL 
:LL 
$strLL B
,LLB C
columnMM 
:MM 
xMM  !
=>MM" $
xMM% &
.MM& '
	PatientIdMM' 0
,MM0 1
principalTableNN &
:NN& '
$strNN( 2
,NN2 3
principalColumnOO '
:OO' (
$strOO) 4
,OO4 5
onDeletePP  
:PP  !
ReferentialActionPP" 3
.PP3 4
RestrictPP4 <
)PP< =
;PP= >
}QQ 
)QQ 
;QQ 
migrationBuilderSS 
.SS 
CreateTableSS (
(SS( )
nameTT 
:TT 
$strTT %
,TT% &
columnsUU 
:UU 
tableUU 
=>UU !
newUU" %
{VV 
HealthRecordIdWW "
=WW# $
tableWW% *
.WW* +
ColumnWW+ 1
<WW1 2
intWW2 5
>WW5 6
(WW6 7
typeWW7 ;
:WW; <
$strWW= B
,WWB C
nullableWWD L
:WWL M
falseWWN S
)WWS T
.XX 

AnnotationXX #
(XX# $
$strXX$ 8
,XX8 9
$strXX: @
)XX@ A
,XXA B
AppointmentIdYY !
=YY" #
tableYY$ )
.YY) *
ColumnYY* 0
<YY0 1
intYY1 4
>YY4 5
(YY5 6
typeYY6 :
:YY: ;
$strYY< A
,YYA B
nullableYYC K
:YYK L
falseYYM R
)YYR S
,YYS T
	PatientIdZZ 
=ZZ 
tableZZ  %
.ZZ% &
ColumnZZ& ,
<ZZ, -
intZZ- 0
>ZZ0 1
(ZZ1 2
typeZZ2 6
:ZZ6 7
$strZZ8 =
,ZZ= >
nullableZZ? G
:ZZG H
falseZZI N
)ZZN O
,ZZO P
DoctorId[[ 
=[[ 
table[[ $
.[[$ %
Column[[% +
<[[+ ,
int[[, /
>[[/ 0
([[0 1
type[[1 5
:[[5 6
$str[[7 <
,[[< =
nullable[[> F
:[[F G
false[[H M
)[[M N
,[[N O
	Diagnosis\\ 
=\\ 
table\\  %
.\\% &
Column\\& ,
<\\, -
string\\- 3
>\\3 4
(\\4 5
type\\5 9
:\\9 :
$str\\; K
,\\K L
	maxLength\\M V
:\\V W
$num\\X \
,\\\ ]
nullable\\^ f
:\\f g
false\\h m
)\\m n
,\\n o
Prescription]]  
=]]! "
table]]# (
.]]( )
Column]]) /
<]]/ 0
string]]0 6
>]]6 7
(]]7 8
type]]8 <
:]]< =
$str]]> N
,]]N O
	maxLength]]P Y
:]]Y Z
$num]][ _
,]]_ `
nullable]]a i
:]]i j
false]]k p
)]]p q
,]]q r
Notes^^ 
=^^ 
table^^ !
.^^! "
Column^^" (
<^^( )
string^^) /
>^^/ 0
(^^0 1
type^^1 5
:^^5 6
$str^^7 G
,^^G H
	maxLength^^I R
:^^R S
$num^^T X
,^^X Y
nullable^^Z b
:^^b c
true^^d h
)^^h i
,^^i j
	CreatedOn__ 
=__ 
table__  %
.__% &
Column__& ,
<__, -
DateTime__- 5
>__5 6
(__6 7
type__7 ;
:__; <
$str__= H
,__H I
nullable__J R
:__R S
false__T Y
)__Y Z
}`` 
,`` 
constraintsaa 
:aa 
tableaa "
=>aa# %
{bb 
tablecc 
.cc 

PrimaryKeycc $
(cc$ %
$strcc% 7
,cc7 8
xcc9 :
=>cc; =
xcc> ?
.cc? @
HealthRecordIdcc@ N
)ccN O
;ccO P
tabledd 
.dd 

ForeignKeydd $
(dd$ %
nameee 
:ee 
$stree K
,eeK L
columnff 
:ff 
xff  !
=>ff" $
xff% &
.ff& '
AppointmentIdff' 4
,ff4 5
principalTablegg &
:gg& '
$strgg( 6
,gg6 7
principalColumnhh '
:hh' (
$strhh) 8
,hh8 9
onDeleteii  
:ii  !
ReferentialActionii" 3
.ii3 4
Restrictii4 <
)ii< =
;ii= >
tablejj 
.jj 

ForeignKeyjj $
(jj$ %
namekk 
:kk 
$strkk A
,kkA B
columnll 
:ll 
xll  !
=>ll" $
xll% &
.ll& '
DoctorIdll' /
,ll/ 0
principalTablemm &
:mm& '
$strmm( 1
,mm1 2
principalColumnnn '
:nn' (
$strnn) 3
,nn3 4
onDeleteoo  
:oo  !
ReferentialActionoo" 3
.oo3 4
Restrictoo4 <
)oo< =
;oo= >
tablepp 
.pp 

ForeignKeypp $
(pp$ %
nameqq 
:qq 
$strqq C
,qqC D
columnrr 
:rr 
xrr  !
=>rr" $
xrr% &
.rr& '
	PatientIdrr' 0
,rr0 1
principalTabless &
:ss& '
$strss( 2
,ss2 3
principalColumntt '
:tt' (
$strtt) 4
,tt4 5
onDeleteuu  
:uu  !
ReferentialActionuu" 3
.uu3 4
Restrictuu4 <
)uu< =
;uu= >
}vv 
)vv 
;vv 
migrationBuilderxx 
.xx 

InsertDataxx '
(xx' (
tableyy 
:yy 
$stryy  
,yy  !
columnszz 
:zz 
newzz 
[zz 
]zz 
{zz  
$strzz! +
,zz+ ,
$strzz- >
,zz> ?
$strzz@ J
,zzJ K
$strzzL V
,zzV W
$strzzX h
,zzh i
$strzzj }
}zz~ 
,	zz Ä
values{{ 
:{{ 
new{{ 
object{{ "
[{{" #
,{{# $
]{{$ %
{|| 
{}} 
$num}} 
,}} 
$num}}  
,}}  !
$str}}" -
,}}- .
true}}/ 3
,}}3 4
$num}}5 6
,}}6 7
$num}}8 9
}}}: ;
,}}; <
{~~ 
$num~~ 
,~~ 
$num~~ !
,~~! "
$str~~# 0
,~~0 1
true~~2 6
,~~6 7
$num~~8 9
,~~9 :
$num~~; =
}~~> ?
} 
) 
; 
migrationBuilder
ÅÅ 
.
ÅÅ 

InsertData
ÅÅ '
(
ÅÅ' (
table
ÇÇ 
:
ÇÇ 
$str
ÇÇ !
,
ÇÇ! "
columns
ÉÉ 
:
ÉÉ 
new
ÉÉ 
[
ÉÉ 
]
ÉÉ 
{
ÉÉ  
$str
ÉÉ! ,
,
ÉÉ, -
$str
ÉÉ. ;
,
ÉÉ; <
$str
ÉÉ= D
,
ÉÉD E
$str
ÉÉF P
,
ÉÉP Q
$str
ÉÉR Z
,
ÉÉZ [
$str
ÉÉ\ m
,
ÉÉm n
$strÉÉo Ä
,ÉÉÄ Å
$strÉÉÇ å
,ÉÉå ç
$strÉÉé õ
}ÉÉú ù
,ÉÉù û
values
ÑÑ 
:
ÑÑ 
new
ÑÑ 
object
ÑÑ "
[
ÑÑ" #
,
ÑÑ# $
]
ÑÑ$ %
{
ÖÖ 
{
ÜÜ 
$num
ÜÜ 
,
ÜÜ 
new
ÜÜ 
DateOnly
ÜÜ %
(
ÜÜ% &
$num
ÜÜ& *
,
ÜÜ* +
$num
ÜÜ, -
,
ÜÜ- .
$num
ÜÜ/ 1
)
ÜÜ1 2
,
ÜÜ2 3
$str
ÜÜ4 G
,
ÜÜG H
$str
ÜÜI W
,
ÜÜW X
$num
ÜÜY Z
,
ÜÜZ [
$str
ÜÜ\ e
,
ÜÜe f
$num
ÜÜg h
,
ÜÜh i
true
ÜÜj n
,
ÜÜn o
$str
ÜÜp |
}
ÜÜ} ~
,
ÜÜ~ 
{
áá 
$num
áá 
,
áá 
new
áá 
DateOnly
áá %
(
áá% &
$num
áá& *
,
áá* +
$num
áá, .
,
áá. /
$num
áá0 1
)
áá1 2
,
áá2 3
$str
áá4 F
,
ááF G
$str
ááH U
,
ááU V
$num
ááW X
,
ááX Y
$str
ááZ c
,
áác d
$num
ááe f
,
ááf g
true
ááh l
,
áál m
$str
áán z
}
áá{ |
}
àà 
)
àà 
;
àà 
migrationBuilder
ää 
.
ää 

InsertData
ää '
(
ää' (
table
ãã 
:
ãã 
$str
ãã %
,
ãã% &
columns
åå 
:
åå 
new
åå 
[
åå 
]
åå 
{
åå  
$str
åå! 0
,
åå0 1
$str
åå2 F
,
ååF G
$str
ååH R
,
ååR S
$str
ååT _
,
åå_ `
$str
ååa p
,
ååp q
$str
åår z
,
ååz {
$stråå| Ü
}ååá à
,ååà â
values
çç 
:
çç 
new
çç 
object
çç "
[
çç" #
]
çç# $
{
çç% &
$num
çç' (
,
çç( )
null
çç* .
,
çç. /
$num
çç0 1
,
çç1 2
$num
çç3 4
,
çç4 5
new
çç6 9
DateOnly
çç: B
(
ççB C
$num
ççC G
,
ççG H
$num
ççI J
,
ççJ K
$num
ççL N
)
ççN O
,
ççO P
$num
ççQ R
,
ççR S
$num
ççT U
}
ççV W
)
ççW X
;
ççX Y
migrationBuilder
èè 
.
èè 
CreateIndex
èè (
(
èè( )
name
êê 
:
êê 
$str
êê G
,
êêG H
table
ëë 
:
ëë 
$str
ëë %
,
ëë% &
columns
íí 
:
íí 
new
íí 
[
íí 
]
íí 
{
íí  
$str
íí! +
,
íí+ ,
$str
íí- <
,
íí< =
$str
íí> H
}
ííI J
,
ííJ K
unique
ìì 
:
ìì 
true
ìì 
)
ìì 
;
ìì 
migrationBuilder
ïï 
.
ïï 
CreateIndex
ïï (
(
ïï( )
name
ññ 
:
ññ 
$str
ññ 1
,
ññ1 2
table
óó 
:
óó 
$str
óó %
,
óó% &
column
òò 
:
òò 
$str
òò #
)
òò# $
;
òò$ %
migrationBuilder
öö 
.
öö 
CreateIndex
öö (
(
öö( )
name
õõ 
:
õõ 
$str
õõ 6
,
õõ6 7
table
úú 
:
úú 
$str
úú &
,
úú& '
column
ùù 
:
ùù 
$str
ùù '
,
ùù' (
unique
ûû 
:
ûû 
true
ûû 
)
ûû 
;
ûû 
migrationBuilder
†† 
.
†† 
CreateIndex
†† (
(
††( )
name
°° 
:
°° 
$str
°° 1
,
°°1 2
table
¢¢ 
:
¢¢ 
$str
¢¢ &
,
¢¢& '
column
££ 
:
££ 
$str
££ "
)
££" #
;
££# $
migrationBuilder
•• 
.
•• 
CreateIndex
•• (
(
••( )
name
¶¶ 
:
¶¶ 
$str
¶¶ 2
,
¶¶2 3
table
ßß 
:
ßß 
$str
ßß &
,
ßß& '
column
®® 
:
®® 
$str
®® #
)
®®# $
;
®®$ %
}
©© 	
	protected
¨¨ 
override
¨¨ 
void
¨¨ 
Down
¨¨  $
(
¨¨$ %
MigrationBuilder
¨¨% 5
migrationBuilder
¨¨6 F
)
¨¨F G
{
≠≠ 	
migrationBuilder
ÆÆ 
.
ÆÆ 
	DropTable
ÆÆ &
(
ÆÆ& '
name
ØØ 
:
ØØ 
$str
ØØ %
)
ØØ% &
;
ØØ& '
migrationBuilder
±± 
.
±± 
	DropTable
±± &
(
±±& '
name
≤≤ 
:
≤≤ 
$str
≤≤ $
)
≤≤$ %
;
≤≤% &
migrationBuilder
¥¥ 
.
¥¥ 
	DropTable
¥¥ &
(
¥¥& '
name
µµ 
:
µµ 
$str
µµ 
)
µµ  
;
µµ  !
migrationBuilder
∑∑ 
.
∑∑ 
	DropTable
∑∑ &
(
∑∑& '
name
∏∏ 
:
∏∏ 
$str
∏∏  
)
∏∏  !
;
∏∏! "
}
ππ 	
}
∫∫ 
}ªª Ä
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Middleware\RequestLoggingMiddleware.cs
	namespace 	

HealthAxis
 
. 
API 
. 

Middleware #
{ 
public 

class $
RequestLoggingMiddleware )
{ 
private 
readonly 
RequestDelegate (
_next) .
;. /
private 
readonly 
ILogger  
<  !$
RequestLoggingMiddleware! 9
>9 :
_logger; B
;B C
public $
RequestLoggingMiddleware '
(' (
RequestDelegate		 
next		  
,		  !
ILogger

 
<

 $
RequestLoggingMiddleware

 ,
>

, -
logger

. 4
)

4 5
{ 	
_next 
= 
next 
; 
_logger 
= 
logger 
; 
} 	
public 
async 
Task 
InvokeAsync %
(% &
HttpContext& 1
context2 9
)9 :
{ 	
_logger 
. 
LogInformation "
(" #
$str 3
,3 4
context 
. 
Request 
.  
Method  &
,& '
context 
. 
Request 
.  
Path  $
)$ %
;% &
await 
_next 
( 
context 
)  
;  !
_logger 
. 
LogInformation "
(" #
$str 1
,1 2
context 
. 
Response  
.  !

StatusCode! +
)+ ,
;, -
} 	
} 
} ’
MC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\UserRole.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum 
UserRole 
{ 
Admin 
= 
$num 
, 
Doctor 
= 
$num 
, 
Patient 
= 
$num 
} 
}		 ß
MC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\TimeSlot.cs
public 
enum 
AppointmentTimeSlot 
{ 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
TenAM 	
=
 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
TenThirtyAM 
= 
$num 
, 
[

 
Display

 
(

 
Name

 
=

 
$str

 )
)

) *
]

* +
ElevenAM 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
ElevenThirtyAM 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
TwelvePM 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
TwelveThirtyPM 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
OnePM 	
=
 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
OneThirtyPM 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
TwoPM 	
=
 
$num 
, 
[ 
Display 
( 
Name 
= 
$str )
)) *
]* +
TwoThirtyPM   
=   
$num   
,   
["" 
Display"" 
("" 
Name"" 
="" 
$str"" )
)"") *
]""* +
ThreePM## 
=## 
$num## 
,## 
[%% 
Display%% 
(%% 
Name%% 
=%% 
$str%% )
)%%) *
]%%* +
ThreeThirtyPM&& 
=&& 
$num&& 
}'' Ä
TC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\InsuranceStatus.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum 
InsuranceStatus 
{ 

NotInsured 
= 
$num 
, 
PendingVerification 
= 
$num 
,  
Active 
= 
$num 
, 
Expired 
= 
$num 
, 
	Suspended		 
=		 
$num		 
}

 
} ©
KC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\Gender.cs
public 
enum 
Gender 
{ 
Male 
=	 

$num 
, 
Female 

= 
$num 
, 
	NonBinary 
= 
$num 
, 
PreferNotToSay 
= 
$num 
} ≈
YC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\DoctorSpecialization.cs
public 
enum  
DoctorSpecialisation  
{ 
[ 
Display 
( 
Name 
= 
$str *
)* +
]+ ,
GeneralPractitioner 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str "
)" #
]# $
Cardiologist		 
=		 
$num		 
,		 
[ 
Display 
( 
Name 
= 
$str #
)# $
]$ %
Dermatologist 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str !
)! "
]" #
Neurologist 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str "
)" #
]# $
Pediatrician 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str "
)" #
]# $
Psychiatrist 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str (
)( )
]) *
OrthopedicSurgeon 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str "
)" #
]# $
Gynecologist 
= 
$num 
, 
[ 
Display 
( 
Name 
= 
$str  
)  !
]! "

Oncologist 
= 
$num 
, 
[   
Display   
(   
Name   
=   
$str   %
)  % &
]  & '
Endocrinologist!! 
=!! 
$num!! 
}"" µ
VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\AppointmentStatus.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum 
AppointmentStatus !
{ 
Pending 
= 
$num 
, 
	Confirmed 
= 
$num 
, 
	Cancelled 
= 
$num 
, 
	Completed 
= 
$num 
}		 
}

 õ
\C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\UpdatePatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class 
UpdatePatientDto !
{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public		 
string		 
?		 
Email		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public

 
string

 
?

 
InsuranceId

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

/ 0
} 
} É
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\PatientSearchResultDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class "
PatientSearchResultDto '
{ 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
}		 √
VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\PatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class 

PatientDto 
{ 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
public		 
string		 
PhoneNumber		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
=		0 1
string		2 8
.		8 9
Empty		9 >
;		> ?
public

 
string

 
?

 
Email

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

) *
public 
string 
? 
InsuranceId "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} õ
\C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\CreatePatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class 
CreatePatientDto !
{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public		 
string		 
?		 
Email		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public

 
string

 
?

 
InsuranceId

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

/ 0
} 
} Í
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\HealthRecord\UpdateHealthRecordDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
HealthRecord  ,
{ 
public 

class !
UpdateHealthRecordDto &
{ 
public 
string 
? 
	Diagnosis  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
? 
Prescription #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
? 
Notes 
{ 
get "
;" #
set$ '
;' (
}) *
} 
}		 Œ
`C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\HealthRecord\HealthRecordDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
HealthRecord  ,
{ 
public 

class 
HealthRecordDto  
{ 
public 
int 
HealthRecordId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public		 
string		 
?		 
	Diagnosis		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
public

 
string

 
?

 
Prescription

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

0 1
public 
string 
? 
Notes 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ª

fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\HealthRecord\CreateHealthRecordDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
HealthRecord  ,
{ 
public 

class !
CreateHealthRecordDto &
{ 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
? 
	Diagnosis  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
string		 
?		 
Prescription		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		0 1
public

 
string

 
?

 
Notes

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

) *
} 
} §
ZC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Doctor\UpdateDoctorDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Doctor  &
{ 
public 

class 
UpdateDoctorDto  
{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
int 
Specialisation !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
}		 
}

 À

TC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Doctor\DoctorDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Doctor  &
{ 
public 

class 
	DoctorDto 
{ 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
int 
Specialisation !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public		 
decimal		 
ConsultationFee		 &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		3 4
public

 
bool

 
IsActive

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

) *
} 
} §
ZC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Doctor\CreateDoctorDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Doctor  &
{ 
public 

class 
CreateDoctorDto  
{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
int 
Specialisation !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
}		 
} ø

PC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\UserDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
UserDto 
{ 
public 
int 
UserId 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
int 
Role 
{ 
get 
; 
set "
;" #
}$ %
public 
int 
? 
	PatientId 
{ 
get  #
;# $
set% (
;( )
}* +
public		 
int		 
?		 
DoctorId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public

 
bool

 
IsActive

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

) *
} 
} ö
QC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\LoginDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
LoginDto 
{ 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
} 
} ä

VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\CreateUserDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
CreateUserDto 
{ 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
int 
Role 
{ 
get 
; 
set "
;" #
}$ %
public 
int 
? 
	PatientId 
{ 
get  #
;# $
set% (
;( )
}* +
public		 
int		 
?		 
DoctorId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
}

 
} ∏
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\UpdateAppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class  
UpdateAppointmentDto %
{ 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
} 
}

 Ô

lC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\PatientAppointmentHistoryDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class (
PatientAppointmentHistoryDto -
{ 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public		 
string		 

DoctorName		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
=		/ 0
string		1 7
.		7 8
Empty		8 =
;		= >
public

 
int

 
Status

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
} 
} „

eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\DoctorScheduleItemDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class !
DoctorScheduleItemDto &
{ 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public		 
string		 
PatientName		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
=		0 1
string		2 8
.		8 9
Empty		9 >
;		> ?
public

 
int

 
Status

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
} 
} “
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\CreateAppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class  
CreateAppointmentDto %
{ 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
}		 
} ›
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\CancelAppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class  
CancelAppointmentDto %
{ 
public 
string 
CancellationReason (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
=7 8
string9 ?
.? @
Empty@ E
;E F
} 
} ∞
^C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\AppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class 
AppointmentDto 
{ 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public		 
int		 
TimeSlot		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public

 
int

 
Status

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} £
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\AppointmentDetailsDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class !
AppointmentDetailsDto &
{ 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PatientName !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public		 
string		 

DoctorName		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
=		/ 0
string		1 7
.		7 8
Empty		8 =
;		= >
public

 
DateTime

 
ScheduledDate

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
Status 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} ô]
PC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Data\AppDbContext.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Data 
{ 
public 

class 
HealthAxisDbContext $
:% &
	DbContext' 0
{ 
public		 
HealthAxisDbContext		 "
(		" #
DbContextOptions		# 3
<		3 4
HealthAxisDbContext		4 G
>		G H
options		I P
)		P Q
:

 
base

 
(

 
options

 
)

 
{ 	
} 	
public 
DbSet 
< 
Patient 
> 
Patients &
=>' )
Set* -
<- .
Patient. 5
>5 6
(6 7
)7 8
;8 9
public 
DbSet 
< 
Doctor 
> 
Doctors $
=>% '
Set( +
<+ ,
Doctor, 2
>2 3
(3 4
)4 5
;5 6
public 
DbSet 
< 
Appointment  
>  !
Appointments" .
=>/ 1
Set2 5
<5 6
Appointment6 A
>A B
(B C
)C D
;D E
public 
DbSet 
< 
HealthRecord !
>! "
HealthRecords# 0
=>1 3
Set4 7
<7 8
HealthRecord8 D
>D E
(E F
)F G
;G H
public 
DbSet 
< 
User 
> 
Users  
=>! #
Set$ '
<' (
User( ,
>, -
(- .
). /
;/ 0
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
modelBuilder= I
)I J
{ 	
base 
. 
OnModelCreating  
(  !
modelBuilder! -
)- .
;. /
modelBuilder 
. 
Entity 
<  
Appointment  +
>+ ,
(, -
)- .
. 
HasOne 
( 
a 
=> 
a 
. 
Patient &
)& '
. 
WithMany 
( 
p 
=> 
p  
.  !
Appointments! -
)- .
. 
HasForeignKey 
( 
a  
=>! #
a$ %
.% &
	PatientId& /
)/ 0
. 
OnDelete 
( 
DeleteBehavior (
.( )
Restrict) 1
)1 2
;2 3
modelBuilder!! 
.!! 
Entity!! 
<!!  
Appointment!!  +
>!!+ ,
(!!, -
)!!- .
."" 
HasOne"" 
("" 
a"" 
=>"" 
a"" 
."" 
Doctor"" %
)""% &
.## 
WithMany## 
(## 
d## 
=>## 
d##  
.##  !
Appointments##! -
)##- .
.$$ 
HasForeignKey$$ 
($$ 
a$$  
=>$$! #
a$$$ %
.$$% &
DoctorId$$& .
)$$. /
.%% 
OnDelete%% 
(%% 
DeleteBehavior%% (
.%%( )
Restrict%%) 1
)%%1 2
;%%2 3
modelBuilder(( 
.(( 
Entity(( 
<((  
Appointment((  +
>((+ ,
(((, -
)((- .
.)) 
HasIndex)) 
()) 
a)) 
=>)) 
new)) "
{** 
a++ 
.++ 
DoctorId++ 
,++ 
a,, 
.,, 
ScheduledDate,, #
,,,# $
a-- 
.-- 
TimeSlot-- 
}.. 
).. 
.// 
IsUnique// 
(// 
)// 
;// 
modelBuilder44 
.44 
Entity44 
<44  
HealthRecord44  ,
>44, -
(44- .
)44. /
.55 
HasOne55 
(55 
hr55 
=>55 
hr55  
.55  !
Appointment55! ,
)55, -
.66 
WithOne66 
(66 
a66 
=>66 
a66 
.66  
HealthRecord66  ,
)66, -
.77 
HasForeignKey77 
<77 
HealthRecord77 +
>77+ ,
(77, -
hr77- /
=>770 2
hr773 5
.775 6
AppointmentId776 C
)77C D
.88 
OnDelete88 
(88 
DeleteBehavior88 (
.88( )
Restrict88) 1
)881 2
;882 3
modelBuilder:: 
.:: 
Entity:: 
<::  
HealthRecord::  ,
>::, -
(::- .
)::. /
.;; 
HasOne;; 
(;; 
hr;; 
=>;; 
hr;;  
.;;  !
Patient;;! (
);;( )
.<< 
WithMany<< 
(<< 
p<< 
=><< 
p<<  
.<<  !
HealthRecords<<! .
)<<. /
.== 
HasForeignKey== 
(== 
hr== !
=>==" $
hr==% '
.==' (
	PatientId==( 1
)==1 2
.>> 
OnDelete>> 
(>> 
DeleteBehavior>> (
.>>( )
Restrict>>) 1
)>>1 2
;>>2 3
modelBuilder@@ 
.@@ 
Entity@@ 
<@@  
HealthRecord@@  ,
>@@, -
(@@- .
)@@. /
.AA 
HasOneAA 
(AA 
hrAA 
=>AA 
hrAA  
.AA  !
DoctorAA! '
)AA' (
.BB 
WithManyBB 
(BB 
dBB 
=>BB 
dBB  
.BB  !
HealthRecordsBB! .
)BB. /
.CC 
HasForeignKeyCC 
(CC 
hrCC !
=>CC" $
hrCC% '
.CC' (
DoctorIdCC( 0
)CC0 1
.DD 
OnDeleteDD 
(DD 
DeleteBehaviorDD (
.DD( )
RestrictDD) 1
)DD1 2
;DD2 3
modelBuilderGG 
.GG 
EntityGG 
<GG  
HealthRecordGG  ,
>GG, -
(GG- .
)GG. /
.HH 
HasIndexHH 
(HH 
hrHH 
=>HH 
hrHH  "
.HH" #
AppointmentIdHH# 0
)HH0 1
.II 
IsUniqueII 
(II 
)II 
;II 
modelBuilderMM 
.MM 
EntityMM 
<MM  
DoctorMM  &
>MM& '
(MM' (
)MM( )
.NN 
PropertyNN 
(NN 
dNN 
=>NN 
dNN  
.NN  !
ConsultationFeeNN! 0
)NN0 1
.OO 
HasPrecisionOO 
(OO 
$numOO  
,OO  !
$numOO" #
)OO# $
;OO$ %
modelBuilderSS 
.SS 
EntitySS 
<SS  
PatientSS  '
>SS' (
(SS( )
)SS) *
.SS* +
HasDataSS+ 2
(SS2 3
newTT 
PatientTT 
{UU 
	PatientIdVV 
=VV 
$numVV  !
,VV! "
FullNameWW 
=WW 
$strWW -
,WW- .
DateOfBirthXX 
=XX  !
newXX" %
DateOnlyXX& .
(XX. /
$numXX/ 3
,XX3 4
$numXX5 6
,XX6 7
$numXX8 :
)XX: ;
,XX; <
GenderYY 
=YY 
GenderYY #
.YY# $
MaleYY$ (
,YY( )
PhoneNumberZZ 
=ZZ  !
$strZZ" .
,ZZ. /
Email[[ 
=[[ 
$str[[ /
,[[/ 0
InsuranceStatus\\ #
=\\$ %
InsuranceStatus\\& 5
.\\5 6
Active\\6 <
,\\< =
InsuranceNumber]] #
=]]$ %
$str]]& /
,]]/ 0
IsActive^^ 
=^^ 
true^^ #
}__ 
,__ 
new`` 
Patient`` 
{aa 
	PatientIdbb 
=bb 
$numbb  !
,bb! "
FullNamecc 
=cc 
$strcc ,
,cc, -
DateOfBirthdd 
=dd  !
newdd" %
DateOnlydd& .
(dd. /
$numdd/ 3
,dd3 4
$numdd5 7
,dd7 8
$numdd9 :
)dd: ;
,dd; <
Genderee 
=ee 
Genderee #
.ee# $
Femaleee$ *
,ee* +
PhoneNumberff 
=ff  !
$strff" .
,ff. /
Emailgg 
=gg 
$strgg .
,gg. /
InsuranceStatushh #
=hh$ %
InsuranceStatushh& 5
.hh5 6
Activehh6 <
,hh< =
InsuranceNumberii #
=ii$ %
$strii& /
,ii/ 0
IsActivejj 
=jj 
truejj #
}kk 
)ll 
;ll 
modelBuilderpp 
.pp 
Entitypp 
<pp  
Doctorpp  &
>pp& '
(pp' (
)pp( )
.pp) *
HasDatapp* 1
(pp1 2
newqq 
Doctorqq 
{rr 
DoctorIdss 
=ss 
$numss  
,ss  !
FullNamett 
=tt 
$strtt *
,tt* +
Specialisationuu "
=uu# $ 
DoctorSpecialisationuu% 9
.uu9 :
GeneralPractitioneruu: M
,uuM N
YearsOfExperiencevv %
=vv& '
$numvv( )
,vv) *
ConsultationFeeww #
=ww$ %
$numww& -
,ww- .
IsActivexx 
=xx 
truexx #
}yy 
,yy 
newzz 
Doctorzz 
{{{ 
DoctorId|| 
=|| 
$num||  
,||  !
FullName}} 
=}} 
$str}} ,
,}}, -
Specialisation~~ "
=~~# $ 
DoctorSpecialisation~~% 9
.~~9 :
Cardiologist~~: F
,~~F G
YearsOfExperience %
=& '
$num( *
,* +
ConsultationFee
ÄÄ #
=
ÄÄ$ %
$num
ÄÄ& .
,
ÄÄ. /
IsActive
ÅÅ 
=
ÅÅ 
true
ÅÅ #
}
ÇÇ 
)
ÉÉ 
;
ÉÉ 
modelBuilder
ââ 
.
ââ 
Entity
ââ 
<
ââ  
Appointment
ââ  +
>
ââ+ ,
(
ââ, -
)
ââ- .
.
ââ. /
HasData
ââ/ 6
(
ââ6 7
new
ää 
Appointment
ää 
{
ãã 
AppointmentId
åå !
=
åå" #
$num
åå$ %
,
åå% &
	PatientId
çç 
=
çç 
$num
çç  !
,
çç! "
DoctorId
éé 
=
éé 
$num
éé  
,
éé  !
ScheduledDate
èè !
=
èè" #
new
èè$ '
DateOnly
èè( 0
(
èè0 1
$num
èè1 5
,
èè5 6
$num
èè7 8
,
èè8 9
$num
èè: <
)
èè< =
,
èè= >
TimeSlot
êê 
=
êê !
AppointmentTimeSlot
êê 2
.
êê2 3
TenAM
êê3 8
,
êê8 9
Status
ëë 
=
ëë 
AppointmentStatus
ëë .
.
ëë. /
Pending
ëë/ 6
}
íí 
)
ìì 
;
ìì 
}
îî 	
}
ïï 
}ññ Ω
[C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\HealthController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
HealthController !
:" #
ControllerBase$ 2
{ 
[		 	
HttpGet			 
]		 
public

 
IActionResult

 
Get

  
(

  !
)

! "
{ 	
return 
Ok 
( 
new 
{ 
status 
= 
$str "
," #
service 
= 
$str *
} 
) 
; 
} 	
} 
} 