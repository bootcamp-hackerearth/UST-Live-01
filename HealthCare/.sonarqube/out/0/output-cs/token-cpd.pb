£
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Interfaces\IPatientService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface 
IPatientService $
{ 
Task 
< 
PatientListDto 
> 
GetByIdAsync )
() *
int* -
id. 0
)0 1
;1 2
Task		 
<		 
PagedResult		 
<		 
PatientListDto		 '
>		' (
>		( )
GetAllAsync		* 5
(		5 6
PatientFilter		6 C
filter		D J
)		J K
;		K L
Task 
AddAsync 
( 
CreatePatientDto &
dto' *
)* +
;+ ,
Task 
UpdateAsync 
( 
int 
id 
,  
UpdatePatientDto  0
dto1 4
)4 5
;5 6
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
Task 
< 
IEnumerable 
< 
PatientListDto '
>' (
>( )
SearchByNameAsync* ;
(; <
string< B
nameC G
)G H
;H I
Task 
< 
PatientListDto 
? 
> 
GetByUserIdAsync .
(. /
string/ 5
userId6 <
)< =
;= >
Task 
UpdateStatusAsync 
( 
int "
id# %
,% &
bool' +
isActive, 4
)4 5
;5 6
Task 
UpdateByUserIdAsync  
(  !
string! '
userId( .
,. /
UpdatePatientDto0 @
dtoA D
)D E
;E F
} 
} ı
nC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Interfaces\IHealthRecordService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface  
IHealthRecordService )
{ 
Task		 
AddAsync		 
(		 !
CreateHealthRecordDto		 +
dto		, /
)		/ 0
;		0 1
Task

 
UpdateAsync

 
(

 
int

 
id

 
,

  !
UpdateHealthRecordDto

! 6
dto

7 :
)

: ;
;

; <
Task 
DeleteAsync 
( 
int 
id  
)  !
;! "
Task 
< 
HealthRecordListDto  
>  !
GetByIdAsync" .
(. /
int/ 2
id3 5
)5 6
;6 7
Task 
< 
PagedResult 
< 
HealthRecordListDto ,
>, -
>- .
GetAllAsync/ :
(: ;
HealthRecordFilter; M
filterN T
)T U
;U V
Task 
< 
List 
< 
HealthRecordListDto %
>% &
>& '$
GetHealthRecordByPatient( @
(@ A
intA D
idE G
)G H
;H I
Task 
< 
List 
< 
HealthRecordListDto %
>% &
>& '(
GetHealthRecordByAppointment( D
(D E
intE H
idI K
)K L
;L M
} 
} ﬁ
hC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Interfaces\IDoctorService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface 
IDoctorService #
{ 
Task		 
<		 
DoctorListDto		 
?		 
>		 
GetByIdAsync		 )
(		) *
int		* -
id		. 0
)		0 1
;		1 2
Task

 
<

 
PagedResult

 
<

 
DoctorListDto

 &
>

& '
>

' (
GetAllAsync

) 4
(

4 5
DoctorFilter

5 A
filter

B H
)

H I
;

I J
Task 
AddAsync 
( 
DoctorRegisterDto '
dto( +
)+ ,
;, -
Task 
UpdateAsync 
( 
int 
id 
,  
UpdateDoctorDto! 0
dto1 4
)4 5
;5 6
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
Task 
UpdateStatusAsync 
( 
int "
id# %
,% &
bool' +
isActive, 4
)4 5
;5 6
Task 
CreateSlots 
( 
int 
id 
,  
List! %
<% &
string& ,
>, -
	timeslots. 7
)7 8
;8 9
Task 
< 
List 
< 
string 
> 
> 
GetSlots #
(# $
int$ '
doctorId( 0
)0 1
;1 2
Task 
<  
CreateLeaveResultDto !
>! "
CreateLeave# .
(. /
int/ 2
id3 5
,5 6
List7 ;
<; <
CreateLeaveDto< J
>J K
leavesL R
)R S
;S T
Task 
< 
List 
< 
DoctorListDto 
>  
>  !
AvailableDoctors" 2
(2 3
string3 9
specialisation: H
,H I
DateOnlyJ R
dateS W
)W X
;X Y
Task 
< 
List 
< 
string 
> 
> #
AvailableTimeSlotsCheck 2
(2 3
DateOnly3 ;
date< @
,@ A
intB E
doctorIdF N
)N O
;O P
} 
} Ÿ
fC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Interfaces\IAuthService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface 
IAuthService !
{ 
Task		  
RegisterPatientAsync		 !
(		! "
PatientRegisterDto		" 4
dto		5 8
)		8 9
;		9 :
Task 
RegisterDoctorAsync  
(  !
DoctorRegisterDto! 2
dto3 6
)6 7
;7 8
Task 
< 
AuthResponseDto 
> 

LoginAsync (
(( )
LoginDto) 1
dto2 5
)5 6
;6 7
} 
} Ò
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Interfaces\IAppointmentService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface 
IAppointmentService (
{ 
Task		 
AddAsync		 
(		  
CreateAppointmentDto		 *
dto		+ .
,		. /
int		/ 2
id		3 5
)		5 6
;		6 7
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
Task 
UpdateAsync 
( 
int 
id 
,   
UpdateAppointmentDto  4
dto5 8
)8 9
;9 :
Task 
UpdateStatusAsync 
( 
int "
id# %
,% & 
UpdateAppointmentDto' ;
dto< ?
)? @
;@ A
Task 
< 
AppointmentListDto 
>  
GetByIdAsync! -
(- .
int. 1
id2 4
)4 5
;5 6
Task 
< 
PagedResult 
< 
AppointmentListDto +
>+ ,
>, -
GetAllAsync. 9
(9 :
AppointmentFilter: K
filterL R
)R S
;S T
Task 
< 
bool 
> 
IsAvailable 
( 
DateOnly '
date( ,
,, -
int. 1
doctorId2 :
,: ;
string< B
timeSlotC K
)K L
;L M
Task 
< 
List 
<  
AppointmentReportDto &
>& '
>' (
GetDailyReport) 7
(7 8
)8 9
;9 :
Task 
< 
List 
< 
string 
> 
> 
AvailableTimeSlots -
(- .
DateOnly. 6
date7 ;
,; <
int= @
doctorIdA I
)I J
;J K
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &
GetDoctorSchedule' 8
(8 9
DateOnly9 A
dateB F
,F G
intH K
idL N
)N O
;O P
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &
GetPatientSchedule' 9
(9 :
DateOnly: B
dateC G
,G H
intI L
idM O
)O P
;P Q
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &#
GetAppointmentByPatient' >
(> ?
int? B
idC E
)E F
;F G
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &"
GetAppointmentByDoctor' =
(= >
int> A
idB D
)D E
;E F
Task *
CancelAppointmentsByDoctorDate +
(+ ,
int, /
doctorId0 8
,8 9
DateOnly: B
dateC G
)G H
;H I
} 
} «d
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\PatientService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
PatientService 
:  !
IPatientService" 1
{ 
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_repository. 9
;9 :
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
private 
readonly 
IMapper  
_mapper! (
;( )
public 
PatientService 
( 
IRepository )
<) *
Patient* 1
>1 2

repository3 =
,= >
HealthCareDbContext? R
contextS Z
,Z [
IMapper\ c
mapperd j
)j k
{ 	
_repository 
= 

repository $
;$ %
_context 
= 
context 
; 
_mapper 
= 
mapper 
; 
} 	
public 
async 
Task 
AddAsync "
(" #
CreatePatientDto# 3
dto4 7
)7 8
{ 	
var 
patient 
= 
_mapper !
.! "
Map" %
<% &
Patient& -
>- .
(. /
dto/ 2
)2 3
;3 4
await 
_repository 
. 
AddAsync &
(& '
patient' .
). /
;/ 0
await 
_context 
. 
SaveChangesAsync +
(+ ,
), -
;- .
} 	
public!! 
async!! 
Task!! 
UpdateAsync!! %
(!!% &
int!!& )
id!!* ,
,!!, -
UpdatePatientDto!!. >
dto!!? B
)!!B C
{"" 	
var## 
patient## 
=## 
await## 
_repository##  +
.##+ ,
GetByIdAsync##, 8
(##8 9
id##9 ;
)##; <
;##< =
if$$ 
($$ 
patient$$ 
==$$ 
null$$ 
)$$  
throw%% 
new%% $
PatientNotFoundException%% 2
(%%2 3
id%%3 5
)%%5 6
;%%6 7
_mapper&& 
.&& 
Map&& 
(&& 
dto&& 
,&& 
patient&& #
)&&# $
;&&$ %
await(( 
_repository(( 
.(( 
UpdateAsync(( )
((() *
patient((* 1
)((1 2
;((2 3
await)) 
_context)) 
.)) 
SaveChangesAsync)) +
())+ ,
))), -
;))- .
}** 	
public,, 
async,, 
Task,, 
DeleteAsync,, %
(,,% &
int,,& )
id,,* ,
),,, -
{-- 	
var.. 
patient.. 
=.. 
await.. 
_repository..  +
...+ ,
GetByIdAsync.., 8
(..8 9
id..9 ;
)..; <
;..< =
if// 
(// 
patient// 
==// 
null// 
)//  
throw00 
new00 $
PatientNotFoundException00 2
(002 3
id003 5
)005 6
;006 7
await11 
_repository11 
.11 
DeleteAsync11 )
(11) *
id11* ,
)11, -
;11- .
await22 
_context22 
.22 
SaveChangesAsync22 +
(22+ ,
)22, -
;22- .
}33 	
public66 
async66 
Task66 
<66 
PatientListDto66 (
>66( )
GetByIdAsync66* 6
(666 7
int667 :
id66; =
)66= >
{77 	
var88 
patient88 
=88 
await88 
_repository88  +
.88+ ,
GetByIdAsync88, 8
(888 9
id889 ;
)88; <
;88< =
if:: 
(:: 
patient:: 
==:: 
null:: 
)::  
throw;; 
new;; $
PatientNotFoundException;; 2
(;;2 3
id;;3 5
);;5 6
;;;6 7
return== 
_mapper== 
.== 
Map== 
<== 
PatientListDto== -
>==- .
(==. /
patient==/ 6
)==6 7
;==7 8
}>> 	
publicBB 
asyncBB 
TaskBB 
<BB 
IEnumerableBB %
<BB% &
PatientListDtoBB& 4
>BB4 5
>BB5 6
SearchByNameAsyncBB7 H
(BBH I
stringBBI O
nameBBP T
)BBT U
{CC 	
varDD 
patientsDD 
=DD 
awaitDD  
_contextDD! )
.DD) *
PatientsDD* 2
.EE 
WhereEE 
(EE 
pEE 
=>EE 
pEE 
.EE 
FullNameEE &
.EE& '
ToLowerEE' .
(EE. /
)EE/ 0
.EE0 1
ContainsEE1 9
(EE9 :
nameEE: >
.EE> ?
ToLowerEE? F
(EEF G
)EEG H
)EEH I
)EEI J
.FF 
ToListAsyncFF 
(FF 
)FF 
;FF 
returnHH 
_mapperHH 
.HH 
MapHH 
<HH 
IEnumerableHH *
<HH* +
PatientListDtoHH+ 9
>HH9 :
>HH: ;
(HH; <
patientsHH< D
)HHD E
;HHE F
}II 	
publicKK 
asyncKK 
TaskKK 
<KK 
PagedResultKK %
<KK% &
PatientListDtoKK& 4
>KK4 5
>KK5 6
GetAllAsyncKK7 B
(KKB C
PatientFilterKKC P
filterKKQ W
)KKW X
{LL 	

IQueryableMM 
<MM 
PatientMM 
>MM 
queryMM  %
=MM& '
_contextMM( 0
.MM0 1
PatientsMM1 9
.MM9 :
AsQueryableMM: E
(MME F
)MMF G
;MMG H
ifPP 
(PP 
filterPP 
.PP 
HasInsurancePP #
.PP# $
HasValuePP$ ,
)PP, -
{QQ 
ifRR 
(RR 
filterRR 
.RR 
HasInsuranceRR '
.RR' (
ValueRR( -
)RR- .
querySS 
=SS 
querySS !
.SS! "
WhereSS" '
(SS' (
pSS( )
=>SS* ,
pSS- .
.SS. /
InsuranceIdSS/ :
!=SS; =
nullSS> B
)SSB C
;SSC D
elseTT 
queryUU 
=UU 
queryUU !
.UU! "
WhereUU" '
(UU' (
pUU( )
=>UU* ,
pUU- .
.UU. /
InsuranceIdUU/ :
==UU; =
nullUU> B
)UUB C
;UUC D
}VV 
ifYY 
(YY 
!YY 
stringYY 
.YY 
IsNullOrWhiteSpaceYY *
(YY* +
filterYY+ 1
.YY1 2
FullNameYY2 :
)YY: ;
)YY; <
{ZZ 
query[[ 
=[[ 
query[[ 
.[[ 
Where[[ #
([[# $
p[[$ %
=>[[& (
p[[) *
.[[* +
FullName[[+ 3
.[[3 4
Contains[[4 <
([[< =
filter[[= C
.[[C D
FullName[[D L
)[[L M
)[[M N
;[[N O
}\\ 
var^^ 

totalCount^^ 
=^^ 
await^^ "
query^^# (
.^^( )

CountAsync^^) 3
(^^3 4
)^^4 5
;^^5 6
varaa 
itemsaa 
=aa 
awaitaa 
queryaa #
.bb 
Skipbb 
(bb 
(bb 
filterbb 
.bb 

PageNumberbb (
-bb) *
$numbb+ ,
)bb, -
*bb. /
filterbb0 6
.bb6 7
PageSizebb7 ?
)bb? @
.cc 
Takecc 
(cc 
filtercc 
.cc 
PageSizecc %
)cc% &
.dd 
ToListAsyncdd 
(dd 
)dd 
;dd 
returnff 
newff 
PagedResultff "
<ff" #
PatientListDtoff# 1
>ff1 2
{gg 
Itemshh 
=hh 
_mapperhh 
.hh  
Maphh  #
<hh# $
IEnumerablehh$ /
<hh/ 0
PatientListDtohh0 >
>hh> ?
>hh? @
(hh@ A
itemshhA F
)hhF G
,hhG H

PageNumberii 
=ii 
filterii #
.ii# $

PageNumberii$ .
,ii. /
PageSizejj 
=jj 
filterjj !
.jj! "
PageSizejj" *
,jj* +

TotalCountkk 
=kk 

totalCountkk '
}ll 
;ll 
}mm 	
publicoo 
asyncoo 
Taskoo 
UpdateStatusAsyncoo +
(oo+ ,
intoo, /
idoo0 2
,oo2 3
booloo4 8
isActiveoo9 A
)ooA B
{pp 	
varqq 
patientqq 
=qq 
awaitqq 
_repositoryqq  +
.qq+ ,
GetByIdAsyncqq, 8
(qq8 9
idqq9 ;
)qq; <
;qq< =
ifss 
(ss 
patientss 
isss 
nullss 
)ss  
throwtt 
newtt %
InvalidOperationExceptiontt 3
(tt3 4
$strtt4 H
)ttH I
;ttI J
patientvv 
.vv 
IsActivevv 
=vv 
isActivevv '
;vv' (
awaitxx 
_repositoryxx 
.xx 
UpdateAsyncxx )
(xx) *
patientxx* 1
)xx1 2
;xx2 3
awaityy 
_contextyy 
.yy 
SaveChangesAsyncyy +
(yy+ ,
)yy, -
;yy- .
}zz 	
public}} 
async}} 
Task}} 
<}} 
PatientListDto}} (
?}}( )
>}}) *
GetByUserIdAsync}}+ ;
(}}; <
string}}< B
userId}}C I
)}}I J
{~~ 	
var 
patient 
= 
await 
_context  (
.( )
Patients) 1
.
ÄÄ !
FirstOrDefaultAsync
ÄÄ $
(
ÄÄ$ %
p
ÄÄ% &
=>
ÄÄ' )
p
ÄÄ* +
.
ÄÄ+ ,
UserId
ÄÄ, 2
==
ÄÄ3 5
userId
ÄÄ6 <
)
ÄÄ< =
;
ÄÄ= >
if
ÇÇ 
(
ÇÇ 
patient
ÇÇ 
==
ÇÇ 
null
ÇÇ 
)
ÇÇ  
throw
ÉÉ 
new
ÉÉ '
InvalidOperationException
ÉÉ 3
(
ÉÉ3 4
$str
ÉÉ4 H
)
ÉÉH I
;
ÉÉI J
return
ÖÖ 
_mapper
ÖÖ 
.
ÖÖ 
Map
ÖÖ 
<
ÖÖ 
PatientListDto
ÖÖ -
>
ÖÖ- .
(
ÖÖ. /
patient
ÖÖ/ 6
)
ÖÖ6 7
;
ÖÖ7 8
}
ÜÜ 	
public
àà 
async
àà 
Task
àà !
UpdateByUserIdAsync
àà -
(
àà- .
string
àà. 4
userId
àà5 ;
,
àà; <
UpdatePatientDto
àà= M
dto
ààN Q
)
ààQ R
{
ââ 	
var
ää 
patient
ää 
=
ää 
await
ää 
_context
ää  (
.
ää( )
Patients
ää) 1
.
ãã !
FirstOrDefaultAsync
ãã $
(
ãã$ %
p
ãã% &
=>
ãã' )
p
ãã* +
.
ãã+ ,
UserId
ãã, 2
==
ãã3 5
userId
ãã6 <
)
ãã< =
;
ãã= >
if
çç 
(
çç 
patient
çç 
==
çç 
null
çç 
)
çç  
throw
éé 
new
éé '
InvalidOperationException
éé 3
(
éé3 4
$str
éé4 H
)
ééH I
;
ééI J
_mapper
êê 
.
êê 
Map
êê 
(
êê 
dto
êê 
,
êê 
patient
êê $
)
êê$ %
;
êê% &
await
íí 
_context
íí 
.
íí 
SaveChangesAsync
íí +
(
íí+ ,
)
íí, -
;
íí- .
}
ìì 	
}
òò 
}ôô ’R
rC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\HealthRecordService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
HealthRecordService $
:% & 
IHealthRecordService' ;
{ 
private 
readonly #
IHealthRecordRepository 0
_repository1 <
;< =
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
public 
HealthRecordService "
(" ##
IHealthRecordRepository# :

repository; E
,E F
IMapperG N
mapperO U
,U V
HealthCareDbContextW j
contextk r
)r s
{ 	
_repository 
= 

repository $
;$ %
_mapper 
= 
mapper 
; 
_context 
= 
context 
; 
} 	
public 
async 
Task 
AddAsync "
(" #!
CreateHealthRecordDto# 8
dto9 <
)< =
{ 	
var   
record   
=   
_mapper   
.   
Map   "
<  " #
HealthRecord  # /
>  / 0
(  0 1
dto  1 4
)  4 5
;  5 6
await!! 
_repository!! 
.!! 
AddAsync!! &
(!!& '
record!!' -
)!!- .
;!!. /
await"" 
_context"" 
."" 
SaveChangesAsync"" +
(""+ ,
)"", -
;""- .
}## 	
[&& 	
HttpPut&&	 
(&& 
$str&& 
)&& 
]&& 
['' 	
	Authorize''	 
('' 
Roles'' 
='' 
$str'' #
)''# $
]''$ %
public)) 
async)) 
Task)) 
UpdateAsync)) %
())% &
int))& )
id))* ,
,)), -!
UpdateHealthRecordDto))- B
dto))C F
)))F G
{** 	
var++ 
record++ 
=++ 
await++ 
_repository++ *
.++* +
GetByIdAsync+++ 7
(++7 8
id++8 :
)++: ;
;++; <
if,, 
(,, 
record,, 
==,, 
null,, 
),, 
throw-- 
new-- )
HealthRecordNotFoundException-- 7
(--7 8
id--8 :
)--: ;
;--; <
_mapper// 
.// 
Map// 
(// 
dto// 
,// 
record// "
)//" #
;//# $
await11 
_repository11 
.11 
UpdateAsync11 )
(11) *
record11* 0
)110 1
;111 2
await22 
_context22 
.22 
SaveChangesAsync22 +
(22+ ,
)22, -
;22- .
}33 	
[66 	

HttpDelete66	 
(66 
$str66 
)66 
]66  
[77 	
	Authorize77	 
(77 
Roles77 
=77 
$str77 "
)77" #
]77# $
public99 
async99 
Task99 
DeleteAsync99 %
(99% &
int99& )
id99* ,
)99, -
{:: 	
var<< 
record<< 
=<< 
await<< 
_repository<< *
.<<* +
GetByIdAsync<<+ 7
(<<7 8
id<<8 :
)<<: ;
;<<; <
if== 
(== 
record== 
==== 
null== 
)== 
throw>> 
new>> )
HealthRecordNotFoundException>> 7
(>>7 8
id>>8 :
)>>: ;
;>>; <
await?? 
_repository?? 
.?? 
DeleteAsync?? )
(??) *
id??* ,
)??, -
;??- .
await@@ 
_context@@ 
.@@ 
SaveChangesAsync@@ +
(@@+ ,
)@@, -
;@@- .
}AA 	
publicCC 
asyncCC 
TaskCC 
<CC 
HealthRecordListDtoCC -
>CC- .
GetByIdAsyncCC/ ;
(CC; <
intCC< ?
idCC@ B
)CCB C
{DD 	
varEE 
recordEE 
=EE 
awaitEE 
_repositoryEE *
.EE* +
GetByIdAsyncEE+ 7
(EE7 8
idEE8 :
)EE: ;
;EE; <
returnFF 
recordFF 
==FF 
nullFF !
?FF" #
nullFF$ (
:FF) *
_mapperFF+ 2
.FF2 3
MapFF3 6
<FF6 7
HealthRecordListDtoFF7 J
?FFJ K
>FFK L
(FFL M
recordFFM S
)FFS T
;FFT U
}GG 	
[II 	
HttpGetII	 
]II 
[JJ 	
	AuthorizeJJ	 
(JJ 
RolesJJ 
=JJ 
$strJJ )
)JJ) *
]JJ* +
publicKK 
asyncKK 
TaskKK 
<KK 
PagedResultKK %
<KK% &
HealthRecordListDtoKK& 9
>KK9 :
>KK: ;
GetAllAsyncKK< G
(KKG H
HealthRecordFilterKKH Z
filterKK[ a
)KKa b
{LL 	

ExpressionNN 
<NN 
FuncNN 
<NN 
HealthRecordNN (
,NN( )
boolNN* .
>NN. /
>NN/ 0
?NN0 1
	predicateNN2 ;
=NN< =
nullNN> B
;NNB C
ifPP 
(PP 
filterPP 
.PP 
	VisitDatePP  
.PP  !
HasValuePP! )
)PP) *
{QQ 
varRR 
startRR 
=RR 
filterRR "
.RR" #
	VisitDateRR# ,
.RR, -
ValueRR- 2
.RR2 3

ToDateTimeRR3 =
(RR= >
TimeOnlyRR> F
.RRF G
MinValueRRG O
)RRO P
;RRP Q
varSS 
endSS 
=SS 
startSS 
.SS  
AddDaysSS  '
(SS' (
$numSS( )
)SS) *
;SS* +
	predicateUU 
=UU 
hrUU 
=>UU !
hrUU" $
.UU$ %
	VisitDateUU% .
>=UU/ 1
startUU2 7
&&UU8 :
hrUU; =
.UU= >
	VisitDateUU> G
<UUH I
endUUJ M
;UUM N
}VV 
FuncYY 
<YY 

IQueryableYY 
<YY 
HealthRecordYY (
>YY( )
,YY) *
IOrderedQueryableYY+ <
<YY< =
HealthRecordYY= I
>YYI J
>YYJ K
orderByYYL S
=YYT U
qZZ 
=>ZZ 
qZZ 
.ZZ 
OrderByZZ 
(ZZ 
hrZZ !
=>ZZ" $
hrZZ% '
.ZZ' (
	VisitDateZZ( 1
)ZZ1 2
;ZZ2 3
var]] 
pagedResult]] 
=]] 
await]] #
_repository]]$ /
.]]/ 0
GetAllAsync]]0 ;
(]]; <
filter^^ 
.^^ 

PageNumber^^ !
,^^! "
filter__ 
.__ 
PageSize__ 
,__  
	predicate`` 
,`` 
orderByaa 
)bb 
;bb 
returnee 
newee 
PagedResultee "
<ee" #
HealthRecordListDtoee# 6
>ee6 7
{ff 
Itemsgg 
=gg 
_mappergg 
.gg  
Mapgg  #
<gg# $
IEnumerablegg$ /
<gg/ 0
HealthRecordListDtogg0 C
>ggC D
>ggD E
(ggE F
pagedResultggF Q
.ggQ R
ItemsggR W
)ggW X
,ggX Y

PageNumberhh 
=hh 
pagedResulthh (
.hh( )

PageNumberhh) 3
,hh3 4
PageSizeii 
=ii 
pagedResultii &
.ii& '
PageSizeii' /
,ii/ 0

TotalCountjj 
=jj 
pagedResultjj (
.jj( )

TotalCountjj) 3
}kk 
;kk 
}ll 	
[oo 	
HttpGetoo	 
(oo 
$stroo *
)oo* +
]oo+ ,
[pp 	
	Authorizepp	 
(pp 
Rolespp 
=pp 
$strpp )
)pp) *
]pp* +
publicrr 
asyncrr 
Taskrr 
<rr 
Listrr 
<rr 
HealthRecordListDtorr 2
>rr2 3
>rr3 4$
GetHealthRecordByPatientrr5 M
(rrM N
intrrN Q
idrrR T
)rrT U
{ss 	
vartt 
recordstt 
=tt 
awaittt 
_repositorytt  +
.tt+ ,$
GetHealthRecordByPatienttt, D
(ttD E
idttE G
)ttG H
;ttH I
returnvv 
_mappervv 
.vv 
Mapvv 
<vv 
Listvv #
<vv# $
HealthRecordListDtovv$ 7
>vv7 8
>vv8 9
(vv9 :
recordsvv: A
)vvA B
;vvB C
}ww 	
[zz 	
HttpGetzz	 
(zz 
$strzz 2
)zz2 3
]zz3 4
[{{ 	
	Authorize{{	 
({{ 
Roles{{ 
={{ 
$str{{ )
){{) *
]{{* +
public|| 
async|| 
Task|| 
<|| 
List|| 
<|| 
HealthRecordListDto|| 2
>||2 3
>||3 4(
GetHealthRecordByAppointment||5 Q
(||Q R
int||R U
id||V X
)||X Y
{}} 	
var~~ 
records~~ 
=~~ 
await~~ 
_repository~~  +
.~~+ ,(
GetHealthRecordByAppointment~~, H
(~~H I
id~~I K
)~~K L
;~~L M
return
ÄÄ 
_mapper
ÄÄ 
.
ÄÄ 
Map
ÄÄ 
<
ÄÄ 
List
ÄÄ #
<
ÄÄ# $!
HealthRecordListDto
ÄÄ$ 7
>
ÄÄ7 8
>
ÄÄ8 9
(
ÄÄ9 :
records
ÄÄ: A
)
ÄÄA B
;
ÄÄB C
}
ÅÅ 	
}
ÑÑ 
}ÖÖ °Ö
lC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\DoctorService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
DoctorService 
:  
IDoctorService! /
{ 
private 
readonly 
IDoctorRepository *
_repository+ 6
;6 7
private 
readonly "
IAppointmentRepository /"
_appointmentRepository0 F
;F G
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
private 
readonly 
IMapper  
_mapper! (
;( )
public 
DoctorService 
( 
IDoctorRepository .

repository/ 9
,9 :
HealthCareDbContext; N
contextO V
,V W
IMapperX _
mapper` f
,f g"
IAppointmentRepositoryh ~"
appointmentRepository	 î
)
î ï
{ 	
_repository 
= 

repository $
;$ %
_context 
= 
context 
; 
_mapper 
= 
mapper 
; "
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
} 	
public 
async 
Task 
AddAsync "
(" #
DoctorRegisterDto# 4
dto5 8
)8 9
{ 	
var 
doctor 
= 
_mapper  
.  !
Map! $
<$ %
Doctor% +
>+ ,
(, -
dto- 0
)0 1
;1 2
await   
_repository   
.   
AddAsync   &
(  & '
doctor  ' -
)  - .
;  . /
await!! 
_context!! 
.!! 
SaveChangesAsync!! +
(!!+ ,
)!!, -
;!!- .
}"" 	
public$$ 
async$$ 
Task$$ 
UpdateAsync$$ %
($$% &
int$$& )
id$$* ,
,$$, -
UpdateDoctorDto$$. =
dto$$> A
)$$A B
{%% 	
var&& 
doctor&& 
=&& 
await&& 
_repository&& *
.&&* +
GetByIdAsync&&+ 7
(&&7 8
id&&8 :
)&&: ;
;&&; <
if'' 
('' 
doctor'' 
=='' 
null'' 
)'' 
throw(( 
new(( #
DoctorNotFoundException(( 1
(((1 2
id((2 4
)((4 5
;((5 6
_mapper)) 
.)) 
Map)) 
()) 
dto)) 
,)) 
doctor)) #
)))# $
;))$ %
await++ 
_repository++ 
.++ 
UpdateAsync++ )
(++) *
doctor++* 0
)++0 1
;++1 2
await,, 
_context,, 
.,, 
SaveChangesAsync,, +
(,,+ ,
),,, -
;,,- .
}-- 	
public// 
async// 
Task// 
DeleteAsync// %
(//% &
int//& )
id//* ,
)//, -
{00 	
var11 
doctor11 
=11 
await11 
_repository11 *
.11* +
GetByIdAsync11+ 7
(117 8
id118 :
)11: ;
;11; <
if22 
(22 
doctor22 
==22 
null22 
)22 
throw33 
new33 #
DoctorNotFoundException33 1
(331 2
id332 4
)334 5
;335 6
await44 
_repository44 
.44 
DeleteAsync44 )
(44) *
id44* ,
)44, -
;44- .
await55 
_context55 
.55 
SaveChangesAsync55 +
(55+ ,
)55, -
;55- .
}66 	
public88 
async88 
Task88 
<88 
DoctorListDto88 '
?88' (
>88( )
GetByIdAsync88* 6
(886 7
int887 :
id88; =
)88= >
{99 	
var:: 
doctor:: 
=:: 
await:: 
_repository:: *
.::* +
GetByIdAsync::+ 7
(::7 8
id::8 :
)::: ;
;::; <
return;; 
doctor;; 
==;; 
null;; !
?;;" #
null;;$ (
:;;) *
_mapper;;+ 2
.;;2 3
Map;;3 6
<;;6 7
DoctorListDto;;7 D
?;;D E
>;;E F
(;;F G
doctor;;G M
);;M N
;;;N O
}<< 	
public>> 
async>> 
Task>> 
<>> 
PagedResult>> %
<>>% &
DoctorListDto>>& 3
>>>3 4
>>>4 5
GetAllAsync>>6 A
(>>A B
DoctorFilter>>B N
filter>>O U
)>>U V
{?? 	

ExpressionAA 
<AA 
FuncAA 
<AA 
DoctorAA "
,AA" #
boolAA$ (
>AA( )
>AA) *
?AA* +
	predicateAA, 5
=AA6 7
nullAA8 <
;AA< =
ifCC 
(CC 
!CC 
stringCC 
.CC 
IsNullOrWhiteSpaceCC *
(CC* +
filterCC+ 1
.CC1 2
SpecialisationCC2 @
)CC@ A
&&CCB D
filterCCE K
.CCK L
MinExperienceCCL Y
.CCY Z
HasValueCCZ b
)CCb c
{DD 
	predicateEE 
=EE 
dEE 
=>EE  
dEE! "
.EE" #
SpecialisationEE# 1
==EE2 4
filterEE5 ;
.EE; <
SpecialisationEE< J
&&FF  
dFF! "
.FF" #
YearsOfExperienceFF# 4
>=FF5 7
filterFF8 >
.FF> ?
MinExperienceFF? L
.FFL M
ValueFFM R
;FFR S
}GG 
elseHH 
ifHH 
(HH 
!HH 
stringHH 
.HH 
IsNullOrWhiteSpaceHH /
(HH/ 0
filterHH0 6
.HH6 7
SpecialisationHH7 E
)HHE F
)HHF G
{II 
	predicateJJ 
=JJ 
dJJ 
=>JJ  
dJJ! "
.JJ" #
SpecialisationJJ# 1
==JJ2 4
filterJJ5 ;
.JJ; <
SpecialisationJJ< J
;JJJ K
}KK 
elseLL 
ifLL 
(LL 
filterLL 
.LL 
MinExperienceLL )
.LL) *
HasValueLL* 2
)LL2 3
{MM 
	predicateNN 
=NN 
dNN 
=>NN  
dNN! "
.NN" #
YearsOfExperienceNN# 4
>=NN5 7
filterNN8 >
.NN> ?
MinExperienceNN? L
.NNL M
ValueNNM R
;NNR S
}OO 
FuncRR 
<RR 

IQueryableRR 
<RR 
DoctorRR "
>RR" #
,RR# $
IOrderedQueryableRR% 6
<RR6 7
DoctorRR7 =
>RR= >
>RR> ?
orderByRR@ G
=RRH I
qSS 
=>SS 
qSS 
.SS 
OrderByDescendingSS (
(SS( )
dSS) *
=>SS+ -
dSS. /
.SS/ 0
YearsOfExperienceSS0 A
)SSA B
;SSB C
varVV 
pagedResultVV 
=VV 
awaitVV #
_repositoryVV$ /
.VV/ 0
GetAllAsyncVV0 ;
(VV; <
filterWW 
.WW 

PageNumberWW !
,WW! "
filterXX 
.XX 
PageSizeXX 
,XX  
	predicateYY 
,YY 
orderByZZ 
)[[ 
;[[ 
return^^ 
new^^ 
PagedResult^^ "
<^^" #
DoctorListDto^^# 0
>^^0 1
{__ 
Items`` 
=`` 
_mapper`` 
.``  
Map``  #
<``# $
IEnumerable``$ /
<``/ 0
DoctorListDto``0 =
>``= >
>``> ?
(``? @
pagedResult``@ K
.``K L
Items``L Q
)``Q R
,``R S

PageNumberaa 
=aa 
pagedResultaa (
.aa( )

PageNumberaa) 3
,aa3 4
PageSizebb 
=bb 
pagedResultbb &
.bb& '
PageSizebb' /
,bb/ 0

TotalCountcc 
=cc 
pagedResultcc (
.cc( )

TotalCountcc) 3
}dd 
;dd 
}ee 	
publicgg 
asyncgg 
Taskgg 
UpdateStatusAsyncgg +
(gg+ ,
intgg, /
idgg0 2
,gg2 3
boolgg4 8
isActivegg9 A
)ggA B
{hh 	
varii 
doctorii 
=ii 
awaitii 
_repositoryii *
.ii* +
GetByIdAsyncii+ 7
(ii7 8
idii8 :
)ii: ;
;ii; <
ifkk 
(kk 
doctorkk 
iskk 
nullkk 
)kk 
throwll 
newll %
InvalidOperationExceptionll 3
(ll3 4
$strll4 G
)llG H
;llH I
doctornn 
.nn 
IsActivenn 
=nn 
isActivenn &
;nn& '
awaitpp 
_repositorypp 
.pp 
UpdateAsyncpp )
(pp) *
doctorpp* 0
)pp0 1
;pp1 2
awaitqq 
_contextqq 
.qq 
SaveChangesAsyncqq +
(qq+ ,
)qq, -
;qq- .
}rr 	
publictt 
asynctt 
Tasktt 
<tt 
Listtt 
<tt 
stringtt %
>tt% &
>tt& '
GetSlotstt( 0
(tt0 1
inttt1 4
doctorIdtt5 =
)tt= >
{uu 	
varvv 
slotsvv 
=vv 
awaitvv 
_repositoryvv )
.vv) *
GetSlotsvv* 2
(vv2 3
doctorIdvv3 ;
)vv; <
;vv< =
ifxx 
(xx 
slotsxx 
.xx 
Countxx 
==xx 
$numxx  
)xx  !
throwyy 
newyy %
InvalidOperationExceptionyy 3
(yy3 4
$stryy4 _
)yy_ `
;yy` a
return{{ 
slots{{ 
;{{ 
}|| 	
public~~ 
async~~ 
Task~~ 
CreateSlots~~ %
(~~% &
int~~& )
id~~* ,
,~~, -
List~~. 2
<~~2 3
string~~3 9
>~~9 :
	timeslots~~; D
)~~D E
{ 	
await
ÄÄ 
_repository
ÄÄ 
.
ÄÄ 
CreateSlots
ÄÄ )
(
ÄÄ) *
id
ÄÄ* ,
,
ÄÄ, -
	timeslots
ÄÄ. 7
)
ÄÄ7 8
;
ÄÄ8 9
await
ÅÅ 
_context
ÅÅ 
.
ÅÅ 
SaveChangesAsync
ÅÅ +
(
ÅÅ+ ,
)
ÅÅ, -
;
ÅÅ- .
}
ÇÇ 	
public
ÑÑ 
async
ÑÑ 
Task
ÑÑ 
<
ÑÑ 
List
ÑÑ 
<
ÑÑ 
string
ÑÑ %
>
ÑÑ% &
>
ÑÑ& '%
AvailableTimeSlotsCheck
ÑÑ( ?
(
ÑÑ? @
DateOnly
ÑÑ@ H
date
ÑÑI M
,
ÑÑM N
int
ÑÑO R
doctorId
ÑÑS [
)
ÑÑ[ \
{
ÖÖ 	
var
ÜÜ 
allSlots
ÜÜ 
=
ÜÜ 
await
ÜÜ  
_repository
ÜÜ! ,
.
ÜÜ, -
GetSlots
ÜÜ- 5
(
ÜÜ5 6
doctorId
ÜÜ6 >
)
ÜÜ> ?
;
ÜÜ? @
var
áá 
bookedSlots
áá 
=
áá 
await
áá #$
_appointmentRepository
áá$ :
.
áá: ; 
AvailableTimeSlots
áá; M
(
ááM N
date
ááN R
,
ááR S
doctorId
ááT \
)
áá\ ]
;
áá] ^
return
àà 
allSlots
àà 
.
àà 
Except
àà "
(
àà" #
bookedSlots
àà# .
)
àà. /
.
àà/ 0
ToList
àà0 6
(
àà6 7
)
àà7 8
;
àà8 9
}
ââ 	
public
ãã 
async
ãã 
Task
ãã 
<
ãã "
CreateLeaveResultDto
ãã .
>
ãã. /
CreateLeave
ãã0 ;
(
ãã; <
int
ãã< ?
id
ãã@ B
,
ããB C
List
ããD H
<
ããH I
CreateLeaveDto
ããI W
>
ããW X
leaves
ããY _
)
ãã_ `
{
åå 	
var
çç 
result
çç 
=
çç 
new
çç "
CreateLeaveResultDto
çç 1
(
çç1 2
)
çç2 3
;
çç3 4
var
éé 
existingLeaves
éé 
=
éé  
await
éé! &
_repository
éé' 2
.
éé2 3!
GetLeavesByDoctorId
éé3 F
(
ééF G
id
ééG I
)
ééI J
;
ééJ K
var
èè  
existingLeaveDates
èè "
=
èè# $
existingLeaves
èè% 3
.
èè3 4
Select
èè4 :
(
èè: ;
l
èè; <
=>
èè= ?
l
èè@ A
.
èèA B
	LeaveDate
èèB K
)
èèK L
.
èèL M
	ToHashSet
èèM V
(
èèV W
)
èèW X
;
èèX Y
var
ëë 
leavesToCreate
ëë 
=
ëë  
new
ëë! $
List
ëë% )
<
ëë) *
CreateLeaveDto
ëë* 8
>
ëë8 9
(
ëë9 :
)
ëë: ;
;
ëë; <
foreach
ìì 
(
ìì 
var
ìì 
leave
ìì 
in
ìì !
leaves
ìì" (
)
ìì( )
{
îî 
if
ïï 
(
ïï  
existingLeaveDates
ïï &
.
ïï& '
Contains
ïï' /
(
ïï/ 0
leave
ïï0 5
.
ïï5 6
	LeaveDate
ïï6 ?
)
ïï? @
)
ïï@ A
{
ññ 
result
óó 
.
óó 
SkippedDates
óó '
.
óó' (
Add
óó( +
(
óó+ ,
leave
óó, 1
.
óó1 2
	LeaveDate
óó2 ;
)
óó; <
;
óó< =
continue
òò 
;
òò 
}
ôô 
var
õõ 
availableSlots
õõ "
=
õõ# $
await
õõ% *%
AvailableTimeSlotsCheck
õõ+ B
(
õõB C
leave
õõC H
.
õõH I
	LeaveDate
õõI R
,
õõR S
id
õõT V
)
õõV W
;
õõW X
var
úú 
allSlots
úú 
=
úú 
await
úú $
GetSlots
úú% -
(
úú- .
id
úú. 0
)
úú0 1
;
úú1 2
if
ûû 
(
ûû 
availableSlots
ûû "
.
ûû" #
Count
ûû# (
!=
ûû) +
allSlots
ûû, 4
.
ûû4 5
Count
ûû5 :
)
ûû: ;
{
üü 
await
°° $
_appointmentRepository
°° 0
.
°°0 1,
CancelAppointmentsByDoctorDate
°°1 O
(
°°O P
id
°°P R
,
°°R S
leave
°°T Y
.
°°Y Z
	LeaveDate
°°Z c
)
°°c d
;
°°d e
await
¢¢ 
_context
¢¢ "
.
¢¢" #
SaveChangesAsync
¢¢# 3
(
¢¢3 4
)
¢¢4 5
;
¢¢5 6
}
§§ 
leavesToCreate
¶¶ 
.
¶¶ 
Add
¶¶ "
(
¶¶" #
leave
¶¶# (
)
¶¶( )
;
¶¶) *
}
ßß 
if
©© 
(
©© 
leavesToCreate
©© 
.
©© 
Count
©© $
>
©©% &
$num
©©' (
)
©©( )
{
™™ 
await
´´ 
_repository
´´ !
.
´´! "
CreateLeaves
´´" .
(
´´. /
id
´´/ 1
,
´´1 2
leavesToCreate
´´3 A
)
´´A B
;
´´B C
await
¨¨ 
_context
¨¨ 
.
¨¨ 
SaveChangesAsync
¨¨ /
(
¨¨/ 0
)
¨¨0 1
;
¨¨1 2
}
≠≠ 
return
ØØ 
result
ØØ 
;
ØØ 
}
∞∞ 	
public
≤≤ 
async
≤≤ 
Task
≤≤ 
<
≤≤ 
List
≤≤ 
<
≤≤ 
DoctorListDto
≤≤ ,
>
≤≤, -
>
≤≤- .
AvailableDoctors
≤≤/ ?
(
≤≤? @
string
≤≤@ F
specialisation
≤≤G U
,
≤≤U V
DateOnly
≤≤W _
date
≤≤` d
)
≤≤d e
=>
≤≤f h
await
≥≥ 
_repository
≥≥ 
.
≥≥ 
AvailableDoctors
≥≥ .
(
≥≥. /
specialisation
≥≥/ =
,
≥≥= >
date
≥≥? C
)
≥≥C D
;
≥≥D E
}
∂∂ 
}∑∑ õÜ
jC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\AuthService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
readonly 
UserManager $
<$ %
IdentityUser% 1
>1 2
_userManager3 ?
;? @
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 
IPatientRepository +
_patientRepo, 8
;8 9
private 
readonly 
IDoctorRepository *
_doctorRepo+ 6
;6 7
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
public 
AuthService 
( 
UserManager 
< 
IdentityUser $
>$ %
userManager& 1
,1 2
IMapper 
mapper 
, 
IPatientRepository 
patientRepo *
,* +
IDoctorRepository 

doctorRepo (
,( )
IConfiguration 
configuration (
,( )
HealthCareDbContext 
context  '
)' (
{ 	
_userManager   
=   
userManager   &
;  & '
_mapper!! 
=!! 
mapper!! 
;!! 
_patientRepo"" 
="" 
patientRepo"" &
;""& '
_doctorRepo## 
=## 

doctorRepo## $
;##$ %
_configuration$$ 
=$$ 
configuration$$ *
;$$* +
_context%% 
=%% 
context%% 
;%% 
}&& 	
private)) 
async)) 
Task)) 
<)) 
IdentityUser)) '
>))' (
CreateUserAsync))) 8
())8 9
string))9 ?
email))@ E
,))E F
string))G M
password))N V
,))V W
string))X ^
role))_ c
)))c d
{** 	
var++ 
existingUser++ 
=++ 
await++ $
_userManager++% 1
.++1 2
FindByEmailAsync++2 B
(++B C
email++C H
)++H I
;++I J
if-- 
(-- 
existingUser-- 
!=-- 
null--  $
)--$ %
throw.. 
new.. 
	Exception.. #
(..# $
$str..$ :
)..: ;
;..; <
var00 
user00 
=00 
new00 
IdentityUser00 '
{11 
UserName22 
=22 
email22  
,22  !
Email33 
=33 
email33 
}44 
;44 
var66 
result66 
=66 
await66 
_userManager66 +
.66+ ,
CreateAsync66, 7
(667 8
user668 <
,66< =
password66> F
)66F G
;66G H
if88 
(88 
!88 
result88 
.88 
	Succeeded88 !
)88! "
throw99 
new99 
	Exception99 #
(99# $
string99$ *
.99* +
Join99+ /
(99/ 0
$str990 4
,994 5
result996 <
.99< =
Errors99= C
.99C D
Select99D J
(99J K
e99K L
=>99M O
e99P Q
.99Q R
Description99R ]
)99] ^
)99^ _
)99_ `
;99` a
await;; 
_userManager;; 
.;; 
AddToRoleAsync;; -
(;;- .
user;;. 2
,;;2 3
role;;4 8
);;8 9
;;;9 :
return== 
user== 
;== 
}>> 	
publicAA 
asyncAA 
TaskAA  
RegisterPatientAsyncAA .
(AA. /
PatientRegisterDtoAA/ A
dtoAAB E
)AAE F
{BB 	
ifDD 
(DD 
dtoDD 
.DD 
PasswordDD 
!=DD 
dtoDD  #
.DD# $
ConfirmPasswordDD$ 3
)DD3 4
throwEE 
newEE 
	ExceptionEE #
(EE# $
$strEE$ <
)EE< =
;EE= >
varHH 
userHH 
=HH 
awaitHH 
CreateUserAsyncHH ,
(HH, -
dtoHH- 0
.HH0 1
EmailHH1 6
,HH6 7
dtoHH8 ;
.HH; <
PasswordHH< D
,HHD E
$strHHF O
)HHO P
;HHP Q
varJJ 
patientJJ 
=JJ 
_mapperJJ !
.JJ! "
MapJJ" %
<JJ% &
PatientJJ& -
>JJ- .
(JJ. /
dtoJJ/ 2
)JJ2 3
;JJ3 4
patientMM 
.MM 
UserIdMM 
=MM 
userMM !
.MM! "
IdMM" $
;MM$ %
awaitOO 
_patientRepoOO 
.OO 
AddAsyncOO '
(OO' (
patientOO( /
)OO/ 0
;OO0 1
awaitPP 
_contextPP 
.PP 
SaveChangesAsyncPP +
(PP+ ,
)PP, -
;PP- .
}QQ 	
publicTT 
asyncTT 
TaskTT 
RegisterDoctorAsyncTT -
(TT- .
DoctorRegisterDtoTT. ?
dtoTT@ C
)TTC D
{UU 	
ifWW 
(WW 
dtoWW 
.WW 
PasswordWW 
!=WW 
dtoWW  #
.WW# $
ConfirmPasswordWW$ 3
)WW3 4
throwXX 
newXX 
	ExceptionXX #
(XX# $
$strXX$ <
)XX< =
;XX= >
var[[ 
user[[ 
=[[ 
await[[ 
CreateUserAsync[[ ,
([[, -
dto[[- 0
.[[0 1
Email[[1 6
,[[6 7
dto[[8 ;
.[[; <
Password[[< D
,[[D E
$str[[F N
)[[N O
;[[O P
var]] 
doctor]] 
=]] 
_mapper]]  
.]]  !
Map]]! $
<]]$ %
Doctor]]% +
>]]+ ,
(]], -
dto]]- 0
)]]0 1
;]]1 2
doctor__ 
.__ 
UserId__ 
=__ 
user__  
.__  !
Id__! #
;__# $
await`` 
_doctorRepo`` 
.`` 
AddAsync`` &
(``& '
doctor``' -
)``- .
;``. /
awaitaa 
_contextaa 
.aa 
SaveChangesAsyncaa +
(aa+ ,
)aa, -
;aa- .
awaitcc 
_doctorRepocc 
.cc 
CreateSlotscc )
(cc) *
doctorcc* 0
.cc0 1
DoctorIdcc1 9
,cc9 :
dtocc; >
.cc> ?
	TimeSlotscc? H
)ccH I
;ccI J
awaitdd 
_contextdd 
.dd 
SaveChangesAsyncdd +
(dd+ ,
)dd, -
;dd- .
}ee 	
publichh 
asynchh 
Taskhh 
<hh 
AuthResponseDtohh )
>hh) *

LoginAsynchh+ 5
(hh5 6
LoginDtohh6 >
dtohh? B
)hhB C
{ii 	
varjj 
userjj 
=jj 
awaitjj 
_userManagerjj )
.jj) *
FindByEmailAsyncjj* :
(jj: ;
dtojj; >
.jj> ?
Emailjj? D
)jjD E
;jjE F
ifll 
(ll 
userll 
==ll 
nullll 
)ll 
throwmm 
newmm 
	Exceptionmm #
(mm# $
$strmm$ ?
)mm? @
;mm@ A
varoo 
validPasswordoo 
=oo 
awaitoo  %
_userManageroo& 2
.oo2 3
CheckPasswordAsyncoo3 E
(ooE F
userooF J
,ooJ K
dtoooL O
.ooO P
PasswordooP X
)ooX Y
;ooY Z
ifqq 
(qq 
!qq 
validPasswordqq 
)qq 
throwrr 
newrr 
	Exceptionrr #
(rr# $
$strrr$ ?
)rr? @
;rr@ A
vartt 
rolestt 
=tt 
awaittt 
_userManagertt *
.tt* +
GetRolesAsynctt+ 8
(tt8 9
usertt9 =
)tt= >
;tt> ?
ifvv 
(vv 
!vv 
rolesvv 
.vv 
Anyvv 
(vv 
)vv 
)vv 
throwww 
newww 
	Exceptionww #
(ww# $
$strww$ ?
)ww? @
;ww@ A
varzz 
rolezz 
=zz 
roleszz 
.zz 
Firstzz "
(zz" #
)zz# $
;zz$ %
string|| 
token|| 
;|| 
if~~ 
(~~ 
role~~ 
==~~ 
$str~~ !
)~~! "
{ 
var
ÄÄ 
patient
ÄÄ 
=
ÄÄ 
await
ÄÄ #
_patientRepo
ÄÄ$ 0
.
ÄÄ0 1
GetByUserIdAsync
ÄÄ1 A
(
ÄÄA B
user
ÄÄB F
.
ÄÄF G
Id
ÄÄG I
)
ÄÄI J
;
ÄÄJ K
if
ÇÇ 
(
ÇÇ 
patient
ÇÇ 
==
ÇÇ 
null
ÇÇ #
)
ÇÇ# $
throw
ÉÉ 
new
ÉÉ 
	Exception
ÉÉ '
(
ÉÉ' (
$str
ÉÉ( B
)
ÉÉB C
;
ÉÉC D
token
ÖÖ 
=
ÖÖ 
GenerateJwtToken
ÖÖ (
(
ÖÖ( )
user
ÖÖ) -
,
ÖÖ- .
role
ÖÖ/ 3
,
ÖÖ3 4
	patientId
ÖÖ5 >
:
ÖÖ> ?
patient
ÖÖ@ G
.
ÖÖG H
	PatientId
ÖÖH Q
)
ÖÖQ R
;
ÖÖR S
}
ÜÜ 
else
áá 
if
áá 
(
áá 
role
áá 
==
áá 
$str
áá %
)
áá% &
{
àà 
var
ââ 
doctor
ââ 
=
ââ 
await
ââ "
_doctorRepo
ââ# .
.
ââ. /
GetByUserIdAsync
ââ/ ?
(
ââ? @
user
ââ@ D
.
ââD E
Id
ââE G
)
ââG H
;
ââH I
await
ää 
_context
ää 
.
ää 
SaveChangesAsync
ää /
(
ää/ 0
)
ää0 1
;
ää1 2
await
åå 
_context
åå 
.
åå 
SaveChangesAsync
åå /
(
åå/ 0
)
åå0 1
;
åå1 2
if
éé 
(
éé 
doctor
éé 
==
éé 
null
éé "
)
éé" #
throw
èè 
new
èè 
	Exception
èè '
(
èè' (
$str
èè( A
)
èèA B
;
èèB C
token
ëë 
=
ëë 
GenerateJwtToken
ëë (
(
ëë( )
user
ëë) -
,
ëë- .
role
ëë/ 3
,
ëë3 4
doctorId
ëë5 =
:
ëë= >
doctor
ëë? E
.
ëëE F
DoctorId
ëëF N
)
ëëN O
;
ëëO P
}
íí 
else
ìì 
if
ìì 
(
ìì 
role
ìì 
==
ìì 
$str
ìì $
)
ìì$ %
{
îî 
token
ññ 
=
ññ 
GenerateJwtToken
ññ (
(
ññ( )
user
ññ) -
,
ññ- .
role
ññ/ 3
)
ññ3 4
;
ññ4 5
}
óó 
else
òò 
{
ôô 
throw
öö 
new
öö 
	Exception
öö #
(
öö# $
$str
öö$ =
)
öö= >
;
öö> ?
}
õõ 
return
ùù 
new
ùù 
AuthResponseDto
ùù &
{
ûû 
AccessToken
üü 
=
üü 
token
üü #
,
üü# $
Role
†† 
=
†† 
role
†† 
}
°° 
;
°° 
}
¢¢ 	
private
•• 
string
•• 
GenerateJwtToken
•• '
(
••' (
IdentityUser
¶¶ 
user
¶¶ 
,
¶¶ 
string
ßß 
role
ßß 
,
ßß 
int
®® 
?
®® 
	patientId
®® 
=
®® 
null
®® !
,
®®! "
int
©© 
?
©© 
doctorId
©© 
=
©© 
null
©©  
)
©©  !
{
™™ 	
var
´´ 
jwtSettings
´´ 
=
´´ 
_configuration
´´ ,
.
´´, -

GetSection
´´- 7
(
´´7 8
$str
´´8 =
)
´´= >
;
´´> ?
var
≠≠ 
key
≠≠ 
=
≠≠ 
new
≠≠ "
SymmetricSecurityKey
≠≠ .
(
≠≠. /
Encoding
ÆÆ 
.
ÆÆ 
UTF8
ÆÆ 
.
ÆÆ 
GetBytes
ÆÆ &
(
ÆÆ& '
jwtSettings
ÆÆ' 2
[
ÆÆ2 3
$str
ÆÆ3 8
]
ÆÆ8 9
!
ÆÆ9 :
)
ÆÆ: ;
)
ØØ 
;
ØØ 
var
±± 
credentials
±± 
=
±± 
new
±± ! 
SigningCredentials
±±" 4
(
±±4 5
key
±±5 8
,
±±8 9 
SecurityAlgorithms
±±: L
.
±±L M

HmacSha256
±±M W
)
±±W X
;
±±X Y
var
≥≥ 
claims
≥≥ 
=
≥≥ 
new
≥≥ 
List
≥≥ !
<
≥≥! "
Claim
≥≥" '
>
≥≥' (
{
¥¥ 
new
µµ 
Claim
µµ 
(
µµ 

ClaimTypes
µµ $
.
µµ$ %
NameIdentifier
µµ% 3
,
µµ3 4
user
µµ5 9
.
µµ9 :
Id
µµ: <
)
µµ< =
,
µµ= >
new
∂∂ 
Claim
∂∂ 
(
∂∂ 

ClaimTypes
∂∂ $
.
∂∂$ %
Email
∂∂% *
,
∂∂* +
user
∂∂, 0
.
∂∂0 1
Email
∂∂1 6
!
∂∂6 7
)
∂∂7 8
,
∂∂8 9
new
∑∑ 
Claim
∑∑ 
(
∑∑ 

ClaimTypes
∑∑ $
.
∑∑$ %
Role
∑∑% )
,
∑∑) *
role
∑∑+ /
)
∑∑/ 0
,
∑∑0 1
new
∏∏ 
Claim
∏∏ 
(
∏∏ %
JwtRegisteredClaimNames
∏∏ 1
.
∏∏1 2
Jti
∏∏2 5
,
∏∏5 6
Guid
∏∏7 ;
.
∏∏; <
NewGuid
∏∏< C
(
∏∏C D
)
∏∏D E
.
∏∏E F
ToString
∏∏F N
(
∏∏N O
)
∏∏O P
)
∏∏P Q
}
ππ 
;
ππ 
if
ºº 
(
ºº 
	patientId
ºº 
.
ºº 
HasValue
ºº "
)
ºº" #
claims
ΩΩ 
.
ΩΩ 
Add
ΩΩ 
(
ΩΩ 
new
ΩΩ 
Claim
ΩΩ $
(
ΩΩ$ %
$str
ΩΩ% 0
,
ΩΩ0 1
	patientId
ΩΩ2 ;
.
ΩΩ; <
Value
ΩΩ< A
.
ΩΩA B
ToString
ΩΩB J
(
ΩΩJ K
)
ΩΩK L
)
ΩΩL M
)
ΩΩM N
;
ΩΩN O
if
øø 
(
øø 
doctorId
øø 
.
øø 
HasValue
øø !
)
øø! "
claims
¿¿ 
.
¿¿ 
Add
¿¿ 
(
¿¿ 
new
¿¿ 
Claim
¿¿ $
(
¿¿$ %
$str
¿¿% /
,
¿¿/ 0
doctorId
¿¿1 9
.
¿¿9 :
Value
¿¿: ?
.
¿¿? @
ToString
¿¿@ H
(
¿¿H I
)
¿¿I J
)
¿¿J K
)
¿¿K L
;
¿¿L M
var
¬¬ 
expiryMinutes
¬¬ 
=
¬¬ 
int
¬¬  #
.
¬¬# $
Parse
¬¬$ )
(
¬¬) *
jwtSettings
¬¬* 5
[
¬¬5 6
$str
¬¬6 T
]
¬¬T U
!
¬¬U V
)
¬¬V W
;
¬¬W X
var
ƒƒ 
token
ƒƒ 
=
ƒƒ 
new
ƒƒ 
JwtSecurityToken
ƒƒ ,
(
ƒƒ, -
issuer
≈≈ 
:
≈≈ 
jwtSettings
≈≈ #
[
≈≈# $
$str
≈≈$ ,
]
≈≈, -
,
≈≈- .
audience
∆∆ 
:
∆∆ 
jwtSettings
∆∆ %
[
∆∆% &
$str
∆∆& 0
]
∆∆0 1
,
∆∆1 2
claims
«« 
:
«« 
claims
«« 
,
«« 
expires
»» 
:
»» 
DateTime
»» !
.
»»! "
UtcNow
»»" (
.
»»( )

AddMinutes
»») 3
(
»»3 4
expiryMinutes
»»4 A
)
»»A B
,
»»B C 
signingCredentials
…… "
:
……" #
credentials
……$ /
)
   
;
   
return
ÃÃ 
new
ÃÃ %
JwtSecurityTokenHandler
ÃÃ .
(
ÃÃ. /
)
ÃÃ/ 0
.
ÃÃ0 1

WriteToken
ÃÃ1 ;
(
ÃÃ; <
token
ÃÃ< A
)
ÃÃA B
;
ÃÃB C
}
ÕÕ 	
}
ŒŒ 
}œœ ˛å
qC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\AppointmentService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
AppointmentService #
:$ %
IAppointmentService& 9
{ 
private 
readonly "
IAppointmentRepository /
_repository0 ;
;; <
private 
readonly 
IDoctorService '
_doctorService( 6
;6 7
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
public 
AppointmentService !
(! ""
IAppointmentRepository" 8

repository9 C
,C D
IDoctorServiceE S
doctorServiceT a
,a b
HealthCareDbContextc v
contextw ~
,~ 
IMapper
Ä á
mapper
à é
)
é è
{ 	
_repository 
= 

repository $
;$ %
_doctorService 
= 
doctorService *
;* +
_context 
= 
context 
; 
_mapper 
= 
mapper 
; 
} 	
public 
async 
Task 
AddAsync "
(" # 
CreateAppointmentDto# 7
dto8 ;
,; <
int< ?
id@ B
)B C
{ 	
var 
appointment 
= 
_mapper %
.% &
Map& )
<) *
Appointment* 5
>5 6
(6 7
dto7 :
): ;
;; <
await   
_repository   
.   
AddAsync   &
(  & '
appointment  ' 2
)  2 3
;  3 4
await!! 
_context!! 
.!! 
SaveChangesAsync!! +
(!!+ ,
)!!, -
;!!- .
}"" 	
public$$ 
async$$ 
Task$$ 
UpdateAsync$$ %
($$% &
int$$& )
id$$* ,
,$$, - 
UpdateAppointmentDto$$. B
dto$$C F
)$$F G
{%% 	
var&& 
appointment&& 
=&& 
await&& #
_repository&&$ /
.&&/ 0
GetByIdAsync&&0 <
(&&< =
id&&= ?
)&&? @
;&&@ A
if'' 
('' 
appointment'' 
=='' 
null'' #
)''# $
throw(( 
new(( (
AppointmentNotFoundException(( 6
(((6 7
id((7 9
)((9 :
;((: ;
_mapper)) 
.)) 
Map)) 
()) 
dto)) 
,)) 
appointment)) (
)))( )
;))) *
await++ 
_repository++ 
.++ 
UpdateAsync++ )
(++) *
appointment++* 5
)++5 6
;++6 7
await,, 
_context,, 
.,, 
SaveChangesAsync,, +
(,,+ ,
),,, -
;,,- .
}-- 	
public// 
async// 
Task// 
DeleteAsync// %
(//% &
int//& )
id//* ,
)//, -
{00 	
var11 
appointment11 
=11 
await11 #
_repository11$ /
.11/ 0
GetByIdAsync110 <
(11< =
id11= ?
)11? @
;11@ A
if22 
(22 
appointment22 
==22 
null22 #
)22# $
throw33 
new33 (
AppointmentNotFoundException33 6
(336 7
id337 9
)339 :
;33: ;
await44 
_repository44 
.44 
DeleteAsync44 )
(44) *
id44* ,
)44, -
;44- .
await55 
_context55 
.55 
SaveChangesAsync55 +
(55+ ,
)55, -
;55- .
}66 	
public88 
async88 
Task88 
<88 
AppointmentListDto88 ,
?88, -
>88- .
GetByIdAsync88/ ;
(88; <
int88< ?
id88@ B
)88B C
{99 	
var:: 
appointment:: 
=:: 
await:: #
_repository::$ /
.::/ 0
GetByIdAsync::0 <
(::< =
id::= ?
)::? @
;::@ A
return;; 
appointment;; 
==;; !
null;;" &
?;;' (
null;;) -
:;;. /
_mapper;;0 7
.;;7 8
Map;;8 ;
<;;; <
AppointmentListDto;;< N
?;;N O
>;;O P
(;;P Q
appointment;;Q \
);;\ ]
;;;] ^
}<< 	
public>> 
async>> 
Task>> 
<>> 
PagedResult>> %
<>>% &
AppointmentListDto>>& 8
>>>8 9
>>>9 :
GetAllAsync>>; F
(>>F G
AppointmentFilter>>G X
filter>>Y _
)>>_ `
{?? 	

ExpressionAA 
<AA 
FuncAA 
<AA 
AppointmentAA '
,AA' (
boolAA) -
>AA- .
>AA. /
?AA/ 0
	predicateAA1 :
=AA; <
nullAA= A
;AAA B
ifCC 
(CC 
!CC 
stringCC 
.CC 
IsNullOrWhiteSpaceCC *
(CC* +
filterCC+ 1
.CC1 2
StatusCC2 8
)CC8 9
&&CC: <
filterCC= C
.CCC D
ScheduledDateCCD Q
.CCQ R
HasValueCCR Z
)CCZ [
{DD 
	predicateEE 
=EE 
aEE 
=>EE  
aFF 
.FF 
StatusFF 
==FF 
filterFF  &
.FF& '
StatusFF' -
&&FF. 0
aGG 
.GG 
ScheduledDateGG #
==GG$ &
filterGG' -
.GG- .
ScheduledDateGG. ;
.GG; <
ValueGG< A
;GGA B
}HH 
elseII 
ifII 
(II 
!II 
stringII 
.II 
IsNullOrWhiteSpaceII /
(II/ 0
filterII0 6
.II6 7
StatusII7 =
)II= >
)II> ?
{JJ 
	predicateKK 
=KK 
aKK 
=>KK  
aKK! "
.KK" #
StatusKK# )
==KK* ,
filterKK- 3
.KK3 4
StatusKK4 :
;KK: ;
}LL 
elseMM 
ifMM 
(MM 
filterMM 
.MM 
ScheduledDateMM )
.MM) *
HasValueMM* 2
)MM2 3
{NN 
	predicateOO 
=OO 
aOO 
=>OO  
aOO! "
.OO" #
ScheduledDateOO# 0
==OO1 3
filterOO4 :
.OO: ;
ScheduledDateOO; H
.OOH I
ValueOOI N
;OON O
}PP 
FuncSS 
<SS 

IQueryableSS 
<SS 
AppointmentSS '
>SS' (
,SS( )
IOrderedQueryableSS* ;
<SS; <
AppointmentSS< G
>SSG H
>SSH I
orderBySSJ Q
=SSR S
qTT 
=>TT 
qTT 
.TT 
OrderByTT 
(TT 
aTT  
=>TT! #
aTT$ %
.TT% &
ScheduledDateTT& 3
)TT3 4
;TT4 5
varWW 
pagedResultWW 
=WW 
awaitWW #
_repositoryWW$ /
.WW/ 0
GetAllAsyncWW0 ;
(WW; <
filterXX 
.XX 

PageNumberXX !
,XX! "
filterYY 
.YY 
PageSizeYY 
,YY  
	predicateZZ 
,ZZ 
orderBy[[ 
)\\ 
;\\ 
return__ 
new__ 
PagedResult__ "
<__" #
AppointmentListDto__# 5
>__5 6
{`` 
Itemsaa 
=aa 
_mapperaa 
.aa  
Mapaa  #
<aa# $
IEnumerableaa$ /
<aa/ 0
AppointmentListDtoaa0 B
>aaB C
>aaC D
(aaD E
pagedResultaaE P
.aaP Q
ItemsaaQ V
)aaV W
,aaW X

PageNumberbb 
=bb 
pagedResultbb (
.bb( )

PageNumberbb) 3
,bb3 4
PageSizecc 
=cc 
pagedResultcc &
.cc& '
PageSizecc' /
,cc/ 0

TotalCountdd 
=dd 
pagedResultdd (
.dd( )

TotalCountdd) 3
}ee 
;ee 
}ff 	
publichh 
asynchh 
Taskhh 
UpdateStatusAsynchh +
(hh+ ,
inthh, /
idhh0 2
,hh2 3 
UpdateAppointmentDtohh4 H
dtohhI L
)hhL M
{ii 	
varjj 
appointmentjj 
=jj 
awaitjj #
_repositoryjj$ /
.jj/ 0
GetByIdAsyncjj0 <
(jj< =
idjj= ?
)jj? @
;jj@ A
ifll 
(ll 
appointmentll 
isll 
nullll #
)ll# $
throwmm 
newmm %
InvalidOperationExceptionmm 3
(mm3 4
$strmm4 H
)mmH I
;mmI J
appointmentoo 
.oo 
Statusoo 
=oo  
dtooo! $
.oo$ %
Statusoo% +
;oo+ ,
appointmentpp 
.pp 
CancellationReasonpp *
=pp+ ,
dtopp- 0
.pp0 1
CancellationReasonpp1 C
;ppC D
awaitrr 
_repositoryrr 
.rr 
UpdateAsyncrr )
(rr) *
appointmentrr* 5
)rr5 6
;rr6 7
awaitss 
_contextss 
.ss 
SaveChangesAsyncss +
(ss+ ,
)ss, -
;ss- .
}tt 	
publicuu 
asyncuu 
Taskuu 
<uu 
Listuu 
<uu 
stringuu %
>uu% &
>uu& '
AvailableTimeSlotsuu( :
(uu: ;
DateOnlyuu; C
dateuuD H
,uuH I
intuuJ M
doctorIduuN V
)uuV W
{vv 	
ifww 
(ww 
dateww 
<ww 
DateOnlyww 
.ww  
FromDateTimeww  ,
(ww, -
DateTimeww- 5
.ww5 6
Todayww6 ;
)ww; <
)ww< =
throwxx 
newxx %
InvalidOperationExceptionxx 3
(xx3 4
$strxx4 `
)xx` a
;xxa b
varzz 
allSlotszz 
=zz 
awaitzz  
_doctorServicezz! /
.zz/ 0
GetSlotszz0 8
(zz8 9
doctorIdzz9 A
)zzA B
;zzB C
var{{ 
bookedSlots{{ 
={{ 
await{{ #
_repository{{$ /
.{{/ 0
AvailableTimeSlots{{0 B
({{B C
date{{C G
,{{G H
doctorId{{I Q
){{Q R
;{{R S
var}} 
	freeSlots}} 
=}} 
allSlots}} $
.}}$ %
Except}}% +
(}}+ ,
bookedSlots}}, 7
)}}7 8
.}}8 9
ToList}}9 ?
(}}? @
)}}@ A
;}}A B
return 
	freeSlots 
; 
}
ÄÄ 	
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
bool
ÇÇ 
>
ÇÇ 
IsAvailable
ÇÇ  +
(
ÇÇ+ ,
DateOnly
ÇÇ, 4
date
ÇÇ5 9
,
ÇÇ9 :
int
ÇÇ; >
doctorId
ÇÇ? G
,
ÇÇG H
string
ÇÇI O
timeSlot
ÇÇP X
)
ÇÇX Y
{
ÉÉ 	
var
ÑÑ 
	available
ÑÑ 
=
ÑÑ 
await
ÑÑ !
_repository
ÑÑ" -
.
ÑÑ- .
IsAvailable
ÑÑ. 9
(
ÑÑ9 :
date
ÑÑ: >
,
ÑÑ> ?
doctorId
ÑÑ@ H
,
ÑÑH I
timeSlot
ÑÑJ R
)
ÑÑR S
;
ÑÑS T
if
ÜÜ 
(
ÜÜ 
!
ÜÜ 
	available
ÜÜ 
)
ÜÜ 
throw
áá 
new
áá '
InvalidOperationException
áá 3
(
áá3 4
$str
áá4 W
)
ááW X
;
ááX Y
return
ââ 
true
ââ 
;
ââ 
}
ää 	
public
åå 
async
åå 
Task
åå 
<
åå 
List
åå 
<
åå "
AppointmentReportDto
åå 3
>
åå3 4
>
åå4 5
GetDailyReport
åå6 D
(
ååD E
)
ååE F
{
çç 	
var
éé 
report
éé 
=
éé 
await
éé 
_repository
éé *
.
éé* +
GetDailyReport
éé+ 9
(
éé9 :
)
éé: ;
;
éé; <
return
èè 
report
èè 
.
èè 
Count
èè 
==
èè  "
$num
èè# $
?
èè% &
new
èè' *
List
èè+ /
<
èè/ 0"
AppointmentReportDto
èè0 D
>
èèD E
(
èèE F
)
èèF G
:
èèH I
report
èèJ P
;
èèP Q
}
êê 	
public
íí 
async
íí 
Task
íí 
<
íí 
List
íí 
<
íí  
AppointmentListDto
íí 1
>
íí1 2
>
íí2 3
GetDoctorSchedule
íí4 E
(
ííE F
DateOnly
ííF N
date
ííO S
,
ííS T
int
ííU X
id
ííY [
)
íí[ \
{
ìì 	
var
îî 
schedule
îî 
=
îî 
await
îî  
_repository
îî! ,
.
îî, -
GetDoctorSchedule
îî- >
(
îî> ?
date
îî? C
,
îîC D
id
îîE G
)
îîG H
;
îîH I
return
ïï 
schedule
ïï 
.
ïï 
Count
ïï !
==
ïï" $
$num
ïï% &
?
ïï' (
new
ïï) ,
List
ïï- 1
<
ïï1 2 
AppointmentListDto
ïï2 D
>
ïïD E
(
ïïE F
)
ïïF G
:
ïïH I
schedule
ïïJ R
;
ïïR S
}
ññ 	
public
òò 
async
òò 
Task
òò 
<
òò 
List
òò 
<
òò  
AppointmentListDto
òò 1
>
òò1 2
>
òò2 3 
GetPatientSchedule
òò4 F
(
òòF G
DateOnly
òòG O
date
òòP T
,
òòT U
int
òòV Y
id
òòZ \
)
òò\ ]
{
ôô 	
var
öö 
schedule
öö 
=
öö 
await
öö  
_repository
öö! ,
.
öö, - 
GetPatientSchedule
öö- ?
(
öö? @
date
öö@ D
,
ööD E
id
ööF H
)
ööH I
;
ööI J
return
õõ 
schedule
õõ 
.
õõ 
Count
õõ !
==
õõ" $
$num
õõ% &
?
õõ' (
new
õõ) ,
List
õõ- 1
<
õõ1 2 
AppointmentListDto
õõ2 D
>
õõD E
(
õõE F
)
õõF G
:
õõH I
schedule
õõJ R
;
õõR S
}
úú 	
public
ûû 
async
ûû 
Task
ûû 
<
ûû 
List
ûû 
<
ûû  
AppointmentListDto
ûû 1
>
ûû1 2
>
ûû2 3%
GetAppointmentByPatient
ûû4 K
(
ûûK L
int
ûûL O
id
ûûP R
)
ûûR S
{
üü 	
var
†† 
appointments
†† 
=
†† 
await
†† $
_repository
††% 0
.
††0 1%
GetAppointmentByPatient
††1 H
(
††H I
id
††I K
)
††K L
;
††L M
return
°° 
appointments
°° 
.
°°  
Count
°°  %
==
°°& (
$num
°°) *
?
°°+ ,
new
°°- 0
List
°°1 5
<
°°5 6 
AppointmentListDto
°°6 H
>
°°H I
(
°°I J
)
°°J K
:
°°L M
appointments
°°N Z
;
°°Z [
}
¢¢ 	
public
§§ 
async
§§ 
Task
§§ 
<
§§ 
List
§§ 
<
§§  
AppointmentListDto
§§ 1
>
§§1 2
>
§§2 3$
GetAppointmentByDoctor
§§4 J
(
§§J K
int
§§K N
id
§§O Q
)
§§Q R
{
•• 	
var
¶¶ 
appointments
¶¶ 
=
¶¶ 
await
¶¶ $
_repository
¶¶% 0
.
¶¶0 1$
GetAppointmentByDoctor
¶¶1 G
(
¶¶G H
id
¶¶H J
)
¶¶J K
;
¶¶K L
return
ßß 
appointments
ßß 
.
ßß  
Count
ßß  %
==
ßß& (
$num
ßß) *
?
ßß+ ,
new
ßß- 0
List
ßß1 5
<
ßß5 6 
AppointmentListDto
ßß6 H
>
ßßH I
(
ßßI J
)
ßßJ K
:
ßßL M
appointments
ßßN Z
;
ßßZ [
}
®® 	
public
™™ 
async
™™ 
Task
™™ ,
CancelAppointmentsByDoctorDate
™™ 8
(
™™8 9
int
™™9 <
doctorId
™™= E
,
™™E F
DateOnly
™™G O
date
™™P T
)
™™T U
{
´´ 	
await
¨¨ 
_repository
¨¨ 
.
¨¨ ,
CancelAppointmentsByDoctorDate
¨¨ <
(
¨¨< =
doctorId
¨¨= E
,
¨¨E F
date
¨¨G K
)
¨¨K L
;
¨¨L M
await
≠≠ 
_context
≠≠ 
.
≠≠ 
SaveChangesAsync
≠≠ +
(
≠≠+ ,
)
≠≠, -
;
≠≠- .
}
ÆÆ 	
}
ØØ 
}∞∞ î
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Interfaces\IRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface 
IRepository  
<  !
T! "
>" #
where$ )
T* +
:, -
class. 3
{ 
Task 
< 
T 
> 
AddAsync 
( 
T 
entity !
,! "
CancellationToken# 4
ct5 7
=8 9
default: A
)A B
;B C
Task		 
UpdateAsync		 
(		 
T		 
entity		 !
,		! "
CancellationToken		# 4
ct		5 7
=		8 9
default		: A
)		A B
;		B C
Task

 
DeleteAsync

 
(

 
int

 
id

 
)

  
;

  !
Task 
< 
T 
> 
GetByIdAsync 
( 
int  
id! #
)# $
;$ %
Task 
< 
PagedResult 
< 
T 
> 
> 
GetAllAsync (
(( )
int 

pageNumber 
, 
int 
pageSize 
, 

Expression 
< 
Func 
< 
T 
, 
bool #
># $
>$ %
?% &
predicte' /
=0 1
null2 6
,6 7
Func 
< 

IQueryable 
< 
T 
> 
, 
IOrderedQueryable  1
<1 2
T2 3
>3 4
>4 5
orderBy6 =
=> ?
null@ D
) 
; 
} 
} •
pC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Interfaces\IPatientRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface 
IPatientRepository '
:( )
IRepository* 5
<5 6
Patient6 =
>= >
{ 
Task 
< 
Patient 
> 
GetByUserIdAsync $
($ %
string% +
userId, 2
)2 3
;3 4
}		 
}

 ‹
uC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Interfaces\IHealthRecordRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface #
IHealthRecordRepository ,
:- .
IRepository/ :
<: ;
HealthRecord; G
>G H
{ 
Task 
< 
List 
< 
HealthRecord 
> 
>  $
GetHealthRecordByPatient! 9
(9 :
int: =
id> @
)@ A
;A B
Task 
< 
List 
< 
HealthRecord 
> 
>  (
GetHealthRecordByAppointment! =
(= >
int> A
idB D
)D E
;E F
}		 
}

 Ç
oC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Interfaces\IDoctorRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface 
IDoctorRepository &
:' (
IRepository) 4
<4 5
Doctor5 ;
>; <
{ 
Task		 
<		 
Doctor		 
?		 
>		 
GetByUserIdAsync		 &
(		& '
string		' -
userId		. 4
)		4 5
;		5 6
Task

 
<

 
List

 
<

 
string

 
>

 
>

 
GetSlots

 #
(

# $
int

$ '
doctorId

( 0
)

0 1
;

1 2
Task 
CreateSlots 
( 
int 
doctorId %
,% &
List' +
<+ ,
string, 2
>2 3
	timeslots4 =
)= >
;> ?
Task 
< 
List 
< 
DoctorLeaves 
> 
>  
GetLeavesByDoctorId! 4
(4 5
int5 8
doctorId9 A
)A B
;B C
Task 
CreateLeaves 
( 
int 
doctorId &
,& '
List( ,
<, -
CreateLeaveDto- ;
>; <
leaves= C
)C D
;D E
Task 
< 
List 
< 
DoctorListDto 
>  
>  !
AvailableDoctors" 2
(2 3
string3 9
specialisation: H
,H I
DateOnlyJ R
dateS W
)W X
;X Y
} 
} ˘
tC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Interfaces\IAppointmentRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface "
IAppointmentRepository +
:, -
IRepository. 9
<9 :
Appointment: E
>E F
{ 
Task		 
<		 
List		 
<		 
string		 
>		 
>		 
AvailableTimeSlots		 -
(		- .
DateOnly		. 6
date		7 ;
,		; <
int		= @
doctorId		A I
)		I J
;		J K
Task

 
<

 
bool

 
>

 
IsAvailable

 
(

 
DateOnly

 '
date

( ,
,

, -
int

. 1
doctorId

2 :
,

: ;
string

< B
timeSlot

C K
)

K L
;

L M
Task 
< 
List 
<  
AppointmentReportDto &
>& '
>' (
GetDailyReport) 7
(7 8
)8 9
;9 :
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &
GetDoctorSchedule' 8
(8 9
DateOnly9 A
dateB F
,F G
intH K
idL N
)N O
;O P
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &
GetPatientSchedule' 9
(9 :
DateOnly: B
dateC G
,G H
intI L
idM O
)O P
;P Q
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &#
GetAppointmentByPatient' >
(> ?
int? B
idC E
)E F
;F G
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &"
GetAppointmentByDoctor' =
(= >
int> A
idB D
)D E
;E F
Task *
CancelAppointmentsByDoctorDate +
(+ ,
int, /
doctorId0 8
,8 9
DateOnly: B
dateC G
)G H
;H I
} 
} ‚,
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Implementations\Repository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &
Implementations& 5
{ 
public		 

class		 

Repository		 
<		 
T		 
>		 
:		  
IRepository		! ,
<		, -
T		- .
>		. /
where		0 5
T		6 7
:		8 9
class		: ?
{

 
	protected 
readonly 
HealthCareDbContext .
_context/ 7
;7 8
	protected 
readonly 
DbSet  
<  !
T! "
>" #
_dbSet$ *
;* +
public 

Repository 
( 
HealthCareDbContext -
context. 5
)5 6
{ 	
_context 
= 
context 
; 
_dbSet 
= 
context 
. 
Set  
<  !
T! "
>" #
(# $
)$ %
;% &
} 	
public 
async 
Task 
< 
T 
> 
AddAsync %
(% &
T& '
entity( .
,. /
CancellationToken/ @
ctA C
=C D
defaultD K
)K L
{ 	
await 
_dbSet 
. 
AddAsync !
(! "
entity" (
,( )
ct) +
)+ ,
;, -
return 
entity 
; 
} 	
public 
Task 
UpdateAsync 
(  
T! "
entity# )
,) *
CancellationToken* ;
ct< >
=> ?
default? F
)F G
{ 	
_dbSet 
. 
Update 
( 
entity  
)  !
;! "
return 
Task 
. 
CompletedTask %
;% &
} 	
public!! 
async!! 
Task!! 
DeleteAsync!! %
(!!% &
int!!& )
id!!* ,
)!!, -
{"" 	
var## 
entity## 
=## 
await## 
_dbSet## %
.##% &
	FindAsync##& /
(##/ 0
id##0 2
)##2 3
;##3 4
if$$ 
($$ 
entity$$ 
is$$ 
not$$ 
null$$ "
)$$" #
_dbSet%% 
.%% 
Remove%% 
(%% 
entity%% $
)%%$ %
;%%% &
}&& 	
public(( 
async(( 
Task(( 
<(( 
T(( 
>(( 
GetByIdAsync(( )
((() *
int((* -
id((. 0
)((0 1
=>((1 3
await)) 
_dbSet)) 
.)) 
	FindAsync)) #
())# $
id))$ &
)))& '
;))' (
public++ 
async++ 
Task++ 
<++ 
PagedResult++ %
<++% &
T++& '
>++' (
>++( )
GetAllAsync++* 5
(++5 6
int,, 

pageNumber,, 
,,, 
int-- 
pageSize-- 
,-- 

Expression.. 
<.. 
Func.. 
<.. 
T.. 
,.. 
bool..  $
>..$ %
>..% &
	predicate..' 0
=..1 2
null..3 7
,..7 8
Func// 
<// 

IQueryable// 
<// 
T// 
>// 
,//  
IOrderedQueryable//! 2
<//2 3
T//3 4
>//4 5
>//5 6
orderBy//7 >
=//? @
null//A E
)//E F
{00 	

IQueryable11 
<11 
T11 
>11 
query11 
=11  !
_dbSet11" (
;11( )
if33 
(33 
	predicate33 
!=33 
null33 !
)33! "
query44 
=44 
query44 
.44 
Where44 #
(44# $
	predicate44$ -
)44- .
;44. /
if66 
(66 
orderBy66 
!=66 
null66 
)66  
query77 
=77 
orderBy77 
(77  
query77  %
)77% &
;77& '
var99 

totalCount99 
=99 
await99 "
query99# (
.99( )

CountAsync99) 3
(993 4
)994 5
;995 6
var;; 
items;; 
=;; 
await;; 
query;; #
.<< 
Skip<< 
(<< 
(<< 

pageNumber<< !
-<<" #
$num<<$ %
)<<% &
*<<' (
pageSize<<) 1
)<<1 2
.== 
Take== 
(== 
pageSize== 
)== 
.>> 
ToListAsync>> 
(>> 
)>> 
;>> 
return@@ 
new@@ 
PagedResult@@ "
<@@" #
T@@# $
>@@$ %
{AA 
ItemsBB 
=BB 
itemsBB 
,BB 

PageNumberCC 
=CC 

pageNumberCC '
,CC' (
PageSizeDD 
=DD 
pageSizeDD #
,DD# $

TotalCountEE 
=EE 

totalCountEE '
}FF 
;FF 
}GG 	
}JJ 
}KK …

tC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Implementations\PatientRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &
Implementations& 5
{ 
public 

class 
PatientRepository "
:# $

Repository% /
</ 0
Patient0 7
>7 8
,8 9
IPatientRepository9 K
{		 
public 
PatientRepository  
(  !
HealthCareDbContext! 4
context5 <
)< =
:> ?
base@ D
(D E
contextE L
)L M
{N O
}P Q
public 
async 
Task 
< 
Patient !
?! "
>" #
GetByUserIdAsync$ 4
(4 5
string5 ;
userId< B
)B C
{ 	
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
UserId, 2
==3 5
userId6 <
)< =
;= >
} 	
} 
} …
yC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Implementations\HealthRecordRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &
Implementations& 5
{ 
public 

class "
HealthRecordRepository '
:( )

Repository* 4
<4 5
HealthRecord5 A
>A B
,B C#
IHealthRecordRepositoryC Z
{		 
public

 "
HealthRecordRepository

 %
(

% &
HealthCareDbContext

& 9
context

: A
)

A B
:

C D
base

E I
(

I J
context

J Q
)

Q R
{

S T
}

U V
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -$
GetHealthRecordByPatient. F
(F G
intG J
idK M
)M N
=>O Q
await 
_dbSet 
. 
Where 
( 
hr 
=> 
hr 
. 
	PatientId (
==) +
id, .
). /
. 
ToListAsync 
( 
) 
; 
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -(
GetHealthRecordByAppointment. J
(J K
intK N
idO Q
)Q R
=>S U
await 
_dbSet 
. 
Where 
( 
hr 
=> 
hr 
.  
AppointmentId  -
==. 0
id1 3
)3 4
. 
ToListAsync 
( 
) 
; 
} 
} Æ7
sC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Implementations\DoctorRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &
Implementations& 5
{ 
public		 

class		 
DoctorRepository		 !
:		" #

Repository		$ .
<		. /
Doctor		/ 5
>		5 6
,		6 7
IDoctorRepository		8 I
{

 
public 
DoctorRepository 
(  
HealthCareDbContext  3
context4 ;
); <
:= >
base? C
(C D
contextD K
)K L
{M N
}O P
public 
async 
Task 
< 
Doctor  
?  !
>! "
GetByUserIdAsync# 3
(3 4
string4 :
userId; A
)A B
{ 	
return 
await 
_context !
.! "
Doctors" )
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
UserId, 2
==3 5
userId6 <
)< =
;= >
} 	
public 
async 
Task 
CreateSlots %
(% &
int& )
doctorId* 2
,2 3
List4 8
<8 9
string9 ?
>? @
	timeslotsA J
)J K
{ 	
var 
slots 
= 
	timeslots !
.! "
Select" (
(( )
t) *
=>+ -
new. 1
AvailableSlots2 @
{ 
DoctorId 
= 
doctorId #
,# $
TimeSlot 
= 
t 
} 
) 
; 
await 
_context 
. 
AvailableSlots )
.) *
AddRangeAsync* 7
(7 8
slots8 =
)= >
;> ?
} 	
public 
async 
Task 
< 
List 
< 
string %
>% &
>& '
GetSlots( 0
(0 1
int1 4
doctorId5 =
)= >
=>? A
await 
_context 
. 
AvailableSlots )
.   
Where   
(   
s   
=>   
s   
.   
DoctorId   &
==  ' )
doctorId  * 2
)  2 3
.!! 
Select!! 
(!! 
s!! 
=>!! 
s!! 
.!! 
TimeSlot!! '
)!!' (
."" 
ToListAsync"" 
("" 
)"" 
;"" 
public$$ 
async$$ 
Task$$ 
<$$ 
List$$ 
<$$ 
DoctorLeaves$$ +
>$$+ ,
>$$, -
GetLeavesByDoctorId$$. A
($$A B
int$$B E
doctorId$$F N
)$$N O
=>$$P R
await%% 
_context%% 
.%% 
DoctorLeaves%% '
.&& 
Where&& 
(&& 
l&& 
=>&& 
l&& 
.&& 
DoctorId&& &
==&&' )
doctorId&&* 2
)&&2 3
.'' 
ToListAsync'' 
('' 
)'' 
;'' 
public)) 
async)) 
Task)) 
CreateLeaves)) &
())& '
int))' *
doctorId))+ 3
,))3 4
List))5 9
<))9 :
CreateLeaveDto)): H
>))H I
leaves))J P
)))P Q
{** 	
var++ 
entities++ 
=++ 
leaves++ !
.++! "
Select++" (
(++( )
l++) *
=>+++ -
new++. 1
DoctorLeaves++2 >
{,, 
DoctorId-- 
=-- 
doctorId-- #
,--# $
	LeaveDate.. 
=.. 
l.. 
... 
	LeaveDate.. '
,..' (
Reason// 
=// 
l// 
.// 
Reason// !
}00 
)00 
;00 
await22 
_context22 
.22 
DoctorLeaves22 '
.22' (
AddRangeAsync22( 5
(225 6
entities226 >
)22> ?
;22? @
}33 	
public44 
async44 
Task44 
<44 
List44 
<44 
DoctorListDto44 ,
>44, -
>44- .
AvailableDoctors44/ ?
(44? @
string44@ F
specialisation44G U
,44U V
DateOnly44W _
date44` d
)44d e
{55 	
return66 
await66 
_dbSet66 
.77 
Where77 
(77 
d77 
=>77 
d77 
.77 
Specialisation77 ,
==77- /
specialisation770 >
&&77? A
d88 
.88 
IsActive88 &
&&88' )
!99 
d99 
.99 
DoctorLeaves99 +
.99+ ,
Any99, /
(99/ 0
l990 1
=>992 4
l995 6
.996 7
	LeaveDate997 @
==99A C
date99D H
)99H I
)99I J
.;; 
Where;; 
(;; 
d;; 
=>;; 
d<< 
.<< 
AvailableSlots<< $
.<<$ %
Any<<% (
(<<( )
slot<<) -
=><<. 0
!== 
d== 
.== 
Appointments== '
.==' (
Any==( +
(==+ ,
a==, -
=>==. 0
a>> 
.>> 
TimeSlot>> &
==>>' )
slot>>* .
.>>. /
TimeSlot>>/ 7
&&>>8 :
a?? 
.?? 
ScheduledDate?? +
==??, .
date??/ 3
&&??4 6
a@@ 
.@@ 
Status@@ $
!=@@% '
$str@@( 3
)AA 
)BB 
)CC 
.EE 
SelectEE 
(EE 
dEE 
=>EE 
newEE  
DoctorListDtoEE! .
{FF 
DoctorIdGG 
=GG 
dGG  
.GG  !
DoctorIdGG! )
,GG) *
FullNameHH 
=HH 
dHH  
.HH  !
FullNameHH! )
,HH) *
SpecialisationII "
=II# $
dII% &
.II& '
SpecialisationII' 5
,II5 6
ConsultationFeeJJ #
=JJ$ %
dJJ& '
.JJ' (
ConsultationFeeJJ( 7
,JJ7 8
IsActiveKK 
=KK 
dKK  
.KK  !
IsActiveKK! )
}LL 
)LL 
.MM 
ToListAsyncMM 
(MM 
)MM 
;MM 
}NN 	
}OO 
}RR ¬b
xC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Implementations\AppointmentRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &
Implementations& 5
{ 
public		 

class		 !
AppointmentRepository		 &
:		' (

Repository		) 3
<		3 4
Appointment		4 ?
>		? @
,		@ A"
IAppointmentRepository		A W
{

 
public !
AppointmentRepository $
($ %
HealthCareDbContext% 8
context9 @
)@ A
:B C
baseD H
(H I
contextI P
)P Q
{R S
}T U
public 
async 
Task 
< 
List 
< 
string %
>% &
>& '
AvailableTimeSlots( :
(: ;
DateOnly; C
dateD H
,H I
intJ M
doctorIdN V
)V W
=>X Z
await 
_dbSet 
. 
Where 
( 
a 
=> 
a 
. 
ScheduledDate +
==, .
date/ 3
&& 
a 
. 
DoctorId &
==' )
doctorId* 2
&& 
a 
. 
Status $
!=% '
$str( 3
)3 4
. 
Select 
( 
a 
=> 
a 
. 
TimeSlot '
)' (
. 
ToListAsync 
( 
) 
; 
public 
async 
Task 
< 
bool 
> 
IsAvailable  +
(+ ,
DateOnly, 4
date5 9
,9 :
int; >
doctorId? G
,G H
stringI O
timeSlotP X
)X Y
{ 	
var 
exists 
= 
await 
_dbSet %
.% &
AnyAsync& .
(. /
a/ 0
=>1 3
a 
. 
ScheduledDate 
==  "
date# '
&& 
a 
. 
DoctorId 
==  
doctorId! )
&& 
a 
. 
TimeSlot 
==  
timeSlot! )
&& 
a 
. 
Status 
!= 
$str *
)* +
;+ ,
return 
! 
exists 
; 
} 	
public!! 
async!! 
Task!! 
<!! 
List!! 
<!!  
AppointmentReportDto!! 3
>!!3 4
>!!4 5
GetDailyReport!!6 D
(!!D E
)!!E F
=>!!G I
await"" 
_dbSet"" 
.## 
GroupBy## 
(## 
a## 
=>## 
a## 
.##  
ScheduledDate##  -
)##- .
.$$ 
Select$$ 
($$ 
g$$ 
=>$$ 
new$$   
AppointmentReportDto$$! 5
{%% 
Date&& 
=&& 
g&& 
.&& 
Key&&  
,&&  !
PendingCount''  
=''! "
g''# $
.''$ %
Count''% *
(''* +
a''+ ,
=>''- /
a''0 1
.''1 2
Status''2 8
==''9 ;
$str''< E
)''E F
,''F G
ConfirmedCount(( "
=((# $
g((% &
.((& '
Count((' ,
(((, -
a((- .
=>((/ 1
a((2 3
.((3 4
Status((4 :
==((; =
$str((> I
)((I J
,((J K
CancelledCount)) "
=))# $
g))% &
.))& '
Count))' ,
()), -
a))- .
=>))/ 1
a))2 3
.))3 4
Status))4 :
==)); =
$str))> I
)))I J
,))J K
CompletedCount** "
=**# $
g**% &
.**& '
Count**' ,
(**, -
a**- .
=>**/ 1
a**2 3
.**3 4
Status**4 :
==**; =
$str**> I
)**I J
}++ 
)++ 
.,, 
OrderBy,, 
(,, 
r,, 
=>,, 
r,, 
.,,  
Date,,  $
),,$ %
.-- 
ToListAsync-- 
(-- 
)-- 
;-- 
public// 
async// 
Task// 
<// 
List// 
<// 
AppointmentListDto// 1
>//1 2
>//2 3
GetDoctorSchedule//4 E
(//E F
DateOnly//F N
date//O S
,//S T
int//U X
id//Y [
)//[ \
=>//] _
await00 
_dbSet00 
.11 
Where11 
(11 
a11 
=>11 
a11 
.11 
ScheduledDate11 +
==11, .
date11/ 3
&&114 6
a117 8
.118 9
DoctorId119 A
==11B D
id11E G
)11G H
.22 
Select22 
(22 
a22 
=>22 
new22  
AppointmentListDto22! 3
{33 
AppointmentId44 !
=44" #
a44$ %
.44% &
AppointmentId44& 3
,443 4
PatientName55 
=55  !
a55" #
.55# $
Patient55$ +
.55+ ,
FullName55, 4
,554 5

DoctorName66 
=66  
a66! "
.66" #
Doctor66# )
.66) *
FullName66* 2
,662 3
ScheduledDate77 !
=77" #
a77$ %
.77% &
ScheduledDate77& 3
,773 4
TimeSlot88 
=88 
a88  
.88  !
TimeSlot88! )
,88) *
Status99 
=99 
a99 
.99 
Status99 %
}:: 
):: 
.;; 
ToListAsync;; 
(;; 
);; 
;;; 
public== 
async== 
Task== 
<== 
List== 
<== 
AppointmentListDto== 1
>==1 2
>==2 3
GetPatientSchedule==4 F
(==F G
DateOnly==G O
date==P T
,==T U
int==V Y
id==Z \
)==\ ]
=>==^ `
await>> 
_dbSet>> 
.?? 
Where?? 
(?? 
a?? 
=>?? 
a?? 
.?? 
ScheduledDate?? +
==??, .
date??/ 3
&&??4 6
a??7 8
.??8 9
	PatientId??9 B
==??C E
id??F H
)??H I
.@@ 
Select@@ 
(@@ 
a@@ 
=>@@ 
new@@  
AppointmentListDto@@! 3
{AA 
AppointmentIdBB !
=BB" #
aBB$ %
.BB% &
AppointmentIdBB& 3
,BB3 4
PatientNameCC 
=CC  !
aCC" #
.CC# $
PatientCC$ +
.CC+ ,
FullNameCC, 4
,CC4 5

DoctorNameDD 
=DD  
aDD! "
.DD" #
DoctorDD# )
.DD) *
FullNameDD* 2
,DD2 3
ScheduledDateEE !
=EE" #
aEE$ %
.EE% &
ScheduledDateEE& 3
,EE3 4
TimeSlotFF 
=FF 
aFF  
.FF  !
TimeSlotFF! )
,FF) *
StatusGG 
=GG 
aGG 
.GG 
StatusGG %
}HH 
)HH 
.II 
ToListAsyncII 
(II 
)II 
;II 
publicKK 
asyncKK 
TaskKK 
<KK 
ListKK 
<KK 
AppointmentListDtoKK 1
>KK1 2
>KK2 3#
GetAppointmentByPatientKK4 K
(KKK L
intKKL O
idKKP R
)KKR S
=>KKT V
awaitLL 
_dbSetLL 
.MM 
WhereMM 
(MM 
aMM 
=>MM 
aMM 
.MM 
	PatientIdMM '
==MM( *
idMM+ -
&&MM. 0
aMM1 2
.MM2 3
ScheduledDateMM3 @
>=MMA C
DateOnlyMMD L
.MML M
FromDateTimeMMM Y
(MMY Z
DateTimeMMZ b
.MMb c
TodayMMc h
)MMh i
)MMi j
.NN 
SelectNN 
(NN 
aNN 
=>NN 
newNN  
AppointmentListDtoNN! 3
{OO 
AppointmentIdPP !
=PP" #
aPP$ %
.PP% &
AppointmentIdPP& 3
,PP3 4
PatientNameQQ 
=QQ  !
aQQ" #
.QQ# $
PatientQQ$ +
.QQ+ ,
FullNameQQ, 4
,QQ4 5

DoctorNameRR 
=RR  
aRR! "
.RR" #
DoctorRR# )
.RR) *
FullNameRR* 2
,RR2 3
ScheduledDateSS !
=SS" #
aSS$ %
.SS% &
ScheduledDateSS& 3
,SS3 4
TimeSlotTT 
=TT 
aTT  
.TT  !
TimeSlotTT! )
,TT) *
StatusUU 
=UU 
aUU 
.UU 
StatusUU %
}VV 
)VV 
.WW 
ToListAsyncWW 
(WW 
)WW 
;WW 
publicYY 
asyncYY 
TaskYY 
<YY 
ListYY 
<YY 
AppointmentListDtoYY 1
>YY1 2
>YY2 3"
GetAppointmentByDoctorYY4 J
(YYJ K
intYYK N
idYYO Q
)YYQ R
=>YYS U
awaitZZ 
_dbSetZZ 
.[[ 
Where[[ 
([[ 
a[[ 
=>[[ 
a[[ 
.[[ 
DoctorId[[ &
==[[' )
id[[* ,
&&[[- /
a[[0 1
.[[1 2
ScheduledDate[[2 ?
>=[[@ B
DateOnly[[C K
.[[K L
FromDateTime[[L X
([[X Y
DateTime[[Y a
.[[a b
Today[[b g
)[[g h
)[[h i
.\\ 
Select\\ 
(\\ 
a\\ 
=>\\ 
new\\  
AppointmentListDto\\! 3
{]] 
AppointmentId^^ !
=^^" #
a^^$ %
.^^% &
AppointmentId^^& 3
,^^3 4
PatientName__ 
=__  !
a__" #
.__# $
Patient__$ +
.__+ ,
FullName__, 4
,__4 5

DoctorName`` 
=``  
a``! "
.``" #
Doctor``# )
.``) *
FullName``* 2
,``2 3
ScheduledDateaa !
=aa" #
aaa$ %
.aa% &
ScheduledDateaa& 3
,aa3 4
TimeSlotbb 
=bb 
abb  
.bb  !
TimeSlotbb! )
,bb) *
Statuscc 
=cc 
acc 
.cc 
Statuscc %
}dd 
)dd 
.ee 
ToListAsyncee 
(ee 
)ee 
;ee 
publicgg 
asyncgg 
Taskgg *
CancelAppointmentsByDoctorDategg 8
(gg8 9
intgg9 <
doctorIdgg= E
,ggE F
DateOnlyggG O
dateggP T
)ggT U
{hh 	
varii 
appointmentsii 
=ii 
awaitii $
_dbSetii% +
.jj 
Wherejj 
(jj 
ajj 
=>jj 
ajj 
.jj 
DoctorIdjj &
==jj' )
doctorIdjj* 2
&&kk 
akk 
.kk 
ScheduledDatekk +
==kk, .
datekk/ 3
&&ll 
all 
.ll 
Statusll $
!=ll% '
$strll( 3
)ll3 4
.mm 
ToListAsyncmm 
(mm 
)mm 
;mm 
foreachoo 
(oo 
varoo 
appointmentoo $
inoo% '
appointmentsoo( 4
)oo4 5
{pp 
appointmentqq 
.qq 
Statusqq "
=qq# $
$strqq% 0
;qq0 1
appointmentrr 
.rr 
CancellationReasonrr .
=rr/ 0
$strrr1 B
;rrB C
}ss 
}tt 	
}vv 
}ww ê`
MC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Program.cs
public 
partial 
class 
Program 
{ 
private 
static 
async 
Task 
Main "
(" #
string# )
[) *
]* +
args, 0
)0 1
{ 
var 
builder 
= 
WebApplication $
.$ %
CreateBuilder% 2
(2 3
args3 7
)7 8
;8 9
builder 
. 
Services 
. 
AddAutoMapper &
(& '
cfg' *
=>+ -
{ 	
cfg 
. 

AddProfile 
< 
MappingProfile )
>) *
(* +
)+ ,
;, -
} 	
)	 

;
 
builder 
. 
Services 
. 
AddSwaggerGen &
(& '
options' .
=>/ 1
{   	
options!! 
.!! 

SwaggerDoc!! 
(!! 
$str!! #
,!!# $
new!!% (
OpenApiInfo!!) 4
{"" 
Title## 
=## 
$str## '
,##' (
Version$$ 
=$$ 
$str$$ 
}%% 
)%% 
;%% 
options'' 
.'' !
AddSecurityDefinition'' )
('') *
$str''* 2
,''2 3
new''4 7!
OpenApiSecurityScheme''8 M
{(( 
Type)) 
=)) 
SecuritySchemeType)) )
.))) *
Http))* .
,)). /
Scheme** 
=** 
$str** !
,**! "
BearerFormat++ 
=++ 
$str++ $
,++$ %
In,, 
=,, 
ParameterLocation,, &
.,,& '
Header,,' -
,,,- .
Description-- 
=-- 
$str-- I
}.. 
).. 
;.. 
options00 
.00 "
AddSecurityRequirement00 *
(00* +
document00+ 3
=>004 6
new007 :&
OpenApiSecurityRequirement00; U
{11 
[22 
new22 *
OpenApiSecuritySchemeReference22 3
(223 4
$str224 <
,22< =
document22> F
)22F G
]22G H
=22I J
[22K L
]22L M
}33 
)33 
;33 
}44 	
)44	 

;44
 
builder88 
.88 
Services88 
.88 
AddProblemDetails88 *
(88* +
)88+ ,
;88, -
builder99 
.99 
Services99 
.99 
AddExceptionHandler99 ,
<99, -"
GlobalExceptionHandler99- C
>99C D
(99D E
)99E F
;99F G
builder:: 
.:: 
Services:: 
.:: 
AddControllers:: '
(::' (
)::( )
;::) *
builder== 
.== 
Services== 
.== 
AddDbContext== %
<==% &
HealthCareDbContext==& 9
>==9 :
(==: ;
options==; B
=>==C E
options>> 
.>> 
UseSqlServer>>  
(>>  !
builder>>! (
.>>( )
Configuration>>) 6
.>>6 7
GetConnectionString>>7 J
(>>J K
$str>>K S
)>>S T
)>>T U
)?? 	
;??	 

builder@@ 
.@@ 
Services@@ 
.@@ 
AddIdentity@@ $
<@@$ %
IdentityUser@@% 1
,@@1 2
IdentityRole@@3 ?
>@@? @
(@@@ A
options@@A H
=>@@I K
{AA 	
optionsBB 
.BB 
UserBB 
.BB 
RequireUniqueEmailBB +
=BB, -
trueBB. 2
;BB2 3
optionsCC 
.CC 
PasswordCC 
.CC 
RequireDigitCC )
=CC* +
trueCC, 0
;CC0 1
optionsDD 
.DD 
PasswordDD 
.DD 
RequireUppercaseDD -
=DD. /
trueDD0 4
;DD4 5
optionsEE 
.EE 
PasswordEE 
.EE "
RequireNonAlphanumericEE 3
=EE4 5
trueEE6 :
;EE: ;
optionsFF 
.FF 
PasswordFF 
.FF 
RequiredLengthFF +
=FF, -
$numFF. /
;FF/ 0
}GG 	
)GG	 

.HH $
AddEntityFrameworkStoresHH %
<HH% &
HealthCareDbContextHH& 9
>HH9 :
(HH: ;
)HH; <
.HH< =$
AddDefaultTokenProvidersHH= U
(HHU V
)HHV W
;HHW X
builderKK 
.KK 
ServicesKK 
.KK 
AddAuthenticationKK *
(KK* +
JwtBearerDefaultsKK+ <
.KK< = 
AuthenticationSchemeKK= Q
)KKQ R
.LL 	
AddJwtBearerLL	 
(LL 
optionsLL 
=>LL  
{MM 	
varNN 
jwtNN 
=NN 
builderNN 
.NN 
ConfigurationNN +
.NN+ ,

GetSectionNN, 6
(NN6 7
$strNN7 <
)NN< =
;NN= >
optionsPP 
.PP %
TokenValidationParametersPP -
=PP. /
newPP0 3%
TokenValidationParametersPP4 M
{QQ 
ValidateIssuerRR 
=RR  
trueRR! %
,RR% &
ValidIssuerSS 
=SS 
jwtSS !
[SS! "
$strSS" *
]SS* +
,SS+ ,
ValidateAudienceTT  
=TT! "
trueTT# '
,TT' (
ValidAudienceUU 
=UU 
jwtUU  #
[UU# $
$strUU$ .
]UU. /
,UU/ 0
ValidateLifetimeVV  
=VV! "
trueVV# '
,VV' ($
ValidateIssuerSigningKeyWW (
=WW) *
trueWW+ /
,WW/ 0
IssuerSigningKeyXX  
=XX! "
newXX# & 
SymmetricSecurityKeyXX' ;
(XX; <
EncodingYY 
.YY 
UTF8YY !
.YY! "
GetBytesYY" *
(YY* +
jwtYY+ .
[YY. /
$strYY/ 4
]YY4 5
!YY5 6
)YY6 7
)ZZ 
,ZZ 
RoleClaimType[[ 
=[[ 

ClaimTypes[[  *
.[[* +
Role[[+ /
,[[/ 0
	ClockSkew\\ 
=\\ 
TimeSpan\\ $
.\\$ %
Zero\\% )
}]] 
;]] 
}^^ 	
)^^	 

;^^
 
builder`` 
.`` 
Services`` 
.`` 
	AddScoped`` "
(``" #
typeof``# )
(``) *
IRepository``* 5
<``5 6
>``6 7
)``7 8
,``8 9
typeof``: @
(``@ A

Repository``A K
<``K L
>``L M
)``M N
)``N O
;``O P
builderaa 
.aa 
Servicesaa 
.aa 
	AddScopedaa "
<aa" #
IPatientRepositoryaa# 5
,aa5 6
PatientRepositoryaa7 H
>aaH I
(aaI J
)aaJ K
;aaK L
builderbb 
.bb 
Servicesbb 
.bb 
	AddScopedbb "
<bb" #
IDoctorRepositorybb# 4
,bb4 5
DoctorRepositorybb6 F
>bbF G
(bbG H
)bbH I
;bbI J
buildercc 
.cc 
Servicescc 
.cc 
	AddScopedcc "
<cc" #"
IAppointmentRepositorycc# 9
,cc9 :!
AppointmentRepositorycc; P
>ccP Q
(ccQ R
)ccR S
;ccS T
builderdd 
.dd 
Servicesdd 
.dd 
	AddScopeddd "
<dd" ##
IHealthRecordRepositorydd# :
,dd: ;"
HealthRecordRepositorydd< R
>ddR S
(ddS T
)ddT U
;ddU V
builderff 
.ff 
Servicesff 
.ff 
	AddScopedff "
<ff" #
IPatientServiceff# 2
,ff2 3
PatientServiceff4 B
>ffB C
(ffC D
)ffD E
;ffE F
buildergg 
.gg 
Servicesgg 
.gg 
	AddScopedgg "
<gg" #
IDoctorServicegg# 1
,gg1 2
DoctorServicegg3 @
>gg@ A
(ggA B
)ggB C
;ggC D
builderhh 
.hh 
Serviceshh 
.hh 
	AddScopedhh "
<hh" #
IAppointmentServicehh# 6
,hh6 7
AppointmentServicehh8 J
>hhJ K
(hhK L
)hhL M
;hhM N
builderii 
.ii 
Servicesii 
.ii 
	AddScopedii "
<ii" # 
IHealthRecordServiceii# 7
,ii7 8
HealthRecordServiceii9 L
>iiL M
(iiM N
)iiN O
;iiO P
builderjj 
.jj 
Servicesjj 
.jj #
AddEndpointsApiExplorerjj 0
(jj0 1
)jj1 2
;jj2 3
builderkk 
.kk 
Serviceskk 
.kk 
AddSwaggerGenkk &
(kk& '
)kk' (
;kk( )
builderll 
.ll 
Servicesll 
.ll 
	AddScopedll "
<ll" #
IAuthServicell# /
,ll/ 0
AuthServicell1 <
>ll< =
(ll= >
)ll> ?
;ll? @
varnn 
appnn 
=nn 
buildernn 
.nn 
Buildnn 
(nn  
)nn  !
;nn! "
appoo 
.oo 
UseExceptionHandleroo 
(oo  
)oo  !
;oo! "
usingqq 
(qq 
varqq 
scopeqq 
=qq 
appqq 
.qq 
Servicesqq '
.qq' (
CreateScopeqq( 3
(qq3 4
)qq4 5
)qq5 6
{rr 	
varss 
servicesss 
=ss 
scopess  
.ss  !
ServiceProviderss! 0
;ss0 1
vartt 
userManagertt 
=tt 
servicestt &
.tt& '
GetRequiredServicett' 9
<tt9 :
UserManagertt: E
<ttE F
IdentityUserttF R
>ttR S
>ttS T
(ttT U
)ttU V
;ttV W
varuu 
roleManageruu 
=uu 
scopeuu #
.uu# $
ServiceProvideruu$ 3
.uu3 4
GetRequiredServiceuu4 F
<uuF G
RoleManageruuG R
<uuR S
IdentityRoleuuS _
>uu_ `
>uu` a
(uua b
)uub c
;uuc d
awaitvv 

RoleSeedervv 
.vv 
SeedRoleAsyncvv *
(vv* +
roleManagervv+ 6
)vv6 7
;vv7 8
awaitww 
AdminSeederww 
.ww 
SeedAdminAsyncww ,
(ww, -
userManagerww- 8
,ww8 9
roleManagerww: E
)wwE F
;wwF G
}xx 	
ifzz 

(zz 
appzz 
.zz 
Environmentzz 
.zz 
IsDevelopmentzz )
(zz) *
)zz* +
)zz+ ,
{{{ 	
app}} 
.}} 

UseSwagger}} 
(}} 
)}} 
;}} 
app~~ 
.~~ 
UseSwaggerUI~~ 
(~~ 
)~~ 
;~~ 
}
ÄÄ 	
app
ÇÇ 
.
ÇÇ !
UseHttpsRedirection
ÇÇ 
(
ÇÇ  
)
ÇÇ  !
;
ÇÇ! "
app
ÑÑ 
.
ÑÑ 
UseAuthentication
ÑÑ 
(
ÑÑ 
)
ÑÑ 
;
ÑÑ  
app
ÜÜ 
.
ÜÜ 
UseAuthorization
ÜÜ 
(
ÜÜ 
)
ÜÜ 
;
ÜÜ 
app
àà 
.
àà 
MapControllers
àà 
(
àà 
)
àà 
;
àà 
app
ää 
.
ää 
Run
ää 
(
ää 
)
ää 
;
ää 
}
ãã 
}åå ›
QC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\User.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
public		 

class		 
User		 
:		 
IdentityUser		 $
{

 
} 
} È
TC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\Patient.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
public 

class 
Patient 
{ 
public 
string 
? 
UserId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public

 
string

 
FullName

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

+ ,
=

- .
null

/ 3
!

3 4
;

4 5
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
public 
string 
? 
PhoneNumber "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
? 
InsuranceId "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
true- 1
;1 2
public 
User 
? 
User 
{ 
get 
;  
set! $
;$ %
}& '
[ 	

ForeignKey	 
( 
nameof 
( 
UserId !
)! "
)" #
]# $
public 
ICollection 
< 
Appointment &
>& '
Appointments( 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
=C D
[E F
]F G
;G H
public 
ICollection 
< 
HealthRecord '
>' (
HealthRecords) 6
{7 8
get9 <
;< =
set> A
;A B
}C D
=E F
[G H
]H I
;I J
} 
} ∫
YC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\HealthRecord.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
public 

class 
HealthRecord 
{ 
[		 	
Key			 
]		 
public

 
int

 
RecordId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

( )
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
] 
public 
DateTime 
	VisitDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
	Diagnosis  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
Prescription #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
Notes 
{ 
get "
;" #
set$ '
;' (
}) *
public   
DateTimeOffset   
CreatedDate   )
{  * +
get  , /
;  / 0
set  1 4
;  4 5
}  6 7
[## 	

ForeignKey##	 
(## 
$str## #
)### $
]##$ %
public$$ 
Appointment$$ 
Appointment$$ &
{$$' (
get$$) ,
;$$, -
set$$. 1
;$$1 2
}$$3 4
=$$4 5
null$$5 9
!$$9 :
;$$: ;
[&& 	

ForeignKey&&	 
(&& 
$str&& 
)&&  
]&&  !
public'' 
Patient'' 
Patient'' 
{''  
get''! $
;''$ %
set''& )
;'') *
}''+ ,
=''- .
null''/ 3
!''3 4
;''4 5
[)) 	

ForeignKey))	 
()) 
$str)) 
))) 
]))  
public** 
Doctor** 
Doctor** 
{** 
get** "
;**" #
set**$ '
;**' (
}**) *
=**+ ,
null**, 0
!**0 1
;**1 2
}++ 
},, Ï
YC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\DoctorLeaves.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
public 

class 
DoctorLeaves 
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
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
] 
public 
DateOnly 
	LeaveDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
Reason 
{ 
get  #
;# $
set% (
;( )
}* +
public 
DateTimeOffset 

CreateDate (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
[ 	

ForeignKey	 
( 
nameof 
( 
DoctorId #
)# $
)$ %
]% &
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
} 
} Ô
SC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\Doctor.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
[ 
Index 

(
 
nameof 
( 
Specialisation  
)  !
,! "
Name" &
=' (
$str( B
)B C
]C D
public 

class 
Doctor 
{		 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
] 
public 
string 
? 
UserId 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
FullName 
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
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=2 3
null4 8
!8 9
;9 :
[ 	
Required	 
] 
[ 	
Range	 
( 
$num 
, 
$num 
) 
] 
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
] 
[ 	
Column	 
( 
TypeName 
= 
$str *
)* +
]+ ,
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public   
bool   
IsActive   
{   
get   "
;  " #
set  $ '
;  ' (
}  ) *
=  + ,
true  - 1
;  1 2
[## 	

ForeignKey##	 
(## 
$str## 
)## 
]## 
public$$ 
User$$ 
?$$ 
User$$ 
{$$ 
get$$ 
;$$  
set$$! $
;$$$ %
}$$& '
public&& 
ICollection&& 
<&& 
Appointment&& &
>&&& '
Appointments&&( 4
{&&5 6
get&&7 :
;&&: ;
set&&< ?
;&&? @
}&&A B
=&&C D
[&&E F
]&&F G
;&&G H
public'' 
ICollection'' 
<'' 
AvailableSlots'' )
>'') *
AvailableSlots''+ 9
{'': ;
get''< ?
;''? @
set''A D
;''D E
}''F G
=''H I
[''J K
]''K L
;''L M
public(( 
ICollection(( 
<(( 
DoctorLeaves(( '
>((' (
DoctorLeaves(() 5
{((6 7
get((8 ;
;((; <
set((= @
;((@ A
}((B C
=((D E
[((F G
]((G H
;((H I
public)) 
ICollection)) 
<)) 
HealthRecord)) '
>))' (
HealthRecords))) 6
{))7 8
get))9 <
;))< =
set))> A
;))A B
}))C D
=))E F
[))G H
]))H I
;))I J
}++ 
},, ∏
[C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\AvailableSlots.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
public 

class 
AvailableSlots 
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
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
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
public 
DateTimeOffset 
CreatedDate )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
[ 	

ForeignKey	 
( 
nameof 
( 
DoctorId #
)# $
)$ %
]% &
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
} 
} ∫
[C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\AuthorResponse.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
public 

class 
AuthorResponse 
{ 
public 
string 
? 
AccessToken "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
? 
Message 
{  
get! $
;$ %
set& )
;) *
}+ ,
public

 
int

 
	ExpiresIn

 
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
} 
} ∫
XC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Models\Appointment.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Models 
{ 
public 

class 
Appointment 
{ 
public		 
int		 
AppointmentId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
[ 	
Required	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
] 
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
[ 	
AllowedValues	 
( 
$str  
,  !
$str" -
,- .
$str/ :
,: ;
$str< G
,G H
ErrorMessageI U
=V W
$str	X ì
)
ì î
]
î ï
public 
string 
Status 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
$str- 6
;6 7
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public   
DateTimeOffset   
CreatedDate   )
{  * +
get  , /
;  / 0
set  1 4
;  4 5
}  6 7
[## 	

ForeignKey##	 
(## 
$str## 
)##  
]##  !
public$$ 
Patient$$ 
Patient$$ 
{$$  
get$$! $
;$$$ %
set$$& )
;$$) *
}$$+ ,
=$$- .
null$$/ 3
!$$3 4
;$$4 5
[&& 	

ForeignKey&&	 
(&& 
$str&& 
)&& 
]&&  
public'' 
Doctor'' 
Doctor'' 
{'' 
get'' "
;''" #
set''$ '
;''' (
}'') *
=''+ ,
null''- 1
!''1 2
;''2 3
public)) 
HealthRecord)) 
?)) 
HealthRecord)) )
{))* +
get)), /
;))/ 0
set))1 4
;))4 5
}))6 7
}** 
}++ §
kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260619034707_AdminSeeded.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Migrations #
{ 
public 

partial 
class 
AdminSeeded $
:% &
	Migration' 0
{		 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
} 	
} 
} ûŸ
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260618170822_Removesedding.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Migrations #
{		 
public 

partial 
class 
Removesedding &
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
. 
DropForeignKey +
(+ ,
name 
: 
$str /
,/ 0
table 
: 
$str  
)  !
;! "
migrationBuilder 
. 
DropForeignKey +
(+ ,
name 
: 
$str 0
,0 1
table 
: 
$str !
)! "
;" #
migrationBuilder 
. 
	DropTable &
(& '
name 
: 
$str 
) 
; 
migrationBuilder 
. 
	DropIndex &
(& '
name 
: 
$str *
,* +
table 
: 
$str !
)! "
;" #
migrationBuilder 
. 
	DropIndex &
(& '
name   
:   
$str   0
,  0 1
table!! 
:!! 
$str!! %
)!!% &
;!!& '
migrationBuilder## 
.## 

DeleteData## '
(##' (
table$$ 
:$$ 
$str$$  
,$$  !
	keyColumn%% 
:%% 
$str%% %
,%%% &
keyValue&& 
:&& 
$num&& 
)&& 
;&& 
migrationBuilder(( 
.(( 

DeleteData(( '
(((' (
table)) 
:)) 
$str)) !
,))! "
	keyColumn** 
:** 
$str** &
,**& '
keyValue++ 
:++ 
$num++ 
)++ 
;++ 
migrationBuilder-- 
.-- 
RenameColumn-- )
(--) *
name.. 
:.. 
$str.. !
,..! "
table// 
:// 
$str// !
,//! "
newName00 
:00 
$str00 #
)00# $
;00$ %
migrationBuilder22 
.22 
AlterColumn22 (
<22( )
string22) /
>22/ 0
(220 1
name33 
:33 
$str33 
,33 
table44 
:44 
$str44 !
,44! "
type55 
:55 
$str55 %
,55% &
nullable66 
:66 
true66 
,66 

oldClrType77 
:77 
typeof77 "
(77" #
int77# &
)77& '
,77' (
oldType88 
:88 
$str88 
)88 
;88  
migrationBuilder:: 
.:: 
AlterColumn:: (
<::( )
string::) /
>::/ 0
(::0 1
name;; 
:;; 
$str;; 
,;; 
table<< 
:<< 
$str<< !
,<<! "
type== 
:== 
$str== %
,==% &
nullable>> 
:>> 
false>> 
,>>  
defaultValue?? 
:?? 
$str??  
,??  !

oldClrType@@ 
:@@ 
typeof@@ "
(@@" #
string@@# )
)@@) *
,@@* +
oldTypeAA 
:AA 
$strAA (
,AA( )
oldNullableBB 
:BB 
trueBB !
)BB! "
;BB" #
migrationBuilderDD 
.DD 
AlterColumnDD (
<DD( )
stringDD) /
>DD/ 0
(DD0 1
nameEE 
:EE 
$strEE  
,EE  !
tableFF 
:FF 
$strFF !
,FF! "
typeGG 
:GG 
$strGG %
,GG% &
nullableHH 
:HH 
falseHH 
,HH  
defaultValueII 
:II 
$strII  
,II  !

oldClrTypeJJ 
:JJ 
typeofJJ "
(JJ" #
stringJJ# )
)JJ) *
,JJ* +
oldTypeKK 
:KK 
$strKK (
,KK( )
oldNullableLL 
:LL 
trueLL !
)LL! "
;LL" #
migrationBuilderNN 
.NN 
AlterColumnNN (
<NN( )
stringNN) /
>NN/ 0
(NN0 1
nameOO 
:OO 
$strOO 
,OO 
tablePP 
:PP 
$strPP  
,PP  !
typeQQ 
:QQ 
$strQQ %
,QQ% &
nullableRR 
:RR 
falseRR 
,RR  

oldClrTypeSS 
:SS 
typeofSS "
(SS" #
intSS# &
)SS& '
,SS' (
oldTypeTT 
:TT 
$strTT 
)TT 
;TT  
migrationBuilderVV 
.VV 
	AddColumnVV &
<VV& '
stringVV' -
>VV- .
(VV. /
nameWW 
:WW 
$strWW %
,WW% &
tableXX 
:XX 
$strXX $
,XX$ %
typeYY 
:YY 
$strYY $
,YY$ %
	maxLengthZZ 
:ZZ 
$numZZ 
,ZZ 
nullable[[ 
:[[ 
false[[ 
,[[  
defaultValue\\ 
:\\ 
$str\\  
)\\  !
;\\! "
migrationBuilder^^ 
.^^ 
CreateIndex^^ (
(^^( )
name__ 
:__ 
$str__ *
,__* +
table`` 
:`` 
$str`` !
,``! "
columnaa 
:aa 
$straa  
,aa  !
uniquebb 
:bb 
truebb 
,bb 
filtercc 
:cc 
$strcc .
)cc. /
;cc/ 0
migrationBuilderee 
.ee 
CreateIndexee (
(ee( )
nameff 
:ff 
$strff :
,ff: ;
tablegg 
:gg 
$strgg  
,gg  !
columnshh 
:hh 
newhh 
[hh 
]hh 
{hh  
$strhh! 1
,hh1 2
$strhh3 =
}hh> ?
)hh? @
;hh@ A
migrationBuilderjj 
.jj 
CreateIndexjj (
(jj( )
namekk 
:kk 
$strkk -
,kk- .
tablell 
:ll 
$strll %
,ll% &
columnsmm 
:mm 
newmm 
[mm 
]mm 
{mm  
$strmm! +
,mm+ ,
$strmm- 8
}mm9 :
)mm: ;
;mm; <
migrationBuilderoo 
.oo 
AddForeignKeyoo *
(oo* +
namepp 
:pp 
$strpp 5
,pp5 6
tableqq 
:qq 
$strqq  
,qq  !
columnrr 
:rr 
$strrr  
,rr  !
principalTabless 
:ss 
$strss  -
,ss- .
principalColumntt 
:tt  
$strtt! %
,tt% &
onDeleteuu 
:uu 
ReferentialActionuu +
.uu+ ,
Cascadeuu, 3
)uu3 4
;uu4 5
migrationBuilderww 
.ww 
AddForeignKeyww *
(ww* +
namexx 
:xx 
$strxx 6
,xx6 7
tableyy 
:yy 
$stryy !
,yy! "
columnzz 
:zz 
$strzz  
,zz  !
principalTable{{ 
:{{ 
$str{{  -
,{{- .
principalColumn|| 
:||  
$str||! %
,||% &
onDelete}} 
:}} 
ReferentialAction}} +
.}}+ ,
Cascade}}, 3
)}}3 4
;}}4 5
}~~ 	
	protected
ÅÅ 
override
ÅÅ 
void
ÅÅ 
Down
ÅÅ  $
(
ÅÅ$ %
MigrationBuilder
ÅÅ% 5
migrationBuilder
ÅÅ6 F
)
ÅÅF G
{
ÇÇ 	
migrationBuilder
ÉÉ 
.
ÉÉ 
DropForeignKey
ÉÉ +
(
ÉÉ+ ,
name
ÑÑ 
:
ÑÑ 
$str
ÑÑ 5
,
ÑÑ5 6
table
ÖÖ 
:
ÖÖ 
$str
ÖÖ  
)
ÖÖ  !
;
ÖÖ! "
migrationBuilder
áá 
.
áá 
DropForeignKey
áá +
(
áá+ ,
name
àà 
:
àà 
$str
àà 6
,
àà6 7
table
ââ 
:
ââ 
$str
ââ !
)
ââ! "
;
ââ" #
migrationBuilder
ãã 
.
ãã 
	DropIndex
ãã &
(
ãã& '
name
åå 
:
åå 
$str
åå *
,
åå* +
table
çç 
:
çç 
$str
çç !
)
çç! "
;
çç" #
migrationBuilder
èè 
.
èè 
	DropIndex
èè &
(
èè& '
name
êê 
:
êê 
$str
êê :
,
êê: ;
table
ëë 
:
ëë 
$str
ëë  
)
ëë  !
;
ëë! "
migrationBuilder
ìì 
.
ìì 
	DropIndex
ìì &
(
ìì& '
name
îî 
:
îî 
$str
îî -
,
îî- .
table
ïï 
:
ïï 
$str
ïï %
)
ïï% &
;
ïï& '
migrationBuilder
óó 
.
óó 

DropColumn
óó '
(
óó' (
name
òò 
:
òò 
$str
òò %
,
òò% &
table
ôô 
:
ôô 
$str
ôô $
)
ôô$ %
;
ôô% &
migrationBuilder
õõ 
.
õõ 
RenameColumn
õõ )
(
õõ) *
name
úú 
:
úú 
$str
úú  
,
úú  !
table
ùù 
:
ùù 
$str
ùù !
,
ùù! "
newName
ûû 
:
ûû 
$str
ûû $
)
ûû$ %
;
ûû% &
migrationBuilder
†† 
.
†† 
AlterColumn
†† (
<
††( )
int
††) ,
>
††, -
(
††- .
name
°° 
:
°° 
$str
°° 
,
°° 
table
¢¢ 
:
¢¢ 
$str
¢¢ !
,
¢¢! "
type
££ 
:
££ 
$str
££ 
,
££ 
nullable
§§ 
:
§§ 
false
§§ 
,
§§  
defaultValue
•• 
:
•• 
$num
•• 
,
••  

oldClrType
¶¶ 
:
¶¶ 
typeof
¶¶ "
(
¶¶" #
string
¶¶# )
)
¶¶) *
,
¶¶* +
oldType
ßß 
:
ßß 
$str
ßß (
,
ßß( )
oldNullable
®® 
:
®® 
true
®® !
)
®®! "
;
®®" #
migrationBuilder
™™ 
.
™™ 
AlterColumn
™™ (
<
™™( )
string
™™) /
>
™™/ 0
(
™™0 1
name
´´ 
:
´´ 
$str
´´ 
,
´´ 
table
¨¨ 
:
¨¨ 
$str
¨¨ !
,
¨¨! "
type
≠≠ 
:
≠≠ 
$str
≠≠ %
,
≠≠% &
nullable
ÆÆ 
:
ÆÆ 
true
ÆÆ 
,
ÆÆ 

oldClrType
ØØ 
:
ØØ 
typeof
ØØ "
(
ØØ" #
string
ØØ# )
)
ØØ) *
,
ØØ* +
oldType
∞∞ 
:
∞∞ 
$str
∞∞ (
)
∞∞( )
;
∞∞) *
migrationBuilder
≤≤ 
.
≤≤ 
AlterColumn
≤≤ (
<
≤≤( )
string
≤≤) /
>
≤≤/ 0
(
≤≤0 1
name
≥≥ 
:
≥≥ 
$str
≥≥  
,
≥≥  !
table
¥¥ 
:
¥¥ 
$str
¥¥ !
,
¥¥! "
type
µµ 
:
µµ 
$str
µµ %
,
µµ% &
nullable
∂∂ 
:
∂∂ 
true
∂∂ 
,
∂∂ 

oldClrType
∑∑ 
:
∑∑ 
typeof
∑∑ "
(
∑∑" #
string
∑∑# )
)
∑∑) *
,
∑∑* +
oldType
∏∏ 
:
∏∏ 
$str
∏∏ (
)
∏∏( )
;
∏∏) *
migrationBuilder
∫∫ 
.
∫∫ 
AlterColumn
∫∫ (
<
∫∫( )
int
∫∫) ,
>
∫∫, -
(
∫∫- .
name
ªª 
:
ªª 
$str
ªª 
,
ªª 
table
ºº 
:
ºº 
$str
ºº  
,
ºº  !
type
ΩΩ 
:
ΩΩ 
$str
ΩΩ 
,
ΩΩ 
nullable
ææ 
:
ææ 
false
ææ 
,
ææ  

oldClrType
øø 
:
øø 
typeof
øø "
(
øø" #
string
øø# )
)
øø) *
,
øø* +
oldType
¿¿ 
:
¿¿ 
$str
¿¿ (
)
¿¿( )
;
¿¿) *
migrationBuilder
¬¬ 
.
¬¬ 
CreateTable
¬¬ (
(
¬¬( )
name
√√ 
:
√√ 
$str
√√ 
,
√√ 
columns
ƒƒ 
:
ƒƒ 
table
ƒƒ 
=>
ƒƒ !
new
ƒƒ" %
{
≈≈ 
UserId
∆∆ 
=
∆∆ 
table
∆∆ "
.
∆∆" #
Column
∆∆# )
<
∆∆) *
int
∆∆* -
>
∆∆- .
(
∆∆. /
type
∆∆/ 3
:
∆∆3 4
$str
∆∆5 :
,
∆∆: ;
nullable
∆∆< D
:
∆∆D E
false
∆∆F K
)
∆∆K L
.
«« 

Annotation
«« #
(
««# $
$str
««$ 8
,
««8 9
$str
««: @
)
««@ A
,
««A B
CreatedDate
»» 
=
»»  !
table
»»" '
.
»»' (
Column
»»( .
<
»». /
DateTimeOffset
»»/ =
>
»»= >
(
»»> ?
type
»»? C
:
»»C D
$str
»»E U
,
»»U V
nullable
»»W _
:
»»_ `
false
»»a f
)
»»f g
,
»»g h
Email
…… 
=
…… 
table
…… !
.
……! "
Column
……" (
<
……( )
string
……) /
>
……/ 0
(
……0 1
type
……1 5
:
……5 6
$str
……7 F
,
……F G
nullable
……H P
:
……P Q
false
……R W
)
……W X
,
……X Y
PasswordHash
    
=
  ! "
table
  # (
.
  ( )
Column
  ) /
<
  / 0
string
  0 6
>
  6 7
(
  7 8
type
  8 <
:
  < =
$str
  > M
,
  M N
	maxLength
  O X
:
  X Y
$num
  Z ]
,
  ] ^
nullable
  _ g
:
  g h
false
  i n
)
  n o
,
  o p
RefreshToken
ÀÀ  
=
ÀÀ! "
table
ÀÀ# (
.
ÀÀ( )
Column
ÀÀ) /
<
ÀÀ/ 0
string
ÀÀ0 6
>
ÀÀ6 7
(
ÀÀ7 8
type
ÀÀ8 <
:
ÀÀ< =
$str
ÀÀ> M
,
ÀÀM N
	maxLength
ÀÀO X
:
ÀÀX Y
$num
ÀÀZ ]
,
ÀÀ] ^
nullable
ÀÀ_ g
:
ÀÀg h
true
ÀÀi m
)
ÀÀm n
,
ÀÀn o 
RefreshTokenExpiry
ÃÃ &
=
ÃÃ' (
table
ÃÃ) .
.
ÃÃ. /
Column
ÃÃ/ 5
<
ÃÃ5 6
DateTimeOffset
ÃÃ6 D
>
ÃÃD E
(
ÃÃE F
type
ÃÃF J
:
ÃÃJ K
$str
ÃÃL \
,
ÃÃ\ ]
nullable
ÃÃ^ f
:
ÃÃf g
false
ÃÃh m
)
ÃÃm n
,
ÃÃn o
Role
ÕÕ 
=
ÕÕ 
table
ÕÕ  
.
ÕÕ  !
Column
ÕÕ! '
<
ÕÕ' (
string
ÕÕ( .
>
ÕÕ. /
(
ÕÕ/ 0
type
ÕÕ0 4
:
ÕÕ4 5
$str
ÕÕ6 D
,
ÕÕD E
	maxLength
ÕÕF O
:
ÕÕO P
$num
ÕÕQ S
,
ÕÕS T
nullable
ÕÕU ]
:
ÕÕ] ^
false
ÕÕ_ d
)
ÕÕd e
}
ŒŒ 
,
ŒŒ 
constraints
œœ 
:
œœ 
table
œœ "
=>
œœ# %
{
–– 
table
—— 
.
—— 

PrimaryKey
—— $
(
——$ %
$str
——% /
,
——/ 0
x
——1 2
=>
——3 5
x
——6 7
.
——7 8
UserId
——8 >
)
——> ?
;
——? @
}
““ 
)
““ 
;
““ 
migrationBuilder
‘‘ 
.
‘‘ 

InsertData
‘‘ '
(
‘‘' (
table
’’ 
:
’’ 
$str
’’ 
,
’’ 
columns
÷÷ 
:
÷÷ 
new
÷÷ 
[
÷÷ 
]
÷÷ 
{
÷÷  
$str
÷÷! )
,
÷÷) *
$str
÷÷+ 8
,
÷÷8 9
$str
÷÷: A
,
÷÷A B
$str
÷÷C Q
,
÷÷Q R
$str
÷÷S a
,
÷÷a b
$str
÷÷c w
,
÷÷w x
$str
÷÷y 
}÷÷Ä Å
,÷÷Å Ç
values
◊◊ 
:
◊◊ 
new
◊◊ 
object
◊◊ "
[
◊◊" #
,
◊◊# $
]
◊◊$ %
{
ÿÿ 
{
ŸŸ 
$num
ŸŸ 
,
ŸŸ 
new
ŸŸ 
DateTimeOffset
ŸŸ +
(
ŸŸ+ ,
new
ŸŸ, /
DateTime
ŸŸ0 8
(
ŸŸ8 9
$num
ŸŸ9 =
,
ŸŸ= >
$num
ŸŸ? @
,
ŸŸ@ A
$num
ŸŸB C
,
ŸŸC D
$num
ŸŸE F
,
ŸŸF G
$num
ŸŸH I
,
ŸŸI J
$num
ŸŸK L
,
ŸŸL M
$num
ŸŸN O
,
ŸŸO P
DateTimeKind
ŸŸQ ]
.
ŸŸ] ^
Unspecified
ŸŸ^ i
)
ŸŸi j
,
ŸŸj k
new
ŸŸl o
TimeSpan
ŸŸp x
(
ŸŸx y
$num
ŸŸy z
,
ŸŸz {
$num
ŸŸ| }
,
ŸŸ} ~
$numŸŸ Ä
,ŸŸÄ Å
$numŸŸÇ É
,ŸŸÉ Ñ
$numŸŸÖ Ü
)ŸŸÜ á
)ŸŸá à
,ŸŸà â
$strŸŸä ö
,ŸŸö õ
$strŸŸú ™
,ŸŸ™ ´
nullŸŸ¨ ∞
,ŸŸ∞ ±
newŸŸ≤ µ
DateTimeOffsetŸŸ∂ ƒ
(ŸŸƒ ≈
newŸŸ≈ »
DateTimeŸŸ… —
(ŸŸ— “
$numŸŸ“ ”
,ŸŸ” ‘
$numŸŸ’ ÷
,ŸŸ÷ ◊
$numŸŸÿ Ÿ
,ŸŸŸ ⁄
$numŸŸ€ ‹
,ŸŸ‹ ›
$numŸŸﬁ ﬂ
,ŸŸﬂ ‡
$numŸŸ· ‚
,ŸŸ‚ „
$numŸŸ‰ Â
,ŸŸÂ Ê
DateTimeKindŸŸÁ Û
.ŸŸÛ Ù
UnspecifiedŸŸÙ ˇ
)ŸŸˇ Ä
,ŸŸÄ Å
newŸŸÇ Ö
TimeSpanŸŸÜ é
(ŸŸé è
$numŸŸè ê
,ŸŸê ë
$numŸŸí ì
,ŸŸì î
$numŸŸï ñ
,ŸŸñ ó
$numŸŸò ô
,ŸŸô ö
$numŸŸõ ú
)ŸŸú ù
)ŸŸù û
,ŸŸû ü
$strŸŸ† ß
}ŸŸ® ©
,ŸŸ© ™
{
⁄⁄ 
$num
⁄⁄ 
,
⁄⁄ 
new
⁄⁄ 
DateTimeOffset
⁄⁄ +
(
⁄⁄+ ,
new
⁄⁄, /
DateTime
⁄⁄0 8
(
⁄⁄8 9
$num
⁄⁄9 =
,
⁄⁄= >
$num
⁄⁄? @
,
⁄⁄@ A
$num
⁄⁄B C
,
⁄⁄C D
$num
⁄⁄E F
,
⁄⁄F G
$num
⁄⁄H I
,
⁄⁄I J
$num
⁄⁄K L
,
⁄⁄L M
$num
⁄⁄N O
,
⁄⁄O P
DateTimeKind
⁄⁄Q ]
.
⁄⁄] ^
Unspecified
⁄⁄^ i
)
⁄⁄i j
,
⁄⁄j k
new
⁄⁄l o
TimeSpan
⁄⁄p x
(
⁄⁄x y
$num
⁄⁄y z
,
⁄⁄z {
$num
⁄⁄| }
,
⁄⁄} ~
$num⁄⁄ Ä
,⁄⁄Ä Å
$num⁄⁄Ç É
,⁄⁄É Ñ
$num⁄⁄Ö Ü
)⁄⁄Ü á
)⁄⁄á à
,⁄⁄à â
$str⁄⁄ä ú
,⁄⁄ú ù
$str⁄⁄û Æ
,⁄⁄Æ Ø
null⁄⁄∞ ¥
,⁄⁄¥ µ
new⁄⁄∂ π
DateTimeOffset⁄⁄∫ »
(⁄⁄» …
new⁄⁄… Ã
DateTime⁄⁄Õ ’
(⁄⁄’ ÷
$num⁄⁄÷ ◊
,⁄⁄◊ ÿ
$num⁄⁄Ÿ ⁄
,⁄⁄⁄ €
$num⁄⁄‹ ›
,⁄⁄› ﬁ
$num⁄⁄ﬂ ‡
,⁄⁄‡ ·
$num⁄⁄‚ „
,⁄⁄„ ‰
$num⁄⁄Â Ê
,⁄⁄Ê Á
$num⁄⁄Ë È
,⁄⁄È Í
DateTimeKind⁄⁄Î ˜
.⁄⁄˜ ¯
Unspecified⁄⁄¯ É
)⁄⁄É Ñ
,⁄⁄Ñ Ö
new⁄⁄Ü â
TimeSpan⁄⁄ä í
(⁄⁄í ì
$num⁄⁄ì î
,⁄⁄î ï
$num⁄⁄ñ ó
,⁄⁄ó ò
$num⁄⁄ô ö
,⁄⁄ö õ
$num⁄⁄ú ù
,⁄⁄ù û
$num⁄⁄ü †
)⁄⁄† °
)⁄⁄° ¢
,⁄⁄¢ £
$str⁄⁄§ ≠
}⁄⁄Æ Ø
,⁄⁄Ø ∞
{
€€ 
$num
€€ 
,
€€ 
new
€€ 
DateTimeOffset
€€ +
(
€€+ ,
new
€€, /
DateTime
€€0 8
(
€€8 9
$num
€€9 =
,
€€= >
$num
€€? @
,
€€@ A
$num
€€B C
,
€€C D
$num
€€E F
,
€€F G
$num
€€H I
,
€€I J
$num
€€K L
,
€€L M
$num
€€N O
,
€€O P
DateTimeKind
€€Q ]
.
€€] ^
Unspecified
€€^ i
)
€€i j
,
€€j k
new
€€l o
TimeSpan
€€p x
(
€€x y
$num
€€y z
,
€€z {
$num
€€| }
,
€€} ~
$num€€ Ä
,€€Ä Å
$num€€Ç É
,€€É Ñ
$num€€Ö Ü
)€€Ü á
)€€á à
,€€à â
$str€€ä ú
,€€ú ù
$str€€û Æ
,€€Æ Ø
null€€∞ ¥
,€€¥ µ
new€€∂ π
DateTimeOffset€€∫ »
(€€» …
new€€… Ã
DateTime€€Õ ’
(€€’ ÷
$num€€÷ ◊
,€€◊ ÿ
$num€€Ÿ ⁄
,€€⁄ €
$num€€‹ ›
,€€› ﬁ
$num€€ﬂ ‡
,€€‡ ·
$num€€‚ „
,€€„ ‰
$num€€Â Ê
,€€Ê Á
$num€€Ë È
,€€È Í
DateTimeKind€€Î ˜
.€€˜ ¯
Unspecified€€¯ É
)€€É Ñ
,€€Ñ Ö
new€€Ü â
TimeSpan€€ä í
(€€í ì
$num€€ì î
,€€î ï
$num€€ñ ó
,€€ó ò
$num€€ô ö
,€€ö õ
$num€€ú ù
,€€ù û
$num€€ü †
)€€† °
)€€° ¢
,€€¢ £
$str€€§ ¨
}€€≠ Æ
}
‹‹ 
)
‹‹ 
;
‹‹ 
migrationBuilder
ﬁﬁ 
.
ﬁﬁ 

InsertData
ﬁﬁ '
(
ﬁﬁ' (
table
ﬂﬂ 
:
ﬂﬂ 
$str
ﬂﬂ  
,
ﬂﬂ  !
columns
‡‡ 
:
‡‡ 
new
‡‡ 
[
‡‡ 
]
‡‡ 
{
‡‡  
$str
‡‡! +
,
‡‡+ ,
$str
‡‡- >
,
‡‡> ?
$str
‡‡@ J
,
‡‡J K
$str
‡‡L V
,
‡‡V W
$str
‡‡X h
,
‡‡h i
$str
‡‡j r
,
‡‡r s
$str‡‡t á
}‡‡à â
,‡‡â ä
values
·· 
:
·· 
new
·· 
object
·· "
[
··" #
]
··# $
{
··% &
$num
··' (
,
··( )
$num
··* .
,
··. /
$str
··0 @
,
··@ A
true
··B F
,
··F G
$str
··H T
,
··T U
$num
··V W
,
··W X
$num
··Y Z
}
··[ \
)
··\ ]
;
··] ^
migrationBuilder
„„ 
.
„„ 

InsertData
„„ '
(
„„' (
table
‰‰ 
:
‰‰ 
$str
‰‰ !
,
‰‰! "
columns
ÂÂ 
:
ÂÂ 
new
ÂÂ 
[
ÂÂ 
]
ÂÂ 
{
ÂÂ  
$str
ÂÂ! ,
,
ÂÂ, -
$str
ÂÂ. ;
,
ÂÂ; <
$str
ÂÂ= G
,
ÂÂG H
$str
ÂÂI Q
,
ÂÂQ R
$str
ÂÂS `
,
ÂÂ` a
$str
ÂÂb m
,
ÂÂm n
$str
ÂÂo |
,
ÂÂ| }
$strÂÂ~ Ü
}ÂÂá à
,ÂÂà â
values
ÊÊ 
:
ÊÊ 
new
ÊÊ 
object
ÊÊ "
[
ÊÊ" #
]
ÊÊ# $
{
ÊÊ% &
$num
ÊÊ' (
,
ÊÊ( )
new
ÊÊ* -
DateOnly
ÊÊ. 6
(
ÊÊ6 7
$num
ÊÊ7 ;
,
ÊÊ; <
$num
ÊÊ= >
,
ÊÊ> ?
$num
ÊÊ@ A
)
ÊÊA B
,
ÊÊB C
$str
ÊÊD M
,
ÊÊM N
$str
ÊÊO U
,
ÊÊU V
null
ÊÊW [
,
ÊÊ[ \
true
ÊÊ] a
,
ÊÊa b
$str
ÊÊc o
,
ÊÊo p
$num
ÊÊq r
}
ÊÊs t
)
ÊÊt u
;
ÊÊu v
migrationBuilder
ËË 
.
ËË 
CreateIndex
ËË (
(
ËË( )
name
ÈÈ 
:
ÈÈ 
$str
ÈÈ *
,
ÈÈ* +
table
ÍÍ 
:
ÍÍ 
$str
ÍÍ !
,
ÍÍ! "
column
ÎÎ 
:
ÎÎ 
$str
ÎÎ  
,
ÎÎ  !
unique
ÏÏ 
:
ÏÏ 
true
ÏÏ 
)
ÏÏ 
;
ÏÏ 
migrationBuilder
ÓÓ 
.
ÓÓ 
CreateIndex
ÓÓ (
(
ÓÓ( )
name
ÔÔ 
:
ÔÔ 
$str
ÔÔ 0
,
ÔÔ0 1
table
 
:
 
$str
 %
,
% &
column
ÒÒ 
:
ÒÒ 
$str
ÒÒ "
)
ÒÒ" #
;
ÒÒ# $
migrationBuilder
ÛÛ 
.
ÛÛ 
CreateIndex
ÛÛ (
(
ÛÛ( )
name
ÙÙ 
:
ÙÙ 
$str
ÙÙ &
,
ÙÙ& '
table
ıı 
:
ıı 
$str
ıı 
,
ıı 
column
ˆˆ 
:
ˆˆ 
$str
ˆˆ 
,
ˆˆ  
unique
˜˜ 
:
˜˜ 
true
˜˜ 
)
˜˜ 
;
˜˜ 
migrationBuilder
˘˘ 
.
˘˘ 
AddForeignKey
˘˘ *
(
˘˘* +
name
˙˙ 
:
˙˙ 
$str
˙˙ /
,
˙˙/ 0
table
˚˚ 
:
˚˚ 
$str
˚˚  
,
˚˚  !
column
¸¸ 
:
¸¸ 
$str
¸¸  
,
¸¸  !
principalTable
˝˝ 
:
˝˝ 
$str
˝˝  '
,
˝˝' (
principalColumn
˛˛ 
:
˛˛  
$str
˛˛! )
,
˛˛) *
onDelete
ˇˇ 
:
ˇˇ 
ReferentialAction
ˇˇ +
.
ˇˇ+ ,
Cascade
ˇˇ, 3
)
ˇˇ3 4
;
ˇˇ4 5
migrationBuilder
ÅÅ 
.
ÅÅ 
AddForeignKey
ÅÅ *
(
ÅÅ* +
name
ÇÇ 
:
ÇÇ 
$str
ÇÇ 0
,
ÇÇ0 1
table
ÉÉ 
:
ÉÉ 
$str
ÉÉ !
,
ÉÉ! "
column
ÑÑ 
:
ÑÑ 
$str
ÑÑ  
,
ÑÑ  !
principalTable
ÖÖ 
:
ÖÖ 
$str
ÖÖ  '
,
ÖÖ' (
principalColumn
ÜÜ 
:
ÜÜ  
$str
ÜÜ! )
,
ÜÜ) *
onDelete
áá 
:
áá 
ReferentialAction
áá +
.
áá+ ,
Cascade
áá, 3
)
áá3 4
;
áá4 5
}
àà 	
}
ââ 
}ää §
kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260617032518_RolesSeeded.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Migrations #
{ 
public 

partial 
class 
RolesSeeded $
:% &
	Migration' 0
{		 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
} 	
} 
} °∆
xC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260616095529_IdentityAddedInDbContext.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Migrations #
{ 
public		 

partial		 
class		 $
IdentityAddedInDbContext		 1
:		2 3
	Migration		4 =
{

 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str #
,# $
columns 
: 
table 
=> !
new" %
{ 
Id 
= 
table 
. 
Column %
<% &
string& ,
>, -
(- .
type. 2
:2 3
$str4 C
,C D
nullableE M
:M N
falseO T
)T U
,U V
Name 
= 
table  
.  !
Column! '
<' (
string( .
>. /
(/ 0
type0 4
:4 5
$str6 E
,E F
	maxLengthG P
:P Q
$numR U
,U V
nullableW _
:_ `
truea e
)e f
,f g
NormalizedName "
=# $
table% *
.* +
Column+ 1
<1 2
string2 8
>8 9
(9 :
type: >
:> ?
$str@ O
,O P
	maxLengthQ Z
:Z [
$num\ _
,_ `
nullablea i
:i j
truek o
)o p
,p q
ConcurrencyStamp $
=% &
table' ,
., -
Column- 3
<3 4
string4 :
>: ;
(; <
type< @
:@ A
$strB Q
,Q R
nullableS [
:[ \
true] a
)a b
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 5
,5 6
x7 8
=>9 ;
x< =
.= >
Id> @
)@ A
;A B
} 
) 
; 
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str #
,# $
columns 
: 
table 
=> !
new" %
{ 
Id   
=   
table   
.   
Column   %
<  % &
string  & ,
>  , -
(  - .
type  . 2
:  2 3
$str  4 C
,  C D
nullable  E M
:  M N
false  O T
)  T U
,  U V
UserName!! 
=!! 
table!! $
.!!$ %
Column!!% +
<!!+ ,
string!!, 2
>!!2 3
(!!3 4
type!!4 8
:!!8 9
$str!!: I
,!!I J
	maxLength!!K T
:!!T U
$num!!V Y
,!!Y Z
nullable!![ c
:!!c d
true!!e i
)!!i j
,!!j k
NormalizedUserName"" &
=""' (
table"") .
."". /
Column""/ 5
<""5 6
string""6 <
>""< =
(""= >
type""> B
:""B C
$str""D S
,""S T
	maxLength""U ^
:""^ _
$num""` c
,""c d
nullable""e m
:""m n
true""o s
)""s t
,""t u
Email## 
=## 
table## !
.##! "
Column##" (
<##( )
string##) /
>##/ 0
(##0 1
type##1 5
:##5 6
$str##7 F
,##F G
	maxLength##H Q
:##Q R
$num##S V
,##V W
nullable##X `
:##` a
true##b f
)##f g
,##g h
NormalizedEmail$$ #
=$$$ %
table$$& +
.$$+ ,
Column$$, 2
<$$2 3
string$$3 9
>$$9 :
($$: ;
type$$; ?
:$$? @
$str$$A P
,$$P Q
	maxLength$$R [
:$$[ \
$num$$] `
,$$` a
nullable$$b j
:$$j k
true$$l p
)$$p q
,$$q r
EmailConfirmed%% "
=%%# $
table%%% *
.%%* +
Column%%+ 1
<%%1 2
bool%%2 6
>%%6 7
(%%7 8
type%%8 <
:%%< =
$str%%> C
,%%C D
nullable%%E M
:%%M N
false%%O T
)%%T U
,%%U V
PasswordHash&&  
=&&! "
table&&# (
.&&( )
Column&&) /
<&&/ 0
string&&0 6
>&&6 7
(&&7 8
type&&8 <
:&&< =
$str&&> M
,&&M N
nullable&&O W
:&&W X
true&&Y ]
)&&] ^
,&&^ _
SecurityStamp'' !
=''" #
table''$ )
.'') *
Column''* 0
<''0 1
string''1 7
>''7 8
(''8 9
type''9 =
:''= >
$str''? N
,''N O
nullable''P X
:''X Y
true''Z ^
)''^ _
,''_ `
ConcurrencyStamp(( $
=((% &
table((' ,
.((, -
Column((- 3
<((3 4
string((4 :
>((: ;
(((; <
type((< @
:((@ A
$str((B Q
,((Q R
nullable((S [
:(([ \
true((] a
)((a b
,((b c
PhoneNumber)) 
=))  !
table))" '
.))' (
Column))( .
<)). /
string))/ 5
>))5 6
())6 7
type))7 ;
:)); <
$str))= L
,))L M
nullable))N V
:))V W
true))X \
)))\ ]
,))] ^ 
PhoneNumberConfirmed** (
=**) *
table**+ 0
.**0 1
Column**1 7
<**7 8
bool**8 <
>**< =
(**= >
type**> B
:**B C
$str**D I
,**I J
nullable**K S
:**S T
false**U Z
)**Z [
,**[ \
TwoFactorEnabled++ $
=++% &
table++' ,
.++, -
Column++- 3
<++3 4
bool++4 8
>++8 9
(++9 :
type++: >
:++> ?
$str++@ E
,++E F
nullable++G O
:++O P
false++Q V
)++V W
,++W X

LockoutEnd,, 
=,,  
table,,! &
.,,& '
Column,,' -
<,,- .
DateTimeOffset,,. <
>,,< =
(,,= >
type,,> B
:,,B C
$str,,D T
,,,T U
nullable,,V ^
:,,^ _
true,,` d
),,d e
,,,e f
LockoutEnabled-- "
=--# $
table--% *
.--* +
Column--+ 1
<--1 2
bool--2 6
>--6 7
(--7 8
type--8 <
:--< =
$str--> C
,--C D
nullable--E M
:--M N
false--O T
)--T U
,--U V
AccessFailedCount.. %
=..& '
table..( -
...- .
Column... 4
<..4 5
int..5 8
>..8 9
(..9 :
type..: >
:..> ?
$str..@ E
,..E F
nullable..G O
:..O P
false..Q V
)..V W
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
$str22% 5
,225 6
x227 8
=>229 ;
x22< =
.22= >
Id22> @
)22@ A
;22A B
}33 
)33 
;33 
migrationBuilder55 
.55 
CreateTable55 (
(55( )
name66 
:66 
$str66 (
,66( )
columns77 
:77 
table77 
=>77 !
new77" %
{88 
Id99 
=99 
table99 
.99 
Column99 %
<99% &
int99& )
>99) *
(99* +
type99+ /
:99/ 0
$str991 6
,996 7
nullable998 @
:99@ A
false99B G
)99G H
.:: 

Annotation:: #
(::# $
$str::$ 8
,::8 9
$str::: @
)::@ A
,::A B
RoleId;; 
=;; 
table;; "
.;;" #
Column;;# )
<;;) *
string;;* 0
>;;0 1
(;;1 2
type;;2 6
:;;6 7
$str;;8 G
,;;G H
nullable;;I Q
:;;Q R
false;;S X
);;X Y
,;;Y Z
	ClaimType<< 
=<< 
table<<  %
.<<% &
Column<<& ,
<<<, -
string<<- 3
><<3 4
(<<4 5
type<<5 9
:<<9 :
$str<<; J
,<<J K
nullable<<L T
:<<T U
true<<V Z
)<<Z [
,<<[ \

ClaimValue== 
===  
table==! &
.==& '
Column==' -
<==- .
string==. 4
>==4 5
(==5 6
type==6 :
:==: ;
$str==< K
,==K L
nullable==M U
:==U V
true==W [
)==[ \
}>> 
,>> 
constraints?? 
:?? 
table?? "
=>??# %
{@@ 
tableAA 
.AA 

PrimaryKeyAA $
(AA$ %
$strAA% :
,AA: ;
xAA< =
=>AA> @
xAAA B
.AAB C
IdAAC E
)AAE F
;AAF G
tableBB 
.BB 

ForeignKeyBB $
(BB$ %
nameCC 
:CC 
$strCC F
,CCF G
columnDD 
:DD 
xDD  !
=>DD" $
xDD% &
.DD& '
RoleIdDD' -
,DD- .
principalTableEE &
:EE& '
$strEE( 5
,EE5 6
principalColumnFF '
:FF' (
$strFF) -
,FF- .
onDeleteGG  
:GG  !
ReferentialActionGG" 3
.GG3 4
CascadeGG4 ;
)GG; <
;GG< =
}HH 
)HH 
;HH 
migrationBuilderJJ 
.JJ 
CreateTableJJ (
(JJ( )
nameKK 
:KK 
$strKK (
,KK( )
columnsLL 
:LL 
tableLL 
=>LL !
newLL" %
{MM 
IdNN 
=NN 
tableNN 
.NN 
ColumnNN %
<NN% &
intNN& )
>NN) *
(NN* +
typeNN+ /
:NN/ 0
$strNN1 6
,NN6 7
nullableNN8 @
:NN@ A
falseNNB G
)NNG H
.OO 

AnnotationOO #
(OO# $
$strOO$ 8
,OO8 9
$strOO: @
)OO@ A
,OOA B
UserIdPP 
=PP 
tablePP "
.PP" #
ColumnPP# )
<PP) *
stringPP* 0
>PP0 1
(PP1 2
typePP2 6
:PP6 7
$strPP8 G
,PPG H
nullablePPI Q
:PPQ R
falsePPS X
)PPX Y
,PPY Z
	ClaimTypeQQ 
=QQ 
tableQQ  %
.QQ% &
ColumnQQ& ,
<QQ, -
stringQQ- 3
>QQ3 4
(QQ4 5
typeQQ5 9
:QQ9 :
$strQQ; J
,QQJ K
nullableQQL T
:QQT U
trueQQV Z
)QQZ [
,QQ[ \

ClaimValueRR 
=RR  
tableRR! &
.RR& '
ColumnRR' -
<RR- .
stringRR. 4
>RR4 5
(RR5 6
typeRR6 :
:RR: ;
$strRR< K
,RRK L
nullableRRM U
:RRU V
trueRRW [
)RR[ \
}SS 
,SS 
constraintsTT 
:TT 
tableTT "
=>TT# %
{UU 
tableVV 
.VV 

PrimaryKeyVV $
(VV$ %
$strVV% :
,VV: ;
xVV< =
=>VV> @
xVVA B
.VVB C
IdVVC E
)VVE F
;VVF G
tableWW 
.WW 

ForeignKeyWW $
(WW$ %
nameXX 
:XX 
$strXX F
,XXF G
columnYY 
:YY 
xYY  !
=>YY" $
xYY% &
.YY& '
UserIdYY' -
,YY- .
principalTableZZ &
:ZZ& '
$strZZ( 5
,ZZ5 6
principalColumn[[ '
:[[' (
$str[[) -
,[[- .
onDelete\\  
:\\  !
ReferentialAction\\" 3
.\\3 4
Cascade\\4 ;
)\\; <
;\\< =
}]] 
)]] 
;]] 
migrationBuilder__ 
.__ 
CreateTable__ (
(__( )
name`` 
:`` 
$str`` (
,``( )
columnsaa 
:aa 
tableaa 
=>aa !
newaa" %
{bb 
LoginProvidercc !
=cc" #
tablecc$ )
.cc) *
Columncc* 0
<cc0 1
stringcc1 7
>cc7 8
(cc8 9
typecc9 =
:cc= >
$strcc? N
,ccN O
nullableccP X
:ccX Y
falseccZ _
)cc_ `
,cc` a
ProviderKeydd 
=dd  !
tabledd" '
.dd' (
Columndd( .
<dd. /
stringdd/ 5
>dd5 6
(dd6 7
typedd7 ;
:dd; <
$strdd= L
,ddL M
nullableddN V
:ddV W
falseddX ]
)dd] ^
,dd^ _
ProviderDisplayNameee '
=ee( )
tableee* /
.ee/ 0
Columnee0 6
<ee6 7
stringee7 =
>ee= >
(ee> ?
typeee? C
:eeC D
$streeE T
,eeT U
nullableeeV ^
:ee^ _
trueee` d
)eed e
,eee f
UserIdff 
=ff 
tableff "
.ff" #
Columnff# )
<ff) *
stringff* 0
>ff0 1
(ff1 2
typeff2 6
:ff6 7
$strff8 G
,ffG H
nullableffI Q
:ffQ R
falseffS X
)ffX Y
}gg 
,gg 
constraintshh 
:hh 
tablehh "
=>hh# %
{ii 
tablejj 
.jj 

PrimaryKeyjj $
(jj$ %
$strjj% :
,jj: ;
xjj< =
=>jj> @
newjjA D
{jjE F
xjjG H
.jjH I
LoginProviderjjI V
,jjV W
xjjX Y
.jjY Z
ProviderKeyjjZ e
}jjf g
)jjg h
;jjh i
tablekk 
.kk 

ForeignKeykk $
(kk$ %
namell 
:ll 
$strll F
,llF G
columnmm 
:mm 
xmm  !
=>mm" $
xmm% &
.mm& '
UserIdmm' -
,mm- .
principalTablenn &
:nn& '
$strnn( 5
,nn5 6
principalColumnoo '
:oo' (
$stroo) -
,oo- .
onDeletepp  
:pp  !
ReferentialActionpp" 3
.pp3 4
Cascadepp4 ;
)pp; <
;pp< =
}qq 
)qq 
;qq 
migrationBuilderss 
.ss 
CreateTabless (
(ss( )
namett 
:tt 
$strtt '
,tt' (
columnsuu 
:uu 
tableuu 
=>uu !
newuu" %
{vv 
UserIdww 
=ww 
tableww "
.ww" #
Columnww# )
<ww) *
stringww* 0
>ww0 1
(ww1 2
typeww2 6
:ww6 7
$strww8 G
,wwG H
nullablewwI Q
:wwQ R
falsewwS X
)wwX Y
,wwY Z
RoleIdxx 
=xx 
tablexx "
.xx" #
Columnxx# )
<xx) *
stringxx* 0
>xx0 1
(xx1 2
typexx2 6
:xx6 7
$strxx8 G
,xxG H
nullablexxI Q
:xxQ R
falsexxS X
)xxX Y
}yy 
,yy 
constraintszz 
:zz 
tablezz "
=>zz# %
{{{ 
table|| 
.|| 

PrimaryKey|| $
(||$ %
$str||% 9
,||9 :
x||; <
=>||= ?
new||@ C
{||D E
x||F G
.||G H
UserId||H N
,||N O
x||P Q
.||Q R
RoleId||R X
}||Y Z
)||Z [
;||[ \
table}} 
.}} 

ForeignKey}} $
(}}$ %
name~~ 
:~~ 
$str~~ E
,~~E F
column 
: 
x  !
=>" $
x% &
.& '
RoleId' -
,- .
principalTable
ÄÄ &
:
ÄÄ& '
$str
ÄÄ( 5
,
ÄÄ5 6
principalColumn
ÅÅ '
:
ÅÅ' (
$str
ÅÅ) -
,
ÅÅ- .
onDelete
ÇÇ  
:
ÇÇ  !
ReferentialAction
ÇÇ" 3
.
ÇÇ3 4
Cascade
ÇÇ4 ;
)
ÇÇ; <
;
ÇÇ< =
table
ÉÉ 
.
ÉÉ 

ForeignKey
ÉÉ $
(
ÉÉ$ %
name
ÑÑ 
:
ÑÑ 
$str
ÑÑ E
,
ÑÑE F
column
ÖÖ 
:
ÖÖ 
x
ÖÖ  !
=>
ÖÖ" $
x
ÖÖ% &
.
ÖÖ& '
UserId
ÖÖ' -
,
ÖÖ- .
principalTable
ÜÜ &
:
ÜÜ& '
$str
ÜÜ( 5
,
ÜÜ5 6
principalColumn
áá '
:
áá' (
$str
áá) -
,
áá- .
onDelete
àà  
:
àà  !
ReferentialAction
àà" 3
.
àà3 4
Cascade
àà4 ;
)
àà; <
;
àà< =
}
ââ 
)
ââ 
;
ââ 
migrationBuilder
ãã 
.
ãã 
CreateTable
ãã (
(
ãã( )
name
åå 
:
åå 
$str
åå (
,
åå( )
columns
çç 
:
çç 
table
çç 
=>
çç !
new
çç" %
{
éé 
UserId
èè 
=
èè 
table
èè "
.
èè" #
Column
èè# )
<
èè) *
string
èè* 0
>
èè0 1
(
èè1 2
type
èè2 6
:
èè6 7
$str
èè8 G
,
èèG H
nullable
èèI Q
:
èèQ R
false
èèS X
)
èèX Y
,
èèY Z
LoginProvider
êê !
=
êê" #
table
êê$ )
.
êê) *
Column
êê* 0
<
êê0 1
string
êê1 7
>
êê7 8
(
êê8 9
type
êê9 =
:
êê= >
$str
êê? N
,
êêN O
nullable
êêP X
:
êêX Y
false
êêZ _
)
êê_ `
,
êê` a
Name
ëë 
=
ëë 
table
ëë  
.
ëë  !
Column
ëë! '
<
ëë' (
string
ëë( .
>
ëë. /
(
ëë/ 0
type
ëë0 4
:
ëë4 5
$str
ëë6 E
,
ëëE F
nullable
ëëG O
:
ëëO P
false
ëëQ V
)
ëëV W
,
ëëW X
Value
íí 
=
íí 
table
íí !
.
íí! "
Column
íí" (
<
íí( )
string
íí) /
>
íí/ 0
(
íí0 1
type
íí1 5
:
íí5 6
$str
íí7 F
,
ííF G
nullable
ííH P
:
ííP Q
true
ííR V
)
ííV W
}
ìì 
,
ìì 
constraints
îî 
:
îî 
table
îî "
=>
îî# %
{
ïï 
table
ññ 
.
ññ 

PrimaryKey
ññ $
(
ññ$ %
$str
ññ% :
,
ññ: ;
x
ññ< =
=>
ññ> @
new
ññA D
{
ññE F
x
ññG H
.
ññH I
UserId
ññI O
,
ññO P
x
ññQ R
.
ññR S
LoginProvider
ññS `
,
ññ` a
x
ññb c
.
ññc d
Name
ññd h
}
ññi j
)
ññj k
;
ññk l
table
óó 
.
óó 

ForeignKey
óó $
(
óó$ %
name
òò 
:
òò 
$str
òò F
,
òòF G
column
ôô 
:
ôô 
x
ôô  !
=>
ôô" $
x
ôô% &
.
ôô& '
UserId
ôô' -
,
ôô- .
principalTable
öö &
:
öö& '
$str
öö( 5
,
öö5 6
principalColumn
õõ '
:
õõ' (
$str
õõ) -
,
õõ- .
onDelete
úú  
:
úú  !
ReferentialAction
úú" 3
.
úú3 4
Cascade
úú4 ;
)
úú; <
;
úú< =
}
ùù 
)
ùù 
;
ùù 
migrationBuilder
üü 
.
üü 
CreateIndex
üü (
(
üü( )
name
†† 
:
†† 
$str
†† 2
,
††2 3
table
°° 
:
°° 
$str
°° )
,
°°) *
column
¢¢ 
:
¢¢ 
$str
¢¢  
)
¢¢  !
;
¢¢! "
migrationBuilder
§§ 
.
§§ 
CreateIndex
§§ (
(
§§( )
name
•• 
:
•• 
$str
•• %
,
••% &
table
¶¶ 
:
¶¶ 
$str
¶¶ $
,
¶¶$ %
column
ßß 
:
ßß 
$str
ßß (
,
ßß( )
unique
®® 
:
®® 
true
®® 
,
®® 
filter
©© 
:
©© 
$str
©© 6
)
©©6 7
;
©©7 8
migrationBuilder
´´ 
.
´´ 
CreateIndex
´´ (
(
´´( )
name
¨¨ 
:
¨¨ 
$str
¨¨ 2
,
¨¨2 3
table
≠≠ 
:
≠≠ 
$str
≠≠ )
,
≠≠) *
column
ÆÆ 
:
ÆÆ 
$str
ÆÆ  
)
ÆÆ  !
;
ÆÆ! "
migrationBuilder
∞∞ 
.
∞∞ 
CreateIndex
∞∞ (
(
∞∞( )
name
±± 
:
±± 
$str
±± 2
,
±±2 3
table
≤≤ 
:
≤≤ 
$str
≤≤ )
,
≤≤) *
column
≥≥ 
:
≥≥ 
$str
≥≥  
)
≥≥  !
;
≥≥! "
migrationBuilder
µµ 
.
µµ 
CreateIndex
µµ (
(
µµ( )
name
∂∂ 
:
∂∂ 
$str
∂∂ 1
,
∂∂1 2
table
∑∑ 
:
∑∑ 
$str
∑∑ (
,
∑∑( )
column
∏∏ 
:
∏∏ 
$str
∏∏  
)
∏∏  !
;
∏∏! "
migrationBuilder
∫∫ 
.
∫∫ 
CreateIndex
∫∫ (
(
∫∫( )
name
ªª 
:
ªª 
$str
ªª "
,
ªª" #
table
ºº 
:
ºº 
$str
ºº $
,
ºº$ %
column
ΩΩ 
:
ΩΩ 
$str
ΩΩ )
)
ΩΩ) *
;
ΩΩ* +
migrationBuilder
øø 
.
øø 
CreateIndex
øø (
(
øø( )
name
¿¿ 
:
¿¿ 
$str
¿¿ %
,
¿¿% &
table
¡¡ 
:
¡¡ 
$str
¡¡ $
,
¡¡$ %
column
¬¬ 
:
¬¬ 
$str
¬¬ ,
,
¬¬, -
unique
√√ 
:
√√ 
true
√√ 
,
√√ 
filter
ƒƒ 
:
ƒƒ 
$str
ƒƒ :
)
ƒƒ: ;
;
ƒƒ; <
}
≈≈ 	
	protected
»» 
override
»» 
void
»» 
Down
»»  $
(
»»$ %
MigrationBuilder
»»% 5
migrationBuilder
»»6 F
)
»»F G
{
…… 	
migrationBuilder
   
.
   
	DropTable
   &
(
  & '
name
ÀÀ 
:
ÀÀ 
$str
ÀÀ (
)
ÀÀ( )
;
ÀÀ) *
migrationBuilder
ÕÕ 
.
ÕÕ 
	DropTable
ÕÕ &
(
ÕÕ& '
name
ŒŒ 
:
ŒŒ 
$str
ŒŒ (
)
ŒŒ( )
;
ŒŒ) *
migrationBuilder
–– 
.
–– 
	DropTable
–– &
(
––& '
name
—— 
:
—— 
$str
—— (
)
——( )
;
——) *
migrationBuilder
”” 
.
”” 
	DropTable
”” &
(
””& '
name
‘‘ 
:
‘‘ 
$str
‘‘ '
)
‘‘' (
;
‘‘( )
migrationBuilder
÷÷ 
.
÷÷ 
	DropTable
÷÷ &
(
÷÷& '
name
◊◊ 
:
◊◊ 
$str
◊◊ (
)
◊◊( )
;
◊◊) *
migrationBuilder
ŸŸ 
.
ŸŸ 
	DropTable
ŸŸ &
(
ŸŸ& '
name
⁄⁄ 
:
⁄⁄ 
$str
⁄⁄ #
)
⁄⁄# $
;
⁄⁄$ %
migrationBuilder
‹‹ 
.
‹‹ 
	DropTable
‹‹ &
(
‹‹& '
name
›› 
:
›› 
$str
›› #
)
››# $
;
››$ %
}
ﬁﬁ 	
}
ﬂﬂ 
}‡‡ ¡
tC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260615151516_SeedingForPATandDOCT.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Migrations #
{ 
public		 

partial		 
class		  
SeedingForPATandDOCT		 -
:		. /
	Migration		0 9
{

 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 

InsertData '
(' (
table 
: 
$str  
,  !
columns 
: 
new 
[ 
] 
{  
$str! +
,+ ,
$str- >
,> ?
$str@ J
,J K
$strL V
,V W
$strX h
,h i
$strj r
,r s
$str	t á
}
à â
,
â ä
values 
: 
new 
object "
[" #
]# $
{% &
$num' (
,( )
$num* .
,. /
$str0 @
,@ A
trueB F
,F G
$strH T
,T U
$numV W
,W X
$numY Z
}[ \
)\ ]
;] ^
migrationBuilder 
. 

InsertData '
(' (
table 
: 
$str !
,! "
columns 
: 
new 
[ 
] 
{  
$str! ,
,, -
$str. ;
,; <
$str= G
,G H
$strI Q
,Q R
$strS `
,` a
$strb m
,m n
$stro |
,| }
$str	~ Ü
}
á à
,
à â
values 
: 
new 
object "
[" #
]# $
{% &
$num' (
,( )
new* -
DateOnly. 6
(6 7
$num7 ;
,; <
$num= >
,> ?
$num@ A
)A B
,B C
$strD M
,M N
$strO U
,U V
nullW [
,[ \
true] a
,a b
$strc o
,o p
$numq r
}s t
)t u
;u v
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
migrationBuilder 
. 

DeleteData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
) 
; 
migrationBuilder!! 
.!! 

DeleteData!! '
(!!' (
table"" 
:"" 
$str"" !
,""! "
	keyColumn## 
:## 
$str## &
,##& '
keyValue$$ 
:$$ 
$num$$ 
)$$ 
;$$ 
}%% 	
}&& 
}'' †[
oC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260615150623_SeedingForUsers.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Migrations #
{		 
public 

partial 
class 
SeedingForUsers (
:) *
	Migration+ 4
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
. 

DropColumn '
(' (
name 
: 
$str #
,# $
table 
: 
$str !
)! "
;" #
migrationBuilder 
. 

DropColumn '
(' (
name 
: 
$str #
,# $
table 
: 
$str  
)  !
;! "
migrationBuilder 
. 
	AddColumn &
<& '
DateTimeOffset' 5
>5 6
(6 7
name 
: 
$str #
,# $
table 
: 
$str 
, 
type 
: 
$str &
,& '
nullable 
: 
false 
,  
defaultValue 
: 
new !
DateTimeOffset" 0
(0 1
new1 4
DateTime5 =
(= >
$num> ?
,? @
$numA B
,B C
$numD E
,E F
$numG H
,H I
$numJ K
,K L
$numM N
,N O
$numP Q
,Q R
DateTimeKindS _
._ `
Unspecified` k
)k l
,l m
newn q
TimeSpanr z
(z {
$num{ |
,| }
$num~ 
,	 Ä
$num
Å Ç
,
Ç É
$num
Ñ Ö
,
Ö Ü
$num
á à
)
à â
)
â ä
)
ä ã
;
ã å
migrationBuilder 
. 

InsertData '
(' (
table   
:   
$str   
,   
columns!! 
:!! 
new!! 
[!! 
]!! 
{!!  
$str!!! )
,!!) *
$str!!+ 8
,!!8 9
$str!!: A
,!!A B
$str!!C Q
,!!Q R
$str!!S a
,!!a b
$str!!c w
,!!w x
$str!!y 
}
!!Ä Å
,
!!Å Ç
values"" 
:"" 
new"" 
object"" "
[""" #
,""# $
]""$ %
{## 
{$$ 
$num$$ 
,$$ 
new$$ 
DateTimeOffset$$ +
($$+ ,
new$$, /
DateTime$$0 8
($$8 9
$num$$9 =
,$$= >
$num$$? @
,$$@ A
$num$$B C
,$$C D
$num$$E F
,$$F G
$num$$H I
,$$I J
$num$$K L
,$$L M
$num$$N O
,$$O P
DateTimeKind$$Q ]
.$$] ^
Unspecified$$^ i
)$$i j
,$$j k
new$$l o
TimeSpan$$p x
($$x y
$num$$y z
,$$z {
$num$$| }
,$$} ~
$num	$$ Ä
,
$$Ä Å
$num
$$Ç É
,
$$É Ñ
$num
$$Ö Ü
)
$$Ü á
)
$$á à
,
$$à â
$str
$$ä ö
,
$$ö õ
$str
$$ú ™
,
$$™ ´
null
$$¨ ∞
,
$$∞ ±
new
$$≤ µ
DateTimeOffset
$$∂ ƒ
(
$$ƒ ≈
new
$$≈ »
DateTime
$$… —
(
$$— “
$num
$$“ ”
,
$$” ‘
$num
$$’ ÷
,
$$÷ ◊
$num
$$ÿ Ÿ
,
$$Ÿ ⁄
$num
$$€ ‹
,
$$‹ ›
$num
$$ﬁ ﬂ
,
$$ﬂ ‡
$num
$$· ‚
,
$$‚ „
$num
$$‰ Â
,
$$Â Ê
DateTimeKind
$$Á Û
.
$$Û Ù
Unspecified
$$Ù ˇ
)
$$ˇ Ä
,
$$Ä Å
new
$$Ç Ö
TimeSpan
$$Ü é
(
$$é è
$num
$$è ê
,
$$ê ë
$num
$$í ì
,
$$ì î
$num
$$ï ñ
,
$$ñ ó
$num
$$ò ô
,
$$ô ö
$num
$$õ ú
)
$$ú ù
)
$$ù û
,
$$û ü
$str
$$† ß
}
$$® ©
,
$$© ™
{%% 
$num%% 
,%% 
new%% 
DateTimeOffset%% +
(%%+ ,
new%%, /
DateTime%%0 8
(%%8 9
$num%%9 =
,%%= >
$num%%? @
,%%@ A
$num%%B C
,%%C D
$num%%E F
,%%F G
$num%%H I
,%%I J
$num%%K L
,%%L M
$num%%N O
,%%O P
DateTimeKind%%Q ]
.%%] ^
Unspecified%%^ i
)%%i j
,%%j k
new%%l o
TimeSpan%%p x
(%%x y
$num%%y z
,%%z {
$num%%| }
,%%} ~
$num	%% Ä
,
%%Ä Å
$num
%%Ç É
,
%%É Ñ
$num
%%Ö Ü
)
%%Ü á
)
%%á à
,
%%à â
$str
%%ä ú
,
%%ú ù
$str
%%û Æ
,
%%Æ Ø
null
%%∞ ¥
,
%%¥ µ
new
%%∂ π
DateTimeOffset
%%∫ »
(
%%» …
new
%%… Ã
DateTime
%%Õ ’
(
%%’ ÷
$num
%%÷ ◊
,
%%◊ ÿ
$num
%%Ÿ ⁄
,
%%⁄ €
$num
%%‹ ›
,
%%› ﬁ
$num
%%ﬂ ‡
,
%%‡ ·
$num
%%‚ „
,
%%„ ‰
$num
%%Â Ê
,
%%Ê Á
$num
%%Ë È
,
%%È Í
DateTimeKind
%%Î ˜
.
%%˜ ¯
Unspecified
%%¯ É
)
%%É Ñ
,
%%Ñ Ö
new
%%Ü â
TimeSpan
%%ä í
(
%%í ì
$num
%%ì î
,
%%î ï
$num
%%ñ ó
,
%%ó ò
$num
%%ô ö
,
%%ö õ
$num
%%ú ù
,
%%ù û
$num
%%ü †
)
%%† °
)
%%° ¢
,
%%¢ £
$str
%%§ ≠
}
%%Æ Ø
,
%%Ø ∞
{&& 
$num&& 
,&& 
new&& 
DateTimeOffset&& +
(&&+ ,
new&&, /
DateTime&&0 8
(&&8 9
$num&&9 =
,&&= >
$num&&? @
,&&@ A
$num&&B C
,&&C D
$num&&E F
,&&F G
$num&&H I
,&&I J
$num&&K L
,&&L M
$num&&N O
,&&O P
DateTimeKind&&Q ]
.&&] ^
Unspecified&&^ i
)&&i j
,&&j k
new&&l o
TimeSpan&&p x
(&&x y
$num&&y z
,&&z {
$num&&| }
,&&} ~
$num	&& Ä
,
&&Ä Å
$num
&&Ç É
,
&&É Ñ
$num
&&Ö Ü
)
&&Ü á
)
&&á à
,
&&à â
$str
&&ä ú
,
&&ú ù
$str
&&û Æ
,
&&Æ Ø
null
&&∞ ¥
,
&&¥ µ
new
&&∂ π
DateTimeOffset
&&∫ »
(
&&» …
new
&&… Ã
DateTime
&&Õ ’
(
&&’ ÷
$num
&&÷ ◊
,
&&◊ ÿ
$num
&&Ÿ ⁄
,
&&⁄ €
$num
&&‹ ›
,
&&› ﬁ
$num
&&ﬂ ‡
,
&&‡ ·
$num
&&‚ „
,
&&„ ‰
$num
&&Â Ê
,
&&Ê Á
$num
&&Ë È
,
&&È Í
DateTimeKind
&&Î ˜
.
&&˜ ¯
Unspecified
&&¯ É
)
&&É Ñ
,
&&Ñ Ö
new
&&Ü â
TimeSpan
&&ä í
(
&&í ì
$num
&&ì î
,
&&î ï
$num
&&ñ ó
,
&&ó ò
$num
&&ô ö
,
&&ö õ
$num
&&ú ù
,
&&ù û
$num
&&ü †
)
&&† °
)
&&° ¢
,
&&¢ £
$str
&&§ ¨
}
&&≠ Æ
}'' 
)'' 
;'' 
}(( 	
	protected++ 
override++ 
void++ 
Down++  $
(++$ %
MigrationBuilder++% 5
migrationBuilder++6 F
)++F G
{,, 	
migrationBuilder-- 
.-- 

DeleteData-- '
(--' (
table.. 
:.. 
$str.. 
,.. 
	keyColumn// 
:// 
$str// #
,//# $
keyValue00 
:00 
$num00 
)00 
;00 
migrationBuilder22 
.22 

DeleteData22 '
(22' (
table33 
:33 
$str33 
,33 
	keyColumn44 
:44 
$str44 #
,44# $
keyValue55 
:55 
$num55 
)55 
;55 
migrationBuilder77 
.77 

DeleteData77 '
(77' (
table88 
:88 
$str88 
,88 
	keyColumn99 
:99 
$str99 #
,99# $
keyValue:: 
::: 
$num:: 
):: 
;:: 
migrationBuilder<< 
.<< 

DropColumn<< '
(<<' (
name== 
:== 
$str== #
,==# $
table>> 
:>> 
$str>> 
)>> 
;>>  
migrationBuilder@@ 
.@@ 
	AddColumn@@ &
<@@& '
DateTimeOffset@@' 5
>@@5 6
(@@6 7
nameAA 
:AA 
$strAA #
,AA# $
tableBB 
:BB 
$strBB !
,BB! "
typeCC 
:CC 
$strCC &
,CC& '
nullableDD 
:DD 
falseDD 
,DD  
defaultValueEE 
:EE 
newEE !
DateTimeOffsetEE" 0
(EE0 1
newEE1 4
DateTimeEE5 =
(EE= >
$numEE> ?
,EE? @
$numEEA B
,EEB C
$numEED E
,EEE F
$numEEG H
,EEH I
$numEEJ K
,EEK L
$numEEM N
,EEN O
$numEEP Q
,EEQ R
DateTimeKindEES _
.EE_ `
UnspecifiedEE` k
)EEk l
,EEl m
newEEn q
TimeSpanEEr z
(EEz {
$numEE{ |
,EE| }
$numEE~ 
,	EE Ä
$num
EEÅ Ç
,
EEÇ É
$num
EEÑ Ö
,
EEÖ Ü
$num
EEá à
)
EEà â
)
EEâ ä
)
EEä ã
;
EEã å
migrationBuilderGG 
.GG 
	AddColumnGG &
<GG& '
DateTimeOffsetGG' 5
>GG5 6
(GG6 7
nameHH 
:HH 
$strHH #
,HH# $
tableII 
:II 
$strII  
,II  !
typeJJ 
:JJ 
$strJJ &
,JJ& '
nullableKK 
:KK 
falseKK 
,KK  
defaultValueLL 
:LL 
newLL !
DateTimeOffsetLL" 0
(LL0 1
newLL1 4
DateTimeLL5 =
(LL= >
$numLL> ?
,LL? @
$numLLA B
,LLB C
$numLLD E
,LLE F
$numLLG H
,LLH I
$numLLJ K
,LLK L
$numLLM N
,LLN O
$numLLP Q
,LLQ R
DateTimeKindLLS _
.LL_ `
UnspecifiedLL` k
)LLk l
,LLl m
newLLn q
TimeSpanLLr z
(LLz {
$numLL{ |
,LL| }
$numLL~ 
,	LL Ä
$num
LLÅ Ç
,
LLÇ É
$num
LLÑ Ö
,
LLÖ Ü
$num
LLá à
)
LLà â
)
LLâ ä
)
LLä ã
;
LLã å
}MM 	
}NN 
}OO Ø
gC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Middleware\GlobalExceptionHandler.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Middleware #
{ 
public 

class "
GlobalExceptionHandler '
:( )
IExceptionHandler* ;
{ 
private		 
ILogger		 
<		 "
GlobalExceptionHandler		 .
>		. /
_logger		0 7
;		7 8
public "
GlobalExceptionHandler %
(% &
ILogger& -
<- ."
GlobalExceptionHandler. D
>D E
loggerE K
)K L
{ 	
_logger 
= 
logger 
; 
} 	
public 
async 
	ValueTask 
< 
bool #
># $
TryHandleAsync% 3
(3 4
HttpContext4 ?
httpContext@ K
,K L
	ExceptionM V
	exceptionW `
,` a
CancellationTokenb s
cancellationToken	t Ö
)
Ö Ü
{ 	
_logger 
. 
LogError 
( 
	exception &
,& '
$str( O
,O P
	exceptionQ Z
.Z [
Message[ b
)b c
;c d
var 
( 

statusCode 
, 
message $
)$ %
=& '
	exception( 1
switch2 8
{ $
PatientNotFoundException (
=>) +
(, -
StatusCodes- 8
.8 9
Status404NotFound9 J
,J K
	exceptionL U
.U V
MessageV ]
)] ^
,^ _#
DoctorNotFoundException '
=>( *
(+ ,
StatusCodes, 7
.7 8
Status404NotFound8 I
,I J
	exceptionK T
.T U
MessageU \
)\ ]
,] ^(
AppointmentNotFoundException ,
=>- /
(0 1
StatusCodes1 <
.< =
Status404NotFound= N
,N O
	exceptionP Y
.Y Z
MessageZ a
)a b
,b c)
HealthRecordNotFoundException -
=>. 0
(1 2
StatusCodes2 =
.= >
Status404NotFound> O
,O P
	exceptionQ Z
.Z [
Message[ b
)b c
,c d
_ 
=> 
( 
StatusCodes !
.! "(
Status500InternalServerError" >
,> ?
$str@ W
)W X
} 
; 
var   
response   
=   
new   
ErrorResponse   ,
{!! 

StatusCode"" 
="" 

statusCode"" '
,""' (
Message## 
=## 
message## !
,##! "
	TimeStamp$$ 
=$$ 
DateTime$$ $
.$$$ %
UtcNow$$% +
,$$+ ,
Path%% 
=%% 
httpContext%% "
.%%" #
Request%%# *
.%%* +
Path%%+ /
}&& 
;&& 
httpContext(( 
.(( 
Response((  
.((  !

StatusCode((! +
=((, -

statusCode((. 8
;((8 9
await** 
httpContext** 
.** 
Response** &
.**& '
WriteAsJsonAsync**' 7
(**7 8
response**8 @
)**@ A
;**A B
return,, 
true,, 
;,, 
}-- 	
}// 
}00 Èê
{C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260615143933_InitialWithAllNavigationSet.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Migrations #
{ 
public		 

partial		 
class		 '
InitialWithAllNavigationSet		 4
:		5 6
	Migration		7 @
{

 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str 
, 
columns 
: 
table 
=> !
new" %
{ 
UserId 
= 
table "
." #
Column# )
<) *
int* -
>- .
(. /
type/ 3
:3 4
$str5 :
,: ;
nullable< D
:D E
falseF K
)K L
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
Email 
= 
table !
.! "
Column" (
<( )
string) /
>/ 0
(0 1
type1 5
:5 6
$str7 F
,F G
nullableH P
:P Q
falseR W
)W X
,X Y
Role 
= 
table  
.  !
Column! '
<' (
string( .
>. /
(/ 0
type0 4
:4 5
$str6 D
,D E
	maxLengthF O
:O P
$numQ S
,S T
nullableU ]
:] ^
false_ d
)d e
,e f
PasswordHash  
=! "
table# (
.( )
Column) /
</ 0
string0 6
>6 7
(7 8
type8 <
:< =
$str> M
,M N
	maxLengthO X
:X Y
$numZ ]
,] ^
nullable_ g
:g h
falsei n
)n o
,o p
RefreshToken  
=! "
table# (
.( )
Column) /
</ 0
string0 6
>6 7
(7 8
type8 <
:< =
$str> M
,M N
	maxLengthO X
:X Y
$numZ ]
,] ^
nullable_ g
:g h
truei m
)m n
,n o
RefreshTokenExpiry &
=' (
table) .
.. /
Column/ 5
<5 6
DateTimeOffset6 D
>D E
(E F
typeF J
:J K
$strL \
,\ ]
nullable^ f
:f g
falseh m
)m n
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% /
,/ 0
x1 2
=>3 5
x6 7
.7 8
UserId8 >
)> ?
;? @
} 
) 
; 
migrationBuilder 
. 
CreateTable (
(( )
name   
:   
$str   
,    
columns!! 
:!! 
table!! 
=>!! !
new!!" %
{"" 
DoctorId## 
=## 
table## $
.##$ %
Column##% +
<##+ ,
int##, /
>##/ 0
(##0 1
type##1 5
:##5 6
$str##7 <
,##< =
nullable##> F
:##F G
false##H M
)##M N
.$$ 

Annotation$$ #
($$# $
$str$$$ 8
,$$8 9
$str$$: @
)$$@ A
,$$A B
UserId%% 
=%% 
table%% "
.%%" #
Column%%# )
<%%) *
int%%* -
>%%- .
(%%. /
type%%/ 3
:%%3 4
$str%%5 :
,%%: ;
nullable%%< D
:%%D E
false%%F K
)%%K L
,%%L M
FullName&& 
=&& 
table&& $
.&&$ %
Column&&% +
<&&+ ,
string&&, 2
>&&2 3
(&&3 4
type&&4 8
:&&8 9
$str&&: I
,&&I J
	maxLength&&K T
:&&T U
$num&&V Y
,&&Y Z
nullable&&[ c
:&&c d
false&&e j
)&&j k
,&&k l
Specialisation'' "
=''# $
table''% *
.''* +
Column''+ 1
<''1 2
string''2 8
>''8 9
(''9 :
type'': >
:''> ?
$str''@ N
,''N O
	maxLength''P Y
:''Y Z
$num''[ ]
,''] ^
nullable''_ g
:''g h
false''i n
)''n o
,''o p
YearsOfExperience(( %
=((& '
table((( -
.((- .
Column((. 4
<((4 5
int((5 8
>((8 9
(((9 :
type((: >
:((> ?
$str((@ E
,((E F
nullable((G O
:((O P
false((Q V
)((V W
,((W X
ConsultationFee)) #
=))$ %
table))& +
.))+ ,
Column)), 2
<))2 3
decimal))3 :
>)): ;
()); <
type))< @
:))@ A
$str))B Q
,))Q R
nullable))S [
:))[ \
false))] b
)))b c
,))c d
IsActive** 
=** 
table** $
.**$ %
Column**% +
<**+ ,
bool**, 0
>**0 1
(**1 2
type**2 6
:**6 7
$str**8 =
,**= >
nullable**? G
:**G H
false**I N
)**N O
,**O P
CreatedDate++ 
=++  !
table++" '
.++' (
Column++( .
<++. /
DateTimeOffset++/ =
>++= >
(++> ?
type++? C
:++C D
$str++E U
,++U V
nullable++W _
:++_ `
false++a f
)++f g
},, 
,,, 
constraints-- 
:-- 
table-- "
=>--# %
{.. 
table// 
.// 

PrimaryKey// $
(//$ %
$str//% 1
,//1 2
x//3 4
=>//5 7
x//8 9
.//9 :
DoctorId//: B
)//B C
;//C D
table00 
.00 

ForeignKey00 $
(00$ %
name11 
:11 
$str11 7
,117 8
column22 
:22 
x22  !
=>22" $
x22% &
.22& '
UserId22' -
,22- .
principalTable33 &
:33& '
$str33( /
,33/ 0
principalColumn44 '
:44' (
$str44) 1
,441 2
onDelete55  
:55  !
ReferentialAction55" 3
.553 4
Cascade554 ;
)55; <
;55< =
}66 
)66 
;66 
migrationBuilder88 
.88 
CreateTable88 (
(88( )
name99 
:99 
$str99  
,99  !
columns:: 
::: 
table:: 
=>:: !
new::" %
{;; 
	PatientId<< 
=<< 
table<<  %
.<<% &
Column<<& ,
<<<, -
int<<- 0
><<0 1
(<<1 2
type<<2 6
:<<6 7
$str<<8 =
,<<= >
nullable<<? G
:<<G H
false<<I N
)<<N O
.== 

Annotation== #
(==# $
$str==$ 8
,==8 9
$str==: @
)==@ A
,==A B
UserId>> 
=>> 
table>> "
.>>" #
Column>># )
<>>) *
int>>* -
>>>- .
(>>. /
type>>/ 3
:>>3 4
$str>>5 :
,>>: ;
nullable>>< D
:>>D E
false>>F K
)>>K L
,>>L M
FullName?? 
=?? 
table?? $
.??$ %
Column??% +
<??+ ,
string??, 2
>??2 3
(??3 4
type??4 8
:??8 9
$str??: I
,??I J
nullable??K S
:??S T
true??U Y
)??Y Z
,??Z [
DateOfBirth@@ 
=@@  !
table@@" '
.@@' (
Column@@( .
<@@. /
DateOnly@@/ 7
>@@7 8
(@@8 9
type@@9 =
:@@= >
$str@@? E
,@@E F
nullable@@G O
:@@O P
false@@Q V
)@@V W
,@@W X
GenderAA 
=AA 
tableAA "
.AA" #
ColumnAA# )
<AA) *
stringAA* 0
>AA0 1
(AA1 2
typeAA2 6
:AA6 7
$strAA8 G
,AAG H
nullableAAI Q
:AAQ R
trueAAS W
)AAW X
,AAX Y
PhoneNumberBB 
=BB  !
tableBB" '
.BB' (
ColumnBB( .
<BB. /
stringBB/ 5
>BB5 6
(BB6 7
typeBB7 ;
:BB; <
$strBB= L
,BBL M
nullableBBN V
:BBV W
trueBBX \
)BB\ ]
,BB] ^
InsuranceIdCC 
=CC  !
tableCC" '
.CC' (
ColumnCC( .
<CC. /
stringCC/ 5
>CC5 6
(CC6 7
typeCC7 ;
:CC; <
$strCC= L
,CCL M
nullableCCN V
:CCV W
trueCCX \
)CC\ ]
,CC] ^
CreatedDateDD 
=DD  !
tableDD" '
.DD' (
ColumnDD( .
<DD. /
DateTimeOffsetDD/ =
>DD= >
(DD> ?
typeDD? C
:DDC D
$strDDE U
,DDU V
nullableDDW _
:DD_ `
falseDDa f
)DDf g
,DDg h
	IsActivedEE 
=EE 
tableEE  %
.EE% &
ColumnEE& ,
<EE, -
boolEE- 1
>EE1 2
(EE2 3
typeEE3 7
:EE7 8
$strEE9 >
,EE> ?
nullableEE@ H
:EEH I
falseEEJ O
)EEO P
}FF 
,FF 
constraintsGG 
:GG 
tableGG "
=>GG# %
{HH 
tableII 
.II 

PrimaryKeyII $
(II$ %
$strII% 2
,II2 3
xII4 5
=>II6 8
xII9 :
.II: ;
	PatientIdII; D
)IID E
;IIE F
tableJJ 
.JJ 

ForeignKeyJJ $
(JJ$ %
nameKK 
:KK 
$strKK 8
,KK8 9
columnLL 
:LL 
xLL  !
=>LL" $
xLL% &
.LL& '
UserIdLL' -
,LL- .
principalTableMM &
:MM& '
$strMM( /
,MM/ 0
principalColumnNN '
:NN' (
$strNN) 1
,NN1 2
onDeleteOO  
:OO  !
ReferentialActionOO" 3
.OO3 4
CascadeOO4 ;
)OO; <
;OO< =
}PP 
)PP 
;PP 
migrationBuilderRR 
.RR 
CreateTableRR (
(RR( )
nameSS 
:SS 
$strSS &
,SS& '
columnsTT 
:TT 
tableTT 
=>TT !
newTT" %
{UU 
IdVV 
=VV 
tableVV 
.VV 
ColumnVV %
<VV% &
intVV& )
>VV) *
(VV* +
typeVV+ /
:VV/ 0
$strVV1 6
,VV6 7
nullableVV8 @
:VV@ A
falseVVB G
)VVG H
.WW 

AnnotationWW #
(WW# $
$strWW$ 8
,WW8 9
$strWW: @
)WW@ A
,WWA B
DoctorIdXX 
=XX 
tableXX $
.XX$ %
ColumnXX% +
<XX+ ,
intXX, /
>XX/ 0
(XX0 1
typeXX1 5
:XX5 6
$strXX7 <
,XX< =
nullableXX> F
:XXF G
falseXXH M
)XXM N
,XXN O
TimeSlotYY 
=YY 
tableYY $
.YY$ %
ColumnYY% +
<YY+ ,
stringYY, 2
>YY2 3
(YY3 4
typeYY4 8
:YY8 9
$strYY: H
,YYH I
	maxLengthYYJ S
:YYS T
$numYYU W
,YYW X
nullableYYY a
:YYa b
falseYYc h
)YYh i
,YYi j
CreatedDateZZ 
=ZZ  !
tableZZ" '
.ZZ' (
ColumnZZ( .
<ZZ. /
DateTimeOffsetZZ/ =
>ZZ= >
(ZZ> ?
typeZZ? C
:ZZC D
$strZZE U
,ZZU V
nullableZZW _
:ZZ_ `
falseZZa f
)ZZf g
}[[ 
,[[ 
constraints\\ 
:\\ 
table\\ "
=>\\# %
{]] 
table^^ 
.^^ 

PrimaryKey^^ $
(^^$ %
$str^^% 8
,^^8 9
x^^: ;
=>^^< >
x^^? @
.^^@ A
Id^^A C
)^^C D
;^^D E
table__ 
.__ 

ForeignKey__ $
(__$ %
name`` 
:`` 
$str`` B
,``B C
columnaa 
:aa 
xaa  !
=>aa" $
xaa% &
.aa& '
DoctorIdaa' /
,aa/ 0
principalTablebb &
:bb& '
$strbb( 1
,bb1 2
principalColumncc '
:cc' (
$strcc) 3
,cc3 4
onDeletedd  
:dd  !
ReferentialActiondd" 3
.dd3 4
Cascadedd4 ;
)dd; <
;dd< =
}ee 
)ee 
;ee 
migrationBuildergg 
.gg 
CreateTablegg (
(gg( )
namehh 
:hh 
$strhh $
,hh$ %
columnsii 
:ii 
tableii 
=>ii !
newii" %
{jj 
Idkk 
=kk 
tablekk 
.kk 
Columnkk %
<kk% &
intkk& )
>kk) *
(kk* +
typekk+ /
:kk/ 0
$strkk1 6
,kk6 7
nullablekk8 @
:kk@ A
falsekkB G
)kkG H
.ll 

Annotationll #
(ll# $
$strll$ 8
,ll8 9
$strll: @
)ll@ A
,llA B
DoctorIdmm 
=mm 
tablemm $
.mm$ %
Columnmm% +
<mm+ ,
intmm, /
>mm/ 0
(mm0 1
typemm1 5
:mm5 6
$strmm7 <
,mm< =
nullablemm> F
:mmF G
falsemmH M
)mmM N
,mmN O
	LeaveDatenn 
=nn 
tablenn  %
.nn% &
Columnnn& ,
<nn, -
DateOnlynn- 5
>nn5 6
(nn6 7
typenn7 ;
:nn; <
$strnn= C
,nnC D
nullablennE M
:nnM N
falsennO T
)nnT U
,nnU V
Reasonoo 
=oo 
tableoo "
.oo" #
Columnoo# )
<oo) *
stringoo* 0
>oo0 1
(oo1 2
typeoo2 6
:oo6 7
$stroo8 G
,ooG H
	maxLengthooI R
:ooR S
$numooT W
,ooW X
nullableooY a
:ooa b
trueooc g
)oog h
,ooh i

CreateDatepp 
=pp  
tablepp! &
.pp& '
Columnpp' -
<pp- .
DateTimeOffsetpp. <
>pp< =
(pp= >
typepp> B
:ppB C
$strppD T
,ppT U
nullableppV ^
:pp^ _
falsepp` e
)ppe f
}qq 
,qq 
constraintsrr 
:rr 
tablerr "
=>rr# %
{ss 
tablett 
.tt 

PrimaryKeytt $
(tt$ %
$strtt% 6
,tt6 7
xtt8 9
=>tt: <
xtt= >
.tt> ?
Idtt? A
)ttA B
;ttB C
tableuu 
.uu 

ForeignKeyuu $
(uu$ %
namevv 
:vv 
$strvv @
,vv@ A
columnww 
:ww 
xww  !
=>ww" $
xww% &
.ww& '
DoctorIdww' /
,ww/ 0
principalTablexx &
:xx& '
$strxx( 1
,xx1 2
principalColumnyy '
:yy' (
$stryy) 3
,yy3 4
onDeletezz  
:zz  !
ReferentialActionzz" 3
.zz3 4
Cascadezz4 ;
)zz; <
;zz< =
}{{ 
){{ 
;{{ 
migrationBuilder}} 
.}} 
CreateTable}} (
(}}( )
name~~ 
:~~ 
$str~~ $
,~~$ %
columns 
: 
table 
=> !
new" %
{
ÄÄ 
AppointmentId
ÅÅ !
=
ÅÅ" #
table
ÅÅ$ )
.
ÅÅ) *
Column
ÅÅ* 0
<
ÅÅ0 1
int
ÅÅ1 4
>
ÅÅ4 5
(
ÅÅ5 6
type
ÅÅ6 :
:
ÅÅ: ;
$str
ÅÅ< A
,
ÅÅA B
nullable
ÅÅC K
:
ÅÅK L
false
ÅÅM R
)
ÅÅR S
.
ÇÇ 

Annotation
ÇÇ #
(
ÇÇ# $
$str
ÇÇ$ 8
,
ÇÇ8 9
$str
ÇÇ: @
)
ÇÇ@ A
,
ÇÇA B
	PatientId
ÉÉ 
=
ÉÉ 
table
ÉÉ  %
.
ÉÉ% &
Column
ÉÉ& ,
<
ÉÉ, -
int
ÉÉ- 0
>
ÉÉ0 1
(
ÉÉ1 2
type
ÉÉ2 6
:
ÉÉ6 7
$str
ÉÉ8 =
,
ÉÉ= >
nullable
ÉÉ? G
:
ÉÉG H
false
ÉÉI N
)
ÉÉN O
,
ÉÉO P
DoctorId
ÑÑ 
=
ÑÑ 
table
ÑÑ $
.
ÑÑ$ %
Column
ÑÑ% +
<
ÑÑ+ ,
int
ÑÑ, /
>
ÑÑ/ 0
(
ÑÑ0 1
type
ÑÑ1 5
:
ÑÑ5 6
$str
ÑÑ7 <
,
ÑÑ< =
nullable
ÑÑ> F
:
ÑÑF G
false
ÑÑH M
)
ÑÑM N
,
ÑÑN O
ScheduledDate
ÖÖ !
=
ÖÖ" #
table
ÖÖ$ )
.
ÖÖ) *
Column
ÖÖ* 0
<
ÖÖ0 1
DateOnly
ÖÖ1 9
>
ÖÖ9 :
(
ÖÖ: ;
type
ÖÖ; ?
:
ÖÖ? @
$str
ÖÖA G
,
ÖÖG H
nullable
ÖÖI Q
:
ÖÖQ R
false
ÖÖS X
)
ÖÖX Y
,
ÖÖY Z
TimeSlot
ÜÜ 
=
ÜÜ 
table
ÜÜ $
.
ÜÜ$ %
Column
ÜÜ% +
<
ÜÜ+ ,
string
ÜÜ, 2
>
ÜÜ2 3
(
ÜÜ3 4
type
ÜÜ4 8
:
ÜÜ8 9
$str
ÜÜ: H
,
ÜÜH I
	maxLength
ÜÜJ S
:
ÜÜS T
$num
ÜÜU W
,
ÜÜW X
nullable
ÜÜY a
:
ÜÜa b
false
ÜÜc h
)
ÜÜh i
,
ÜÜi j
Status
áá 
=
áá 
table
áá "
.
áá" #
Column
áá# )
<
áá) *
string
áá* 0
>
áá0 1
(
áá1 2
type
áá2 6
:
áá6 7
$str
áá8 F
,
ááF G
	maxLength
ááH Q
:
ááQ R
$num
ááS U
,
ááU V
nullable
ááW _
:
áá_ `
false
ááa f
)
ááf g
,
áág h 
CancellationReason
àà &
=
àà' (
table
àà) .
.
àà. /
Column
àà/ 5
<
àà5 6
string
àà6 <
>
àà< =
(
àà= >
type
àà> B
:
ààB C
$str
ààD S
,
ààS T
	maxLength
ààU ^
:
àà^ _
$num
àà` c
,
ààc d
nullable
ààe m
:
ààm n
true
àào s
)
ààs t
,
ààt u
CreatedDate
ââ 
=
ââ  !
table
ââ" '
.
ââ' (
Column
ââ( .
<
ââ. /
DateTimeOffset
ââ/ =
>
ââ= >
(
ââ> ?
type
ââ? C
:
ââC D
$str
ââE U
,
ââU V
nullable
ââW _
:
ââ_ `
false
ââa f
)
ââf g
,
ââg h
UserId
ää 
=
ää 
table
ää "
.
ää" #
Column
ää# )
<
ää) *
int
ää* -
>
ää- .
(
ää. /
type
ää/ 3
:
ää3 4
$str
ää5 :
,
ää: ;
nullable
ää< D
:
ääD E
false
ääF K
)
ääK L
}
ãã 
,
ãã 
constraints
åå 
:
åå 
table
åå "
=>
åå# %
{
çç 
table
éé 
.
éé 

PrimaryKey
éé $
(
éé$ %
$str
éé% 6
,
éé6 7
x
éé8 9
=>
éé: <
x
éé= >
.
éé> ?
AppointmentId
éé? L
)
ééL M
;
ééM N
table
èè 
.
èè 

ForeignKey
èè $
(
èè$ %
name
êê 
:
êê 
$str
êê @
,
êê@ A
column
ëë 
:
ëë 
x
ëë  !
=>
ëë" $
x
ëë% &
.
ëë& '
DoctorId
ëë' /
,
ëë/ 0
principalTable
íí &
:
íí& '
$str
íí( 1
,
íí1 2
principalColumn
ìì '
:
ìì' (
$str
ìì) 3
,
ìì3 4
onDelete
îî  
:
îî  !
ReferentialAction
îî" 3
.
îî3 4
Restrict
îî4 <
)
îî< =
;
îî= >
table
ïï 
.
ïï 

ForeignKey
ïï $
(
ïï$ %
name
ññ 
:
ññ 
$str
ññ B
,
ññB C
column
óó 
:
óó 
x
óó  !
=>
óó" $
x
óó% &
.
óó& '
	PatientId
óó' 0
,
óó0 1
principalTable
òò &
:
òò& '
$str
òò( 2
,
òò2 3
principalColumn
ôô '
:
ôô' (
$str
ôô) 4
,
ôô4 5
onDelete
öö  
:
öö  !
ReferentialAction
öö" 3
.
öö3 4
Restrict
öö4 <
)
öö< =
;
öö= >
}
õõ 
)
õõ 
;
õõ 
migrationBuilder
ùù 
.
ùù 
CreateTable
ùù (
(
ùù( )
name
ûû 
:
ûû 
$str
ûû %
,
ûû% &
columns
üü 
:
üü 
table
üü 
=>
üü !
new
üü" %
{
†† 
RecordId
°° 
=
°° 
table
°° $
.
°°$ %
Column
°°% +
<
°°+ ,
int
°°, /
>
°°/ 0
(
°°0 1
type
°°1 5
:
°°5 6
$str
°°7 <
,
°°< =
nullable
°°> F
:
°°F G
false
°°H M
)
°°M N
.
¢¢ 

Annotation
¢¢ #
(
¢¢# $
$str
¢¢$ 8
,
¢¢8 9
$str
¢¢: @
)
¢¢@ A
,
¢¢A B
AppointmentId
££ !
=
££" #
table
££$ )
.
££) *
Column
££* 0
<
££0 1
int
££1 4
>
££4 5
(
££5 6
type
££6 :
:
££: ;
$str
££< A
,
££A B
nullable
££C K
:
££K L
false
££M R
)
££R S
,
££S T
	PatientId
§§ 
=
§§ 
table
§§  %
.
§§% &
Column
§§& ,
<
§§, -
int
§§- 0
>
§§0 1
(
§§1 2
type
§§2 6
:
§§6 7
$str
§§8 =
,
§§= >
nullable
§§? G
:
§§G H
false
§§I N
)
§§N O
,
§§O P
DoctorId
•• 
=
•• 
table
•• $
.
••$ %
Column
••% +
<
••+ ,
int
••, /
>
••/ 0
(
••0 1
type
••1 5
:
••5 6
$str
••7 <
,
••< =
nullable
••> F
:
••F G
false
••H M
)
••M N
,
••N O
	VisitDate
¶¶ 
=
¶¶ 
table
¶¶  %
.
¶¶% &
Column
¶¶& ,
<
¶¶, -
DateTime
¶¶- 5
>
¶¶5 6
(
¶¶6 7
type
¶¶7 ;
:
¶¶; <
$str
¶¶= H
,
¶¶H I
nullable
¶¶J R
:
¶¶R S
false
¶¶T Y
)
¶¶Y Z
,
¶¶Z [
	Diagnosis
ßß 
=
ßß 
table
ßß  %
.
ßß% &
Column
ßß& ,
<
ßß, -
string
ßß- 3
>
ßß3 4
(
ßß4 5
type
ßß5 9
:
ßß9 :
$str
ßß; J
,
ßßJ K
	maxLength
ßßL U
:
ßßU V
$num
ßßW Z
,
ßßZ [
nullable
ßß\ d
:
ßßd e
false
ßßf k
)
ßßk l
,
ßßl m
Prescription
®®  
=
®®! "
table
®®# (
.
®®( )
Column
®®) /
<
®®/ 0
string
®®0 6
>
®®6 7
(
®®7 8
type
®®8 <
:
®®< =
$str
®®> M
,
®®M N
	maxLength
®®O X
:
®®X Y
$num
®®Z ]
,
®®] ^
nullable
®®_ g
:
®®g h
false
®®i n
)
®®n o
,
®®o p
Notes
©© 
=
©© 
table
©© !
.
©©! "
Column
©©" (
<
©©( )
string
©©) /
>
©©/ 0
(
©©0 1
type
©©1 5
:
©©5 6
$str
©©7 G
,
©©G H
	maxLength
©©I R
:
©©R S
$num
©©T X
,
©©X Y
nullable
©©Z b
:
©©b c
true
©©d h
)
©©h i
,
©©i j
CreatedDate
™™ 
=
™™  !
table
™™" '
.
™™' (
Column
™™( .
<
™™. /
DateTimeOffset
™™/ =
>
™™= >
(
™™> ?
type
™™? C
:
™™C D
$str
™™E U
,
™™U V
nullable
™™W _
:
™™_ `
false
™™a f
)
™™f g
}
´´ 
,
´´ 
constraints
¨¨ 
:
¨¨ 
table
¨¨ "
=>
¨¨# %
{
≠≠ 
table
ÆÆ 
.
ÆÆ 

PrimaryKey
ÆÆ $
(
ÆÆ$ %
$str
ÆÆ% 7
,
ÆÆ7 8
x
ÆÆ9 :
=>
ÆÆ; =
x
ÆÆ> ?
.
ÆÆ? @
RecordId
ÆÆ@ H
)
ÆÆH I
;
ÆÆI J
table
ØØ 
.
ØØ 

ForeignKey
ØØ $
(
ØØ$ %
name
∞∞ 
:
∞∞ 
$str
∞∞ K
,
∞∞K L
column
±± 
:
±± 
x
±±  !
=>
±±" $
x
±±% &
.
±±& '
AppointmentId
±±' 4
,
±±4 5
principalTable
≤≤ &
:
≤≤& '
$str
≤≤( 6
,
≤≤6 7
principalColumn
≥≥ '
:
≥≥' (
$str
≥≥) 8
,
≥≥8 9
onDelete
¥¥  
:
¥¥  !
ReferentialAction
¥¥" 3
.
¥¥3 4
Restrict
¥¥4 <
)
¥¥< =
;
¥¥= >
table
µµ 
.
µµ 

ForeignKey
µµ $
(
µµ$ %
name
∂∂ 
:
∂∂ 
$str
∂∂ A
,
∂∂A B
column
∑∑ 
:
∑∑ 
x
∑∑  !
=>
∑∑" $
x
∑∑% &
.
∑∑& '
DoctorId
∑∑' /
,
∑∑/ 0
principalTable
∏∏ &
:
∏∏& '
$str
∏∏( 1
,
∏∏1 2
principalColumn
ππ '
:
ππ' (
$str
ππ) 3
,
ππ3 4
onDelete
∫∫  
:
∫∫  !
ReferentialAction
∫∫" 3
.
∫∫3 4
Restrict
∫∫4 <
)
∫∫< =
;
∫∫= >
table
ªª 
.
ªª 

ForeignKey
ªª $
(
ªª$ %
name
ºº 
:
ºº 
$str
ºº C
,
ººC D
column
ΩΩ 
:
ΩΩ 
x
ΩΩ  !
=>
ΩΩ" $
x
ΩΩ% &
.
ΩΩ& '
	PatientId
ΩΩ' 0
,
ΩΩ0 1
principalTable
ææ &
:
ææ& '
$str
ææ( 2
,
ææ2 3
principalColumn
øø '
:
øø' (
$str
øø) 4
,
øø4 5
onDelete
¿¿  
:
¿¿  !
ReferentialAction
¿¿" 3
.
¿¿3 4
Restrict
¿¿4 <
)
¿¿< =
;
¿¿= >
}
¡¡ 
)
¡¡ 
;
¡¡ 
migrationBuilder
√√ 
.
√√ 
CreateIndex
√√ (
(
√√( )
name
ƒƒ 
:
ƒƒ 
$str
ƒƒ 3
,
ƒƒ3 4
table
≈≈ 
:
≈≈ 
$str
≈≈ %
,
≈≈% &
columns
∆∆ 
:
∆∆ 
new
∆∆ 
[
∆∆ 
]
∆∆ 
{
∆∆  
$str
∆∆! +
,
∆∆+ ,
$str
∆∆- <
}
∆∆= >
)
∆∆> ?
;
∆∆? @
migrationBuilder
»» 
.
»» 
CreateIndex
»» (
(
»»( )
name
…… 
:
…… 
$str
…… 4
,
……4 5
table
   
:
   
$str
   %
,
  % &
columns
ÀÀ 
:
ÀÀ 
new
ÀÀ 
[
ÀÀ 
]
ÀÀ 
{
ÀÀ  
$str
ÀÀ! ,
,
ÀÀ, -
$str
ÀÀ. =
}
ÀÀ> ?
)
ÀÀ? @
;
ÀÀ@ A
migrationBuilder
ÕÕ 
.
ÕÕ 
CreateIndex
ÕÕ (
(
ÕÕ( )
name
ŒŒ 
:
ŒŒ 
$str
ŒŒ 8
,
ŒŒ8 9
table
œœ 
:
œœ 
$str
œœ %
,
œœ% &
columns
–– 
:
–– 
new
–– 
[
–– 
]
–– 
{
––  
$str
––! +
,
––+ ,
$str
––- <
,
––< =
$str
––> H
}
––I J
,
––J K
unique
—— 
:
—— 
true
—— 
,
—— 
filter
““ 
:
““ 
$str
““ 1
)
““1 2
;
““2 3
migrationBuilder
‘‘ 
.
‘‘ 
CreateIndex
‘‘ (
(
‘‘( )
name
’’ 
:
’’ 
$str
’’ 2
,
’’2 3
table
÷÷ 
:
÷÷ 
$str
÷÷ '
,
÷÷' (
column
◊◊ 
:
◊◊ 
$str
◊◊ "
)
◊◊" #
;
◊◊# $
migrationBuilder
ŸŸ 
.
ŸŸ 
CreateIndex
ŸŸ (
(
ŸŸ( )
name
⁄⁄ 
:
⁄⁄ 
$str
⁄⁄ 0
,
⁄⁄0 1
table
€€ 
:
€€ 
$str
€€ %
,
€€% &
column
‹‹ 
:
‹‹ 
$str
‹‹ "
)
‹‹" #
;
‹‹# $
migrationBuilder
ﬁﬁ 
.
ﬁﬁ 
CreateIndex
ﬁﬁ (
(
ﬁﬁ( )
name
ﬂﬂ 
:
ﬂﬂ 
$str
ﬂﬂ 0
,
ﬂﬂ0 1
table
‡‡ 
:
‡‡ 
$str
‡‡  
,
‡‡  !
column
·· 
:
·· 
$str
·· (
)
··( )
;
··) *
migrationBuilder
„„ 
.
„„ 
CreateIndex
„„ (
(
„„( )
name
‰‰ 
:
‰‰ 
$str
‰‰ )
,
‰‰) *
table
ÂÂ 
:
ÂÂ 
$str
ÂÂ  
,
ÂÂ  !
column
ÊÊ 
:
ÊÊ 
$str
ÊÊ  
,
ÊÊ  !
unique
ÁÁ 
:
ÁÁ 
true
ÁÁ 
)
ÁÁ 
;
ÁÁ 
migrationBuilder
ÈÈ 
.
ÈÈ 
CreateIndex
ÈÈ (
(
ÈÈ( )
name
ÍÍ 
:
ÍÍ 
$str
ÍÍ 6
,
ÍÍ6 7
table
ÎÎ 
:
ÎÎ 
$str
ÎÎ &
,
ÎÎ& '
column
ÏÏ 
:
ÏÏ 
$str
ÏÏ '
,
ÏÏ' (
unique
ÌÌ 
:
ÌÌ 
true
ÌÌ 
)
ÌÌ 
;
ÌÌ 
migrationBuilder
ÔÔ 
.
ÔÔ 
CreateIndex
ÔÔ (
(
ÔÔ( )
name
 
:
 
$str
 1
,
1 2
table
ÒÒ 
:
ÒÒ 
$str
ÒÒ &
,
ÒÒ& '
column
ÚÚ 
:
ÚÚ 
$str
ÚÚ "
)
ÚÚ" #
;
ÚÚ# $
migrationBuilder
ÙÙ 
.
ÙÙ 
CreateIndex
ÙÙ (
(
ÙÙ( )
name
ıı 
:
ıı 
$str
ıı :
,
ıı: ;
table
ˆˆ 
:
ˆˆ 
$str
ˆˆ &
,
ˆˆ& '
columns
˜˜ 
:
˜˜ 
new
˜˜ 
[
˜˜ 
]
˜˜ 
{
˜˜  
$str
˜˜! ,
,
˜˜, -
$str
˜˜. 9
}
˜˜: ;
)
˜˜; <
;
˜˜< =
migrationBuilder
˘˘ 
.
˘˘ 
CreateIndex
˘˘ (
(
˘˘( )
name
˙˙ 
:
˙˙ 
$str
˙˙ *
,
˙˙* +
table
˚˚ 
:
˚˚ 
$str
˚˚ !
,
˚˚! "
column
¸¸ 
:
¸¸ 
$str
¸¸  
,
¸¸  !
unique
˝˝ 
:
˝˝ 
true
˝˝ 
)
˝˝ 
;
˝˝ 
migrationBuilder
ˇˇ 
.
ˇˇ 
CreateIndex
ˇˇ (
(
ˇˇ( )
name
ÄÄ 
:
ÄÄ 
$str
ÄÄ &
,
ÄÄ& '
table
ÅÅ 
:
ÅÅ 
$str
ÅÅ 
,
ÅÅ 
column
ÇÇ 
:
ÇÇ 
$str
ÇÇ 
,
ÇÇ  
unique
ÉÉ 
:
ÉÉ 
true
ÉÉ 
)
ÉÉ 
;
ÉÉ 
}
ÑÑ 	
	protected
áá 
override
áá 
void
áá 
Down
áá  $
(
áá$ %
MigrationBuilder
áá% 5
migrationBuilder
áá6 F
)
ááF G
{
àà 	
migrationBuilder
ââ 
.
ââ 
	DropTable
ââ &
(
ââ& '
name
ää 
:
ää 
$str
ää &
)
ää& '
;
ää' (
migrationBuilder
åå 
.
åå 
	DropTable
åå &
(
åå& '
name
çç 
:
çç 
$str
çç $
)
çç$ %
;
çç% &
migrationBuilder
èè 
.
èè 
	DropTable
èè &
(
èè& '
name
êê 
:
êê 
$str
êê %
)
êê% &
;
êê& '
migrationBuilder
íí 
.
íí 
	DropTable
íí &
(
íí& '
name
ìì 
:
ìì 
$str
ìì $
)
ìì$ %
;
ìì% &
migrationBuilder
ïï 
.
ïï 
	DropTable
ïï &
(
ïï& '
name
ññ 
:
ññ 
$str
ññ 
)
ññ  
;
ññ  !
migrationBuilder
òò 
.
òò 
	DropTable
òò &
(
òò& '
name
ôô 
:
ôô 
$str
ôô  
)
ôô  !
;
ôô! "
migrationBuilder
õõ 
.
õõ 
	DropTable
õõ &
(
õõ& '
name
úú 
:
úú 
$str
úú 
)
úú 
;
úú 
}
ùù 	
}
ûû 
}üü ∞
\C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Mapping\MappingProfile.cs
	namespace		 	

HealthCare		
 
.		 
Api		 
.		 
Mapping		  
{

 
public 

class 
MappingProfile 
:  !
Profile" )
{ 
public 
MappingProfile 
( 
) 
{ 
	CreateMap 
< 
PatientRegisterDto (
,( )
Patient* 1
>1 2
(2 3
)3 4
;4 5
	CreateMap 
< 
UpdatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
PatientListDto $
,$ %
Patient& -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
DoctorRegisterDto '
,' (
Doctor) /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
UpdateDoctorDto %
,% &
Doctor' -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
DoctorListDto #
,# $
Doctor% +
>+ ,
(, -
)- .
;. /
	CreateMap 
<  
CreateAppointmentDto *
,* +
Appointment, 7
>7 8
(8 9
)9 :
;: ;
	CreateMap 
<  
UpdateAppointmentDto *
,* +
Appointment, 7
>7 8
(8 9
)9 :
;: ;
	CreateMap 
< 
AppointmentListDto (
,( )
Appointment* 5
>5 6
(6 7
)7 8
;8 9
	CreateMap## 
<## !
CreateHealthRecordDto## +
,##+ ,
HealthRecord##- 9
>##9 :
(##: ;
)##; <
;##< =
	CreateMap$$ 
<$$ 
UpdateDoctorDto$$ %
,$$% &
HealthRecord$$' 3
>$$3 4
($$4 5
)$$5 6
;$$6 7
	CreateMap%% 
<%% 
HealthRecordListDto%% )
,%%) *
HealthRecord%%+ 7
>%%7 8
(%%8 9
)%%9 :
;%%: ;
}(( 	
}** 
}++ —
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Exceptions\PatientNotFoundException.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Exceptions #
{ 
public 

class $
PatientNotFoundException )
:* +
	Exception, 5
{ 
public $
PatientNotFoundException '
(' (
int( +
id, .
). /
: 
base 
( 
$" 
$str -
{- .
id. 0
}0 1
$str1 ;
"; <
)< =
{> ?
}@ A
} 
}		 Ò
eC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Exceptions\InvalidDataException.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Exceptions #
{ 
public 

class  
InvalidDataException %
:& '
	Exception( 1
{ 
public  
InvalidDataException #
(# $
string$ *
message+ 2
)2 3
:4 5
base6 :
(: ;
message; B
)B C
{D E
}F G
} 
} ‡
nC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Exceptions\HealthRecordNotFoundException.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Exceptions #
{ 
public 

class )
HealthRecordNotFoundException .
:/ 0
	Exception1 :
{ 
public )
HealthRecordNotFoundException ,
(, -
int- 0
id1 3
)3 4
: 
base 
( 
$" 
$str 3
{3 4
id4 6
}6 7
$str7 A
"A B
)B C
{D E
}F G
}		 
}

 Œ
hC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Exceptions\DoctorNotFoundException.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Exceptions #
{ 
public 

class #
DoctorNotFoundException (
:) *
	Exception+ 4
{ 
public #
DoctorNotFoundException &
(& '
int' *
id+ -
)- .
: 
base 
( 
$" 
$str ,
{, -
id- /
}/ 0
$str0 :
": ;
); <
{= >
}? @
}		 
}

 ›
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Exceptions\AppointmentNotFoundException.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Exceptions #
{ 
public 

class (
AppointmentNotFoundException -
:. /
	Exception0 9
{ 
public (
AppointmentNotFoundException +
(+ ,
int, /
id0 2
)2 3
: 
base 
( 
$" 
$str 0
{0 1
id1 3
}3 4
$str4 >
"> ?
)? @
{A B
}C D
}		 
}

 ì
cC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Patient\UpdatePatientDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Patient %
{ 
public 

class 
UpdatePatientDto !
{ 
[ 	
Required	 
] 
public		 
string		 
FullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
[ 	
Required	 
] 
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
RegularExpression	 
( 
$str .
,. /
ErrorMessage0 <
== >
$str? g
)g h
]h i
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
RegularExpression	 
( 
$str +
,+ ,
ErrorMessage- 9
=: ;
$str< {
){ |
]| }
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
InsuranceId !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} í
aC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Patient\PatientListDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Patient %
{ 
public 

class 
PatientListDto 
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
=- .
null/ 3
!3 4
;4 5
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
null, 0
!0 1
;1 2
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
null2 6
!6 7
;7 8
public		 
string		 
Gender		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
=		+ ,
null		- 1
!		1 2
;		2 3
public

 
bool

 
HasInsurance

  
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
} 
} £
cC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Patient\CreatePatientDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Patient %
{ 
public 

class 
CreatePatientDto !
{ 
[ 	
Required	 
] 
public		 
string		 
?		 
FullName		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
[ 	
Required	 
] 
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
RegularExpression	 
( 
$str f
)f g
]g h
public 
string 
? 
Gender 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
RegularExpression	 
( 
$str +
,+ ,
ErrorMessage- 9
=: ;
$str< {
){ |
]| }
public 
string 
? 
PhoneNumber "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
Email 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
	MaxLength	 
( 
$num 
) 
] 
public   
string   
?   
InsuranceId   "
{  # $
get  % (
;  ( )
set  * -
;  - .
}  / 0
}!! 
}"" Ú
`C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Patient\PatientFilter.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Patient %
{ 
public 

class 
PatientFilter 
:  
PaginationParam! 0
{ 
public 
bool 
? 
HasInsurance !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
? 
FullName 
{  !
get" %
;% &
set' *
;* +
}, -
}		 
}

 €
]C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Patient\PatientDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Patient %
{ 
public 

class 

PatientDto 
{ 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public		 
int		 
UsertId		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
[ 	
Required	 
] 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Required	 
] 
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
RegularExpression	 
( 
$str .
). /
]/ 0
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
RegularExpression	 
( 
$str +
,+ ,
ErrorMessage- 9
=: ;
$str< {
){ |
]| }
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
InsuranceId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public   
DateTime   
CreatedDate   #
{  $ %
get  & )
;  ) *
set  + .
;  . /
}  0 1
public!! 
bool!! 
	IsActived!! 
{!! 
get!!  #
;!!# $
set!!% (
;!!( )
}!!* +
=!!, -
true!!. 2
;!!2 3
}"" 
}## À
ZC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\PaginationParam.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
{ 
public 

class 
PaginationParam  
{ 
private 
const 
int 
MaxPageSize %
=& '
$num( +
;+ ,
private 
int 
	_pageSize 
= 
$num  
;  !
public 
int 

PageNumber 
{ 
get  #
;# $
set% (
;( )
}* +
public		 
int		 
PageSize		 
{

 	
get 
=> 
	_pageSize 
; 
set 
=> 
	_pageSize 
= 
value $
>$ %
MaxPageSize% 0
?1 2
MaxPageSize3 >
:? @
valueA F
;F G
} 	
} 
} è
VC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\PagedResult.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
{ 
public 

class 
PagedResult 
< 
T 
> 
{ 
public 
IEnumerable 
< 
T 
> 
Items $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
[5 6
]6 7
;7 8
public 
int 

PageNumber 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
PageSize 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 

TotalCount 
{ 
get  #
;# $
set% (
;( )
}* +
public

 
int

 

TotalPages

 
=>

  
(

  !
int

! $
)

$ %
Math

% )
.

) *
Ceiling

* 1
(

1 2

TotalCount

2 <
/

= >
(

? @
double

@ F
)

F G
PageSize

G O
)

O P
;

P Q
} 
} à
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\HealthRecord\UpdateHealthRecordDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
HealthRecord *
{ 
public 

class !
UpdateHealthRecordDto &
{ 
} 
} Ñ
kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\HealthRecord\HealthRecordListDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
HealthRecord *
{ 
public 

class 
HealthRecordListDto $
{ 
} 
} ‹
jC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\HealthRecord\HealthRecordFilter.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
HealthRecord *
{ 
public 

class 
HealthRecordFilter #
:# $
PaginationParam% 4
{ 
public 
DateOnly 
? 
	VisitDate "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} à
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\HealthRecord\CreateHealthRecordDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
HealthRecord *
{ 
public 

class !
CreateHealthRecordDto &
{ 
} 
} ˜
XC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\ErrorResponse.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
{ 
public 

class 
ErrorResponse 
{ 
public 
int 

StatusCode 
{ 
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
? 
Message 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
? 
Detials 
{  
get! $
;$ %
set& )
;) *
}+ ,
public		 
DateTime		 
	TimeStamp		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
public

 
string

 
?

 
Path

 
{

 
get

 !
;

! "
set

# &
;

& '
}

( )
} 
} ®
aC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Doctor\UpdateDoctorDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Doctor $
{ 
public 

class 
UpdateDoctorDto  
{ 
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public		 
string		 
FullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
=		- .
null		/ 3
!		3 4
;		4 5
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
null5 9
!9 :
;: ;
[ 	
Range	 
( 
$num 
, 
$num 
) 
] 
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Range	 
( 
$num 
, 
$num 
) 
] 
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} °
_C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Doctor\DoctorListDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Doctor $
{ 
public 

class 
DoctorListDto 
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
=- .
null/ 3
!3 4
;4 5
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
null5 9
!9 :
;: ;
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
} Â
^C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Doctor\DoctorFilter.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Doctor $
{ 
public 

class 
DoctorFilter 
: 
PaginationParam  /
{ 
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
int 
? 
MinExperience !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} Ã
fC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Doctor\CreateLeaveResultDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Doctor $
{ 
public 

class  
CreateLeaveResultDto %
{ 
public 
List 
< 
DateOnly 
> 
SkippedDates *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
=9 :
new; >
(> ?
)? @
;@ A
public 
List 
< 
DateOnly 
> ,
 CreatedWithCancelledAppointments >
{? @
getA D
;D E
setF I
;I J
}K L
=M N
newO R
(R S
)S T
;T U
} 
} œ
`C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Doctor\CreateLeaveDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Doctor $
{ 
public 

class 
CreateLeaveDto 
{ 
[ 	
Required	 
] 
public 
DateOnly 
	LeaveDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
[

 	
	MaxLength

	 
(

 
$num

 
)

 
]

 
public 
string 
? 
Reason 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} å
lC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Authentication\PatientRegisterDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Authentication ,
{ 
public 

class 
PatientRegisterDto #
{ 
[ 	
Required	 
] 
public 
string 
? 
FullName 
{  !
get" %
;% &
set' *
;* +
}, -
[

 	
Required

	 
]

 
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
RegularExpression	 
( 
$str f
)f g
]g h
public 
string 
? 
Gender 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
RegularExpression	 
( 
$str +
,+ ,
ErrorMessage- 9
=: ;
$str< {
){ |
]| }
public 
string 
? 
PhoneNumber "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
Email 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[!! 	
	MaxLength!!	 
(!! 
$num!! 
)!! 
]!! 
public"" 
string"" 
?"" 
InsuranceId"" "
{""# $
get""% (
;""( )
set""* -
;""- .
}""/ 0
}## 
}$$ ˜
bC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Authentication\LoginDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Authentication ,
{ 
public 

class 
LoginDto 
{ 
[ 	
Required	 
] 
public		 
string		 
Email		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
=		* +
string		, 2
.		2 3
Empty		3 8
;		8 9
[ 	
Required	 
] 
[ 	 
PasswordPropertyText	 
] 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=, -
string. 4
.4 5
Empty5 :
;: ;
} 
} Ã
kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Authentication\DoctorRegisterDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Authentication ,
{ 
public 

class 
DoctorRegisterDto "
{ 
[ 	
Required	 
] 
[		 	
	MaxLength			 
(		 
$num		 
)		 
]		 
public

 
string

 
FullName

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

+ ,
=

- .
null

/ 3
!

3 4
;

4 5
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
null5 9
!9 :
;: ;
[ 	
Range	 
( 
$num 
, 
$num 
) 
] 
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Range	 
( 
$num 
, 
$num 
) 
] 
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
null, 0
!0 1
;1 2
[ 	
Required	 
] 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[!! 	
Required!!	 
]!! 
public"" 
List"" 
<"" 
string"" 
>"" 
	TimeSlots"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""2 3
}$$ 
}%% ¡
kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Authentication\ChangePasswordDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Authentication ,
{ 
public 

class 
ChangePasswordDto "
{ 
public 
string 
? 
Email 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
? 
CurrentPassword &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
? 
NewPassword "
{# $
get% (
;( )
set* -
;- .
}/ 0
public		 
string		 
?		 
ConfirmNewPassword		 )
{		* +
get		, /
;		/ 0
set		1 4
;		4 5
}		6 7
} 
} ’
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Authentication\AuthResponseDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Authentication ,
{ 
public 

class 
AuthResponseDto  
{ 
public 
string 
? 
AccessToken "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
? 
Role 
{ 
get !
;! "
set# &
;& '
}( )
}

 
} ë
kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Appointment\UpdateAppointmentDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Appointments *
{ 
public 

class  
UpdateAppointmentDto %
{ 
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public		 
string		 
Status		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
=		+ ,
null		- 1
!		1 2
;		2 3
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} Î

kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Appointment\CreateAppointmentDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Appointments *
{ 
public 

class  
CreateAppointmentDto %
{ 
[ 	
Required	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
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
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
] 
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
TimeSlot 
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
} 
} ô	
kC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Appointment\AppointmentReportDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Appointments *
{ 
public 

class  
AppointmentReportDto %
{ 
public 
DateOnly 
Date 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
PendingCount 
{  !
get" %
;% &
set' *
;* +
}, -
public 
int 
ConfirmedCount !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
CancelledCount !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 
int		 
CompletedCount		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
}

 
} ±
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Appointment\AppointmentListDto.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Appointments *
{ 
public 

class 
AppointmentListDto #
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
public 
string 
PatientName !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
null2 6
!6 7
;7 8
public 
string 

DoctorName  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
null1 5
!5 6
;6 7
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public		 
string		 
TimeSlot		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
=		- .
null		/ 3
!		3 4
;		4 5
public

 
string

 
Status

 
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
=

+ ,
null

- 1
!

1 2
;

2 3
} 
} Ö
hC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\DTOs\Appointment\AppointmentFilter.cs
	namespace 	

HealthCare
 
. 
Api 
. 
DTOs 
. 
Appointment )
{ 
public 

class 
AppointmentFilter "
:# $
PaginationParam% 4
{ 
public 
string 
? 
Status 
{ 
get  #
;# $
set% (
;( )
}* +
public 
DateOnly 
? 
ScheduledDate &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} ◊

UC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Data\RoleSeeder.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Data 
{ 
public 

class 

RoleSeeder 
{ 
public 
static 
async 
Task  
SeedRoleAsync! .
(. /
RoleManager/ :
<: ;
IdentityRole; G
>G H
roleManagerH S
)S T
{		 	
string

 
[

 
]

 
roles

 
=

 
{

 
$str

 &
,

& '
$str

( 1
,

1 2
$str

3 ;
}

< =
;

= >
foreach 
( 
var 
role 
in  
roles! &
)& '
{ 
if 
( 
! 
await 
roleManager &
.& '
RoleExistsAsync' 6
(6 7
role7 ;
); <
)< =
await 
roleManager %
.% &
CreateAsync& 1
(1 2
new2 5
IdentityRole6 B
(B C
roleC G
)G H
)H I
;I J
} 
} 	
} 
} ÉY
^C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Data\HealthCareDbContext.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Data 
{ 
public		 

class		 
HealthCareDbContext		 $
:		% &
IdentityDbContext		' 8
<		8 9
IdentityUser		9 E
>		E F
{

 
public 
HealthCareDbContext "
(" #
DbContextOptions# 3
<3 4
HealthCareDbContext4 G
>G H
optionsI P
)P Q
:R S
baseT X
(X Y
optionsY `
)` a
{ 	
} 	
public 
DbSet 
< 
User 
> 
Users  
=>! #
Set$ '
<' (
User( ,
>, -
(- .
). /
;/ 0
public 
DbSet 
< 
Patient 
> 
Patients &
=>' )
Set* -
<- .
Patient. 5
>5 6
(6 7
)7 8
;8 9
public 
DbSet 
< 
Doctor 
> 
Doctors $
=>% '
Set( +
<+ ,
Doctor, 2
>2 3
(3 4
)4 5
;5 6
public 
DbSet 
< 
Appointment  
>  !
Appointments" .
=>/ 1
Set2 5
<5 6
Appointment6 A
>A B
(B C
)C D
;D E
public 
DbSet 
< 
HealthRecord !
>! "
HealthRecords# 0
=>1 3
Set4 7
<7 8
HealthRecord8 D
>D E
(E F
)F G
;G H
public 
DbSet 
< 
DoctorLeaves !
>! "
DoctorLeaves# /
=>0 2
Set3 6
<6 7
DoctorLeaves7 C
>C D
(D E
)E F
;F G
public 
DbSet 
< 
AvailableSlots #
># $
AvailableSlots% 3
=>4 6
Set7 :
<: ;
AvailableSlots; I
>I J
(J K
)K L
;L M
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
modelBuilder= I
)I J
{ 	
base 
. 
OnModelCreating  
(  !
modelBuilder! -
)- .
;. /
modelBuilder 
. 
Entity 
<  
Appointment  +
>+ ,
(, -
)- .
. 
HasIndex 
( 
a 
=> 
new 
{  
a! "
." #
DoctorId# +
,+ ,
a- .
.. /
ScheduledDate/ <
,< =
a> ?
.? @
TimeSlot@ H
}I J
)J K
. 
IsUnique 
( 
) 
. 
	HasFilter 
( 
$str 4
)4 5
. 
HasDatabaseName  
(  !
$str! C
)C D
;D E
modelBuilder"" 
."" 
Entity"" 
<""  
Appointment""  +
>""+ ,
("", -
)""- .
.## 
HasIndex## 
(## 
a## 
=>## 
new## "
{### $
a##% &
.##& '
DoctorId##' /
,##/ 0
a##1 2
.##2 3
ScheduledDate##3 @
}##A B
)##B C
.$$ 
HasDatabaseName$$  
($$  !
$str$$! >
)$$> ?
;$$? @
modelBuilder&& 
.&& 
Entity&& 
<&&  
Appointment&&  +
>&&+ ,
(&&, -
)&&- .
.'' 
HasIndex'' 
('' 
a'' 
=>'' 
new'' "
{''# $
a''% &
.''& '
	PatientId''' 0
,''0 1
a''2 3
.''3 4
ScheduledDate''4 A
}''B C
)''C D
.(( 
HasDatabaseName((  
(((  !
$str((! ?
)((? @
;((@ A
modelBuilder** 
.** 
Entity** 
<**  
HealthRecord**  ,
>**, -
(**- .
)**. /
.++ 
HasIndex++ 
(++ 
hr++ 
=>++  
new++! $
{++% &
hr++' )
.++) *
	PatientId++* 3
,++3 4
hr++5 7
.++7 8
	VisitDate++8 A
}++B C
)++C D
.,, 
HasDatabaseName,, !
(,,! "
$str,," F
),,F G
;,,G H
modelBuilder// 
.// 
Entity// 
<//  
Doctor//  &
>//& '
(//' (
)//( )
.00 
HasIndex00 
(00 
d00 
=>00 
new00 "
{00# $
d00% &
.00& '
Specialisation00' 5
,005 6
d007 8
.008 9
IsActive009 A
}00B C
)00C D
.11 
HasDatabaseName11  
(11  !
$str11! E
)11E F
;11F G
modelBuilder33 
.33 
Entity33 
<33  
DoctorLeaves33  ,
>33, -
(33- .
)33. /
.44 
HasIndex44 
(44 
l44 
=>44 
new44 "
{44# $
l44% &
.44& '
DoctorId44' /
,44/ 0
l441 2
.442 3
	LeaveDate443 <
}44= >
)44> ?
.55 
HasDatabaseName55  
(55  !
$str55! 8
)558 9
;559 :
modelBuilder88 
.88 
Entity88 
<88  
Patient88  '
>88' (
(88( )
)88) *
.99 
HasOne99 
(99 
p99 
=>99 
p99 
.99 
User99 #
)99# $
.:: 
WithOne:: 
(:: 
):: 
.;; 
HasForeignKey;; 
<;; 
Patient;; &
>;;& '
(;;' (
p;;( )
=>;;* ,
p;;- .
.;;. /
UserId;;/ 5
);;5 6
.<< 
OnDelete<< 
(<< 
DeleteBehavior<< (
.<<( )
Cascade<<) 0
)<<0 1
;<<1 2
modelBuilder?? 
.?? 
Entity?? 
<??  
Doctor??  &
>??& '
(??' (
)??( )
.@@ 
HasOne@@ 
(@@ 
d@@ 
=>@@ 
d@@ 
.@@ 
User@@ "
)@@" #
.AA 
WithOneAA 
(AA 
)AA 
.BB 
HasForeignKeyBB 
<BB 
DoctorBB $
>BB$ %
(BB% &
dBB& '
=>BB( *
dBB+ ,
.BB, -
UserIdBB- 3
)BB3 4
.CC 
OnDeleteCC 
(CC 
DeleteBehaviorCC '
.CC' (
CascadeCC( /
)CC/ 0
;CC0 1
modelBuilderEE 
.EE 
EntityEE 
<EE  
AppointmentEE  +
>EE+ ,
(EE, -
)EE- .
.FF 
HasOneFF 
(FF 
aFF 
=>FF 
aFF 
.FF 
PatientFF &
)FF& '
.GG 
WithManyGG 
(GG 
pGG 
=>GG 
pGG  
.GG  !
AppointmentsGG! -
)GG- .
.HH 
HasForeignKeyHH 
(HH 
aHH  
=>HH! #
aHH$ %
.HH% &
	PatientIdHH& /
)HH/ 0
.II 
OnDeleteII 
(II 
DeleteBehaviorII (
.II( )
RestrictII) 1
)II1 2
;II2 3
modelBuilderKK 
.KK 
EntityKK 
<KK  
AppointmentKK  +
>KK+ ,
(KK, -
)KK- .
.LL 
HasOneLL 
(LL 
aLL 
=>LL 
aLL 
.LL 
DoctorLL %
)LL% &
.MM 
WithManyMM 
(MM 
dMM 
=>MM 
dMM  
.MM  !
AppointmentsMM! -
)MM- .
.NN 
HasForeignKeyNN 
(NN 
aNN  
=>NN! #
aNN$ %
.NN% &
DoctorIdNN& .
)NN. /
.OO 
OnDeleteOO 
(OO 
DeleteBehaviorOO (
.OO( )
RestrictOO) 1
)OO1 2
;OO2 3
modelBuilderQQ 
.QQ 
EntityQQ 
<QQ  
HealthRecordQQ  ,
>QQ, -
(QQ- .
)QQ. /
.RR 
HasOneRR 
(RR 
hrRR 
=>RR 
hrRR  
.RR  !
AppointmentRR! ,
)RR, -
.SS 
WithOneSS 
(SS 
aSS 
=>SS 
aSS 
.SS  
HealthRecordSS  ,
)SS, -
.TT 
HasForeignKeyTT 
<TT 
HealthRecordTT +
>TT+ ,
(TT, -
hrTT- /
=>TT0 2
hrTT3 5
.TT5 6
AppointmentIdTT6 C
)TTC D
.UU 
OnDeleteUU 
(UU 
DeleteBehaviorUU (
.UU( )
RestrictUU) 1
)UU1 2
;UU2 3
modelBuilderWW 
.WW 
EntityWW 
<WW  
HealthRecordWW  ,
>WW, -
(WW- .
)WW. /
.XX 
HasOneXX 
(XX 
hrXX 
=>XX 
hrXX  
.XX  !
PatientXX! (
)XX( )
.YY 
WithManyYY 
(YY 
pYY 
=>YY 
pYY  
.YY  !
HealthRecordsYY! .
)YY. /
.ZZ 
HasForeignKeyZZ 
(ZZ 
hrZZ !
=>ZZ" $
hrZZ% '
.ZZ' (
	PatientIdZZ( 1
)ZZ1 2
.[[ 
OnDelete[[ 
([[ 
DeleteBehavior[[ (
.[[( )
Restrict[[) 1
)[[1 2
;[[2 3
modelBuilder]] 
.]] 
Entity]] 
<]]  
HealthRecord]]  ,
>]], -
(]]- .
)]]. /
.^^ 
HasOne^^ 
(^^ 
hr^^ 
=>^^ 
hr^^  
.^^  !
Doctor^^! '
)^^' (
.__ 
WithMany__ 
(__ 
d__ 
=>__ 
d__  
.__  !
HealthRecords__! .
)__. /
.`` 
HasForeignKey`` 
(`` 
hr`` !
=>``" $
hr``% '
.``' (
DoctorId``( 0
)``0 1
.aa 
OnDeleteaa 
(aa 
DeleteBehavioraa (
.aa( )
Restrictaa) 1
)aa1 2
;aa2 3
}bb 	
}ee 
}ff Ü
VC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Data\AdminSeeder.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Data 
{ 
public 

class 
AdminSeeder 
{ 
public 
static 
async 
Task  
SeedAdminAsync! /
(/ 0
UserManager0 ;
<; <
IdentityUser< H
>H I
userManagerJ U
,U V
RoleManagerW b
<b c
IdentityRolec o
>o p
roleManagerq |
)| }
{		 	
string

 

adminEmail

 
=

 
$str

  6
;

6 7
string 
adminPassword  
=! "
$str# .
;. /
await 

RoleSeeder 
. 
SeedRoleAsync *
(* +
roleManager+ 6
)6 7
;7 8
var 
existingAdmin 
= 
await  %
userManager& 1
.1 2
FindByEmailAsync2 B
(B C

adminEmailC M
)M N
;N O
if 
( 
existingAdmin 
==  
null! %
)% &
{ 
var 
admin 
= 
new 
IdentityUser  ,
{ 
UserName 
= 

adminEmail )
,) *
Email 
= 

adminEmail &
,& '
EmailConfirmed "
=# $
true% )
} 
; 
var 
result 
= 
await "
userManager# .
.. /
CreateAsync/ :
(: ;
admin; @
,@ A
adminPasswordB O
)O P
;P Q
if 
( 
result 
. 
	Succeeded $
)$ %
{ 
await 
userManager %
.% &
AddToRoleAsync& 4
(4 5
admin5 :
,: ;
$str< C
)C D
;D E
} 
else   
{!! 
throw"" 
new"" 
	Exception"" '
(""' (
$str""( A
+""B C
string## 
.## 
Join## #
(### $
$str##$ (
,##( )
result##* 0
.##0 1
Errors##1 7
.##7 8
Select##8 >
(##> ?
e##? @
=>##A C
e##D E
.##E F
Description##F Q
)##Q R
)##R S
)##S T
;##T U
}$$ 
}%% 
}&& 	
}(( 
})) ¨3
dC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\RegisterController.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Controllers $
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		 
[

 
ApiController

 
]

 
public 

class 
RegisterController #
:$ %
ControllerBase& 4
{ 
private 
readonly 
IAuthService %
_authService& 2
;2 3
public 
RegisterController !
(! "
IAuthService" .
authService/ :
): ;
{ 	
_authService 
= 
authService &
;& '
} 	
[ 	
AllowAnonymous	 
] 
[ 	
HttpPost	 
( 
$str $
)$ %
]% &
public 
async 
Task 
< 
IActionResult '
>' (
RegisterPatient) 8
(8 9
[9 :
FromBody: B
]B C
PatientRegisterDtoD V
dtoW Z
)Z [
{ 	
if 
( 
! 

ModelState 
. 
IsValid #
)# $
return 

BadRequest !
(! "

ModelState" ,
), -
;- .
await 
_authService 
.  
RegisterPatientAsync 3
(3 4
dto4 7
)7 8
;8 9
return 
Ok 
( 
new 
{ 
message #
=$ %
$str& G
}H I
)I J
;J K
}   	
[## 	
HttpPost##	 
(## 
$str## #
)### $
]##$ %
[$$ 	
	Authorize$$	 
($$ !
AuthenticationSchemes$$ (
=$$) *
JwtBearerDefaults$$+ <
.$$< = 
AuthenticationScheme$$= Q
)$$Q R
]$$R S
[%% 	
	Authorize%%	 
(%% 
Roles%% 
=%% 
$str%% "
)%%" #
]%%# $
public&& 
async&& 
Task&& 
<&& 
IActionResult&& '
>&&' (
RegisterDoctor&&) 7
(&&7 8
[&&8 9
FromBody&&9 A
]&&A B
DoctorRegisterDto&&C T
dto&&U X
)&&X Y
{'' 	
if(( 
((( 
!(( 

ModelState(( 
.(( 
IsValid(( #
)((# $
return)) 

BadRequest)) !
())! "

ModelState))" ,
))), -
;))- .
await++ 
_authService++ 
.++ 
RegisterDoctorAsync++ 2
(++2 3
dto++3 6
)++6 7
;++7 8
return-- 
Ok-- 
(-- 
new-- 
{-- 
message-- #
=--$ %
$str--& C
}--D E
)--E F
;--F G
}.. 	
[11 	
AllowAnonymous11	 
]11 
[22 	
HttpPost22	 
(22 
$str22 
)22 
]22 
public33 
async33 
Task33 
<33 
IActionResult33 '
>33' (
Login33) .
(33. /
[33/ 0
FromBody330 8
]338 9
LoginDto33: B
dto33C F
)33F G
{44 	
if55 
(55 
!55 

ModelState55 
.55 
IsValid55 #
)55# $
return66 

BadRequest66 !
(66! "

ModelState66" ,
)66, -
;66- .
var88 
response88 
=88 
await88  
_authService88! -
.88- .

LoginAsync88. 8
(888 9
dto889 <
)88< =
;88= >
return:: 
Ok:: 
(:: 
response:: 
):: 
;::  
};; 	
[>> 	
	Authorize>>	 
]>> 
[?? 	
	Authorize??	 
(?? !
AuthenticationSchemes?? (
=??) *
JwtBearerDefaults??+ <
.??< = 
AuthenticationScheme??= Q
)??Q R
]??R S
[@@ 	
HttpGet@@	 
(@@ 
$str@@ 
)@@ 
]@@ 
publicAA 
IActionResultAA 
GetCurrentUserAA +
(AA+ ,
)AA, -
{BB 	
varCC 
userIdCC 
=CC 
UserCC 
.CC 
	FindFirstCC '
(CC' (
SystemCC( .
.CC. /
SecurityCC/ 7
.CC7 8
ClaimsCC8 >
.CC> ?

ClaimTypesCC? I
.CCI J
NameIdentifierCCJ X
)CCX Y
?CCY Z
.CCZ [
ValueCC[ `
;CC` a
varDD 
emailDD 
=DD 
UserDD 
.DD 
	FindFirstDD &
(DD& '
SystemDD' -
.DD- .
SecurityDD. 6
.DD6 7
ClaimsDD7 =
.DD= >

ClaimTypesDD> H
.DDH I
EmailDDI N
)DDN O
?DDO P
.DDP Q
ValueDDQ V
;DDV W
varEE 
roleEE 
=EE 
UserEE 
.EE 
	FindFirstEE %
(EE% &
SystemEE& ,
.EE, -
SecurityEE- 5
.EE5 6
ClaimsEE6 <
.EE< =

ClaimTypesEE= G
.EEG H
RoleEEH L
)EEL M
?EEM N
.EEN O
ValueEEO T
;EET U
varGG 
	patientIdGG 
=GG 
UserGG  
.GG  !
	FindFirstGG! *
(GG* +
$strGG+ 6
)GG6 7
?GG7 8
.GG8 9
ValueGG9 >
;GG> ?
varHH 
doctorIdHH 
=HH 
UserHH 
.HH  
	FindFirstHH  )
(HH) *
$strHH* 4
)HH4 5
?HH5 6
.HH6 7
ValueHH7 <
;HH< =
returnJJ 
OkJJ 
(JJ 
newJJ 
{KK 
userIdLL 
,LL 
emailMM 
,MM 
roleNN 
,NN 
	patientIdOO 
,OO 
doctorIdPP 
}QQ 
)QQ 
;QQ 
}RR 	
}SS 
}TT ëT
cC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\PatientController.cs
	namespace

 	

HealthCare


 
.

 
Api

 
.

 
Controllers

 $
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public 

class 
PatientController "
:# $
ControllerBase% 3
{ 
private 
readonly 
IPatientService (
_service) 1
;1 2
public 
PatientController  
(  !
IPatientService! 0
service1 8
)8 9
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
)Q R
]R S
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
PatientFilter0 =
filter> D
)D E
{ 	
var 
patients 
= 
await  
_service! )
.) *
GetAllAsync* 5
(5 6
filter6 <
)< =
;= >
return 
Ok 
( 
patients 
) 
;  
} 	
["" 	
HttpGet""	 
("" 
$str"" 
)"" 
]"" 
[## 	
	Authorize##	 
(## !
AuthenticationSchemes## (
=##) *
JwtBearerDefaults##+ <
.##< = 
AuthenticationScheme##= Q
)##Q R
]##R S
[$$ 	
	Authorize$$	 
($$ 
Roles$$ 
=$$ 
$str$$ "
)$$" #
]$$# $
public%% 
async%% 
Task%% 
<%% 
IActionResult%% '
>%%' (
GetById%%) 0
(%%0 1
int%%1 4
id%%5 7
)%%7 8
{&& 	
var'' 
patient'' 
='' 
await'' 
_service''  (
.''( )
GetByIdAsync'') 5
(''5 6
id''6 8
)''8 9
;''9 :
if)) 
()) 
patient)) 
is)) 
null)) 
)))  
return** 
NotFound** 
(**  
)**  !
;**! "
return,, 
Ok,, 
(,, 
patient,, 
),, 
;,, 
}-- 	
[00 	
HttpPut00	 
]00 
[11 	
	Authorize11	 
(11 !
AuthenticationSchemes11 (
=11) *
JwtBearerDefaults11+ <
.11< = 
AuthenticationScheme11= Q
)11Q R
]11R S
[22 	
	Authorize22	 
(22 
Roles22 
=22 
$str22 *
)22* +
]22+ ,
public33 
async33 
Task33 
<33 
IActionResult33 '
>33' (
Update33) /
(33/ 0
int330 3
id334 6
,336 7
UpdatePatientDto339 I
dto33J M
)33M N
{44 	
if55 
(55 
!55 

ModelState55 
.55 
IsValid55 #
)55# $
return66 

BadRequest66 !
(66! "

ModelState66" ,
)66, -
;66- .
if88 
(88 
id88 
==88 
null88 
)88 
return99 
Unauthorized99 #
(99# $
)99$ %
;99% &
await<< 
_service<< 
.<< 
UpdateAsync<< &
(<<& '
id<<' )
,<<) *
dto<<+ .
)<<. /
;<</ 0
return>> 
	NoContent>> 
(>> 
)>> 
;>> 
}?? 	
[AA 	

HttpDeleteAA	 
(AA 
$strAA 
)AA 
]AA  
[BB 	
	AuthorizeBB	 
(BB !
AuthenticationSchemesBB (
=BB) *
JwtBearerDefaultsBB+ <
.BB< = 
AuthenticationSchemeBB= Q
)BBQ R
]BBR S
[CC 	
	AuthorizeCC	 
(CC 
RolesCC 
=CC 
$strCC "
)CC" #
]CC# $
publicDD 
asyncDD 
TaskDD 
<DD 
IActionResultDD '
>DD' (
DeleteDD) /
(DD/ 0
intDD0 3
idDD4 6
)DD6 7
{EE 	
awaitFF 
_serviceFF 
.FF 
DeleteAsyncFF &
(FF& '
idFF' )
)FF) *
;FF* +
returnGG 
	NoContentGG 
(GG 
)GG 
;GG 
}HH 	
[LL 	
HttpGetLL	 
(LL 
$strLL 
)LL 
]LL 
publicMM 
asyncMM 
TaskMM 
<MM 
IActionResultMM '
>MM' (
SearchByNameMM) 5
(MM5 6
stringMM6 <
nameMM= A
)MMA B
{NN 	
ifOO 
(OO 
stringOO 
.OO 
IsNullOrWhiteSpaceOO )
(OO) *
nameOO* .
)OO. /
)OO/ 0
returnPP 

BadRequestPP !
(PP! "
$strPP" 4
)PP4 5
;PP5 6
varRR 
resultRR 
=RR 
awaitRR 
_serviceRR '
.RR' (
SearchByNameAsyncRR( 9
(RR9 :
nameRR: >
)RR> ?
;RR? @
ifTT 
(TT 
!TT 
resultTT 
.TT 
AnyTT 
(TT 
)TT 
)TT 
returnUU 
NotFoundUU 
(UU  
$"UU  "
$strUU" ?
{UU? @
nameUU@ D
}UUD E
$strUUE F
"UUF G
)UUG H
;UUH I
returnWW 
OkWW 
(WW 
resultWW 
)WW 
;WW 
}XX 	
[\\ 	
HttpGet\\	 
(\\ 
$str\\ 
)\\ 
]\\ 
[]] 	
	Authorize]]	 
(]] !
AuthenticationSchemes]] (
=]]) *
JwtBearerDefaults]]+ <
.]]< = 
AuthenticationScheme]]= Q
)]]Q R
]]]R S
[^^ 	
	Authorize^^	 
(^^ 
Roles^^ 
=^^ 
$str^^ $
)^^$ %
]^^% &
public__ 
async__ 
Task__ 
<__ 
IActionResult__ '
>__' (
GetMyProfile__) 5
(__5 6
)__6 7
{`` 	
varaa 
userIdaa 
=aa 
Useraa 
.aa 
	FindFirstaa '
(aa' (
Systemaa( .
.aa. /
Securityaa/ 7
.aa7 8
Claimsaa8 >
.aa> ?

ClaimTypesaa? I
.aaI J
NameIdentifieraaJ X
)aaX Y
?aaY Z
.aaZ [
Valueaa[ `
;aa` a
ifcc 
(cc 
stringcc 
.cc 
IsNullOrEmptycc $
(cc$ %
userIdcc% +
)cc+ ,
)cc, -
returndd 
Unauthorizeddd #
(dd# $
)dd$ %
;dd% &
vargg 
patientgg 
=gg 
awaitgg 
_servicegg  (
.gg( )
GetByUserIdAsyncgg) 9
(gg9 :
userIdgg: @
)gg@ A
;ggA B
returnii 
Okii 
(ii 
patientii 
)ii 
;ii 
}jj 	
[mm 	
HttpPutmm	 
(mm 
$strmm 
)mm 
]mm 
[nn 	
	Authorizenn	 
(nn !
AuthenticationSchemesnn (
=nn) *
JwtBearerDefaultsnn+ <
.nn< = 
AuthenticationSchemenn= Q
)nnQ R
]nnR S
[oo 	
	Authorizeoo	 
(oo 
Rolesoo 
=oo 
$stroo $
)oo$ %
]oo% &
publicpp 
asyncpp 
Taskpp 
<pp 
IActionResultpp '
>pp' (
UpdateMyProfilepp) 8
(pp8 9
UpdatePatientDtopp9 I
dtoppJ M
)ppM N
{qq 	
varrr 
userIdrr 
=rr 
Userrr 
.rr 
	FindFirstrr '
(rr' (
Systemrr( .
.rr. /
Securityrr/ 7
.rr7 8
Claimsrr8 >
.rr> ?

ClaimTypesrr? I
.rrI J
NameIdentifierrrJ X
)rrX Y
?rrY Z
.rrZ [
Valuerr[ `
;rr` a
iftt 
(tt 
stringtt 
.tt 
IsNullOrEmptytt $
(tt$ %
userIdtt% +
)tt+ ,
)tt, -
returnuu 
Unauthorizeduu #
(uu# $
)uu$ %
;uu% &
awaitww 
_serviceww 
.ww 
UpdateByUserIdAsyncww .
(ww. /
userIdww/ 5
,ww5 6
dtoww7 :
)ww: ;
;ww; <
returnyy 
	NoContentyy 
(yy 
)yy 
;yy 
}zz 	
[}} 	
	HttpPatch}}	 
(}} 
$str}} $
)}}$ %
]}}% &
[~~ 	
	Authorize~~	 
(~~ !
AuthenticationSchemes~~ (
=~~) *
JwtBearerDefaults~~+ <
.~~< = 
AuthenticationScheme~~= Q
)~~Q R
]~~R S
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public
ÄÄ 
async
ÄÄ 
Task
ÄÄ 
<
ÄÄ 
IActionResult
ÄÄ '
>
ÄÄ' (
UpdateStatus
ÄÄ) 5
(
ÄÄ5 6
int
ÄÄ6 9
id
ÄÄ: <
,
ÄÄ< =
bool
ÄÄ> B
isActive
ÄÄC K
)
ÄÄK L
{
ÅÅ 	
await
ÇÇ 
_service
ÇÇ 
.
ÇÇ 
UpdateStatusAsync
ÇÇ ,
(
ÇÇ, -
id
ÇÇ- /
,
ÇÇ/ 0
isActive
ÇÇ1 9
)
ÇÇ9 :
;
ÇÇ: ;
return
ÉÉ 
	NoContent
ÉÉ 
(
ÉÉ 
)
ÉÉ 
;
ÉÉ 
}
ÑÑ 	
}
áá 
}àà ñ)
hC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\HealthRecordController.cs
	namespace		 	

HealthCare		
 
.		 
Api		 
.		 
Controllers		 $
{

 
[ 
Route 

(
 
$str 
) 
]  
[ 
ApiController 
] 
public 

class "
HealthRecordController '
:( )
ControllerBase* 8
{ 
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
public "
HealthRecordController %
(% & 
IHealthRecordService& :
healthRecordService; N
)N O
{ 	 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
[ 	
HttpPost	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
)Q R
]R S
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
Add) ,
(, -
[- .
FromBody. 6
]6 7!
CreateHealthRecordDto8 M
dtoN Q
)Q R
{ 	
if 
( 
! 

ModelState 
. 
IsValid #
)# $
return 

BadRequest !
(! "

ModelState" ,
), -
;- .
await    
_healthRecordService   &
.  & '
AddAsync  ' /
(  / 0
dto  0 3
)  3 4
;  4 5
return"" 
Ok"" 
("" 
new"" 
{"" 
message"" #
=""$ %
$str""& J
}""K L
)""L M
;""M N
}## 	
[&& 	
HttpGet&&	 
]&& 
public'' 
async'' 
Task'' 
<'' 
IActionResult'' '
>''' (
GetAll'') /
(''/ 0
HealthRecordFilter''0 B
filter''C I
)''I J
{(( 	
var)) 
records)) 
=)) 
await))  
_healthRecordService))  4
.))4 5
GetAllAsync))5 @
())@ A
filter))A G
)))G H
;))H I
return** 
Ok** 
(** 
records** 
)** 
;** 
}++ 	
[.. 	
HttpGet..	 
(.. 
$str.. 
).. 
].. 
public00 
async00 
Task00 
<00 
IActionResult00 '
>00' (
GetById00) 0
(000 1
int001 4
id005 7
)007 8
{11 	
var22 
record22 
=22 
await22  
_healthRecordService22 3
.223 4
GetByIdAsync224 @
(22@ A
id22A C
)22C D
;22D E
if44 
(44 
record44 
==44 
null44 
)44 
return55 
NotFound55 
(55  
)55  !
;55! "
return77 
Ok77 
(77 
record77 
)77 
;77 
}88 	
[;; 	
HttpPut;;	 
(;; 
$str;; 
);; 
];; 
public<< 
async<< 
Task<< 
<<< 
IActionResult<< '
><<' (
Update<<) /
(<</ 0
int<<0 3
id<<4 6
,<<6 7
[<<8 9
FromBody<<9 A
]<<A B!
UpdateHealthRecordDto<<C X
dto<<Y \
)<<\ ]
{== 	
if>> 
(>> 
!>> 

ModelState>> 
.>> 
IsValid>> #
)>># $
return?? 

BadRequest?? !
(??! "

ModelState??" ,
)??, -
;??- .
awaitAA  
_healthRecordServiceAA &
.AA& '
UpdateAsyncAA' 2
(AA2 3
idAA3 5
,AA5 6
dtoAA7 :
)AA: ;
;AA; <
returnCC 
OkCC 
(CC 
newCC 
{CC 
messageCC #
=CC$ %
$strCC& J
}CCK L
)CCL M
;CCM N
}DD 	
[GG 	

HttpDeleteGG	 
(GG 
$strGG 
)GG 
]GG  
publicHH 
asyncHH 
TaskHH 
<HH 
IActionResultHH '
>HH' (
DeleteHH) /
(HH/ 0
intHH0 3
idHH4 6
)HH6 7
{II 	
awaitJJ  
_healthRecordServiceJJ &
.JJ& '
DeleteAsyncJJ' 2
(JJ2 3
idJJ3 5
)JJ5 6
;JJ6 7
returnLL 
	NoContentLL 
(LL 
)LL 
;LL 
}MM 	
}PP 
}QQ ≠b
bC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\DoctorController.cs
	namespace		 	

HealthCare		
 
.		 
Api		 
.		 
Controllers		 $
{

 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public 

class 
DoctorController !
:" #
ControllerBase$ 2
{ 
private 
readonly 
IDoctorService '
_service( 0
;0 1
public 
DoctorController 
(  
IDoctorService  .
service/ 6
)6 7
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
)Q R
]R S
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
DoctorFilter0 <
filter= C
)C D
{ 	
var 
doctors 
= 
await 
_service  (
.( )
GetAllAsync) 4
(4 5
filter5 ;
); <
;< =
return 
Ok 
( 
doctors 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[   	
	Authorize  	 
(   !
AuthenticationSchemes   (
=  ) *
JwtBearerDefaults  + <
.  < = 
AuthenticationScheme  = Q
)  Q R
]  R S
[!! 	
	Authorize!!	 
(!! 
Roles!! 
=!! 
$str!! "
)!!" #
]!!# $
public"" 
async"" 
Task"" 
<"" 
IActionResult"" '
>""' (
GetById"") 0
(""0 1
int""1 4
id""5 7
)""7 8
{## 	
var$$ 
doctor$$ 
=$$ 
await$$ 
_service$$ '
.$$' (
GetByIdAsync$$( 4
($$4 5
id$$5 7
)$$7 8
;$$8 9
if&& 
(&& 
doctor&& 
is&& 
null&& 
)&& 
return'' 
NotFound'' 
(''  
)''  !
;''! "
return)) 
Ok)) 
()) 
doctor)) 
))) 
;)) 
}** 	
[-- 	
HttpPut--	 
(-- 
$str-- 
)-- 
]-- 
[.. 	
	Authorize..	 
(.. !
AuthenticationSchemes.. (
=..) *
JwtBearerDefaults..+ <
...< = 
AuthenticationScheme..= Q
)..Q R
]..R S
[// 	
	Authorize//	 
(// 
Roles// 
=// 
$str// )
)//) *
]//* +
public00 
async00 
Task00 
<00 
IActionResult00 '
>00' (
Update00) /
(00/ 0
int000 3
id004 6
,006 7
[008 9
FromBody009 A
]00A B
UpdateDoctorDto00C R
dto00S V
)00V W
{11 	
if22 
(22 
!22 

ModelState22 
.22 
IsValid22 #
)22# $
return33 

BadRequest33 !
(33! "

ModelState33" ,
)33, -
;33- .
await55 
_service55 
.55 
UpdateAsync55 &
(55& '
id55' )
,55) *
dto55+ .
)55. /
;55/ 0
return77 
	NoContent77 
(77 
)77 
;77 
}88 	
[;; 	

HttpDelete;;	 
(;; 
$str;; 
);; 
];;  
[<< 	
	Authorize<<	 
(<< !
AuthenticationSchemes<< (
=<<) *
JwtBearerDefaults<<+ <
.<<< = 
AuthenticationScheme<<= Q
)<<Q R
]<<R S
[== 	
	Authorize==	 
(== 
Roles== 
=== 
$str== "
)==" #
]==# $
public>> 
async>> 
Task>> 
<>> 
IActionResult>> '
>>>' (
Delete>>) /
(>>/ 0
int>>0 3
id>>4 6
)>>6 7
{?? 	
await@@ 
_service@@ 
.@@ 
DeleteAsync@@ &
(@@& '
id@@' )
)@@) *
;@@* +
returnBB 
	NoContentBB 
(BB 
)BB 
;BB 
}CC 	
[FF 	
	HttpPatchFF	 
(FF 
$strFF $
)FF$ %
]FF% &
[GG 	
	AuthorizeGG	 
(GG !
AuthenticationSchemesGG (
=GG) *
JwtBearerDefaultsGG+ <
.GG< = 
AuthenticationSchemeGG= Q
)GGQ R
]GGR S
[HH 	
	AuthorizeHH	 
(HH 
RolesHH 
=HH 
$strHH "
)HH" #
]HH# $
publicII 
asyncII 
TaskII 
<II 
IActionResultII '
>II' (
UpdateStatusII) 5
(II5 6
intII6 9
idII: <
,II< =
[II> ?
	FromQueryII? H
]IIH I
boolIIJ N
isActiveIIO W
)IIW X
{JJ 	
awaitKK 
_serviceKK 
.KK 
UpdateStatusAsyncKK ,
(KK, -
idKK- /
,KK/ 0
isActiveKK1 9
)KK9 :
;KK: ;
returnLL 
	NoContentLL 
(LL 
)LL 
;LL 
}MM 	
[PP 	
HttpPostPP	 
(PP 
$strPP "
)PP" #
]PP# $
[QQ 	
	AuthorizeQQ	 
(QQ !
AuthenticationSchemesQQ (
=QQ) *
JwtBearerDefaultsQQ+ <
.QQ< = 
AuthenticationSchemeQQ= Q
)QQQ R
]QQR S
[RR 	
	AuthorizeRR	 
(RR 
RolesRR 
=RR 
$strRR )
)RR) *
]RR* +
publicSS 
asyncSS 
TaskSS 
<SS 
IActionResultSS '
>SS' (
CreateSlotsSS) 4
(SS4 5
intSS5 8
idSS9 ;
,SS; <
[SS= >
FromBodySS> F
]SSF G
ListSSH L
<SSL M
stringSSM S
>SSS T
	timeSlotsSSU ^
)SS^ _
{TT 	
ifUU 
(UU 
	timeSlotsUU 
==UU 
nullUU !
||UU" $
!UU% &
	timeSlotsUU& /
.UU/ 0
AnyUU0 3
(UU3 4
)UU4 5
)UU5 6
returnVV 

BadRequestVV !
(VV! "
$strVV" ;
)VV; <
;VV< =
awaitXX 
_serviceXX 
.XX 
CreateSlotsXX &
(XX& '
idXX' )
,XX) *
	timeSlotsXX+ 4
)XX4 5
;XX5 6
returnZZ 
OkZZ 
(ZZ 
$strZZ 2
)ZZ2 3
;ZZ3 4
}[[ 	
[^^ 	
HttpGet^^	 
(^^ 
$str^^ !
)^^! "
]^^" #
[__ 	
	Authorize__	 
(__ !
AuthenticationSchemes__ (
=__) *
JwtBearerDefaults__+ <
.__< = 
AuthenticationScheme__= Q
)__Q R
]__R S
[`` 	
	Authorize``	 
(`` 
Roles`` 
=`` 
$str`` )
)``) *
]``* +
publicaa 
asyncaa 
Taskaa 
<aa 
IActionResultaa '
>aa' (
GetSlotsaa) 1
(aa1 2
intaa2 5
idaa6 8
)aa8 9
{bb 	
varcc 
slotscc 
=cc 
awaitcc 
_servicecc &
.cc& '
GetSlotscc' /
(cc/ 0
idcc0 2
)cc2 3
;cc3 4
returndd 
Okdd 
(dd 
slotsdd 
)dd 
;dd 
}ee 	
[hh 	
HttpGethh	 
(hh 
$strhh +
)hh+ ,
]hh, -
[ii 	
	Authorizeii	 
(ii !
AuthenticationSchemesii (
=ii) *
JwtBearerDefaultsii+ <
.ii< = 
AuthenticationSchemeii= Q
)iiQ R
]iiR S
[jj 	
	Authorizejj	 
(jj 
Rolesjj 
=jj 
$strjj *
)jj* +
]jj+ ,
publickk 
asynckk 
Taskkk 
<kk 
IActionResultkk '
>kk' (
AvailableSlotskk) 7
(kk7 8
intkk8 ;
idkk< >
,kk> ?
[kk@ A
	FromQuerykkA J
]kkJ K
DateOnlykkL T
datekkU Y
)kkY Z
{ll 	
varmm 
resultmm 
=mm 
awaitmm 
_servicemm '
.mm' (#
AvailableTimeSlotsCheckmm( ?
(mm? @
datemm@ D
,mmD E
idmmF H
)mmH I
;mmI J
returnnn 
Oknn 
(nn 
resultnn 
)nn 
;nn 
}oo 	
[rr 	
HttpPostrr	 
(rr 
$strrr #
)rr# $
]rr$ %
[ss 	
	Authorizess	 
(ss !
AuthenticationSchemesss (
=ss) *
JwtBearerDefaultsss+ <
.ss< = 
AuthenticationSchemess= Q
)ssQ R
]ssR S
[tt 	
	Authorizett	 
(tt 
Rolestt 
=tt 
$strtt "
)tt" #
]tt# $
publicuu 
asyncuu 
Taskuu 
<uu 
IActionResultuu '
>uu' (
CreateLeaveuu) 4
(uu4 5
intuu5 8
iduu9 ;
,uu; <
[uu= >
FromBodyuu> F
]uuF G
ListuuH L
<uuL M
CreateLeaveDtouuM [
>uu[ \
leavesuu] c
)uuc d
{vv 	
ifww 
(ww 
leavesww 
==ww 
nullww 
||ww !
!ww" #
leavesww# )
.ww) *
Anyww* -
(ww- .
)ww. /
)ww/ 0
returnxx 

BadRequestxx !
(xx! "
$strxx" :
)xx: ;
;xx; <
varzz 
resultzz 
=zz 
awaitzz 
_servicezz '
.zz' (
CreateLeavezz( 3
(zz3 4
idzz4 6
,zz6 7
leaveszz8 >
)zz> ?
;zz? @
return|| 
Ok|| 
(|| 
result|| 
)|| 
;|| 
}}} 	
[
ÄÄ 	
HttpGet
ÄÄ	 
(
ÄÄ 
$str
ÄÄ 
)
ÄÄ 
]
ÄÄ 
[
ÅÅ 	
AllowAnonymous
ÅÅ	 
]
ÅÅ 
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
IActionResult
ÇÇ '
>
ÇÇ' (
AvailableDoctors
ÇÇ) 9
(
ÇÇ9 :
[
ÉÉ 
	FromQuery
ÉÉ 
]
ÉÉ 
string
ÉÉ  &
specialisation
ÉÉ' 5
,
ÉÉ5 6
[
ÑÑ 
	FromQuery
ÑÑ 
]
ÑÑ 
DateOnly
ÑÑ  (
date
ÑÑ) -
)
ÑÑ- .
{
ÖÖ 	
if
ÜÜ 
(
ÜÜ 
string
ÜÜ 
.
ÜÜ  
IsNullOrWhiteSpace
ÜÜ )
(
ÜÜ) *
specialisation
ÜÜ* 8
)
ÜÜ8 9
)
ÜÜ9 :
return
áá 

BadRequest
áá !
(
áá! "
$str
áá" >
)
áá> ?
;
áá? @
var
ââ 
result
ââ 
=
ââ 
await
ââ 
_service
ââ '
.
ââ' (
AvailableDoctors
ââ( 8
(
ââ8 9
specialisation
ââ9 G
,
ââG H
date
ââI M
)
ââM N
;
ââN O
return
ãã 
Ok
ãã 
(
ãã 
result
ãã 
)
ãã 
;
ãã 
}
åå 	
}
êê 
}ëë ˘b
gC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AppointmentController.cs
	namespace		 	

HealthCare		
 
.		 
Api		 
.		 
Controllers		 $
{

 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public 

class !
AppointmentController &
:' (
ControllerBase) 7
{ 
private 
readonly 
IAppointmentService ,
_service- 5
;5 6
public !
AppointmentController $
($ %
IAppointmentService% 8
service9 @
)@ A
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpPost	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
)Q R
]R S
[ 	
	Authorize	 
( 
Roles 
= 
$str $
)$ %
]% &
public 
async 
Task 
< 
IActionResult '
>' (
Create) /
(/ 0
[0 1
FromBody1 9
]9 : 
CreateAppointmentDto; O
dtoP S
,S T
intT W
idX Z
)Z [
{ 	
if 
( 
! 

ModelState 
. 
IsValid #
)# $
return 

BadRequest !
(! "

ModelState" ,
), -
;- .
await   
_service   
.   
AddAsync   #
(  # $
dto  $ '
,  ' (
id  ( *
)  * +
;  + ,
return"" 

StatusCode"" 
("" 
$num"" !
,""! "
new""# &
{## 
message$$ 
=$$ 
$str$$ <
}%% 
)%% 
;%% 
}&& 	
[)) 	
HttpPut))	 
()) 
$str)) 
))) 
])) 
[** 	
	Authorize**	 
(** !
AuthenticationSchemes** (
=**) *
JwtBearerDefaults**+ <
.**< = 
AuthenticationScheme**= Q
)**Q R
]**R S
[++ 	
	Authorize++	 
(++ 
Roles++ 
=++ 
$str++ #
)++# $
]++$ %
public,, 
async,, 
Task,, 
<,, 
IActionResult,, '
>,,' (
Update,,) /
(,,/ 0
int,,0 3
id,,4 6
,,,6 7
[,,8 9
FromBody,,9 A
],,A B 
UpdateAppointmentDto,,C W
dto,,X [
),,[ \
{-- 	
if.. 
(.. 
!.. 

ModelState.. 
... 
IsValid.. #
)..# $
return// 

BadRequest// !
(//! "

ModelState//" ,
)//, -
;//- .
await11 
_service11 
.11 
UpdateAsync11 &
(11& '
id11' )
,11) *
dto11+ .
)11. /
;11/ 0
return33 
	NoContent33 
(33 
)33 
;33 
}44 	
[77 	
	HttpPatch77	 
(77 
$str77 $
)77$ %
]77% &
[88 	
	Authorize88	 
(88 !
AuthenticationSchemes88 (
=88) *
JwtBearerDefaults88+ <
.88< = 
AuthenticationScheme88= Q
)88Q R
]88R S
[99 	
	Authorize99	 
(99 
Roles99 
=99 
$str99 #
)99# $
]99$ %
public:: 
async:: 
Task:: 
<:: 
IActionResult:: '
>::' (
UpdateStatus::) 5
(::5 6
int::6 9
id::: <
,::< =
[::> ?
FromBody::? G
]::G H 
UpdateAppointmentDto::I ]
dto::^ a
)::a b
{;; 	
await<< 
_service<< 
.<< 
UpdateStatusAsync<< ,
(<<, -
id<<- /
,<</ 0
dto<<1 4
)<<4 5
;<<5 6
return== 
	NoContent== 
(== 
)== 
;== 
}>> 	
[AA 	
HttpGetAA	 
(AA 
$strAA "
)AA" #
]AA# $
[BB 	
	AuthorizeBB	 
(BB !
AuthenticationSchemesBB (
=BB) *
JwtBearerDefaultsBB+ <
.BB< = 
AuthenticationSchemeBB= Q
)BBQ R
]BBR S
[CC 	
	AuthorizeCC	 
(CC 
RolesCC 
=CC 
$strCC $
)CC$ %
]CC% &
publicDD 
asyncDD 
TaskDD 
<DD 
IActionResultDD '
>DD' (
GetAvailableSlotsDD) :
(DD: ;
[EE 
	FromQueryEE 
]EE 
intEE  #
doctorIdEE$ ,
,EE, -
[FF 
	FromQueryFF 
]FF 
DateOnlyFF  (
dateFF) -
)FF- .
{GG 	
varHH 
slotsHH 
=HH 
awaitHH 
_serviceHH &
.HH& '
AvailableTimeSlotsHH' 9
(HH9 :
dateHH: >
,HH> ?
doctorIdHH@ H
)HHH I
;HHI J
returnII 
OkII 
(II 
slotsII 
)II 
;II 
}JJ 	
[MM 	
HttpGetMM	 
(MM 
$strMM %
)MM% &
]MM& '
[NN 	
	AuthorizeNN	 
(NN !
AuthenticationSchemesNN (
=NN) *
JwtBearerDefaultsNN+ <
.NN< = 
AuthenticationSchemeNN= Q
)NNQ R
]NNR S
[OO 	
	AuthorizeOO	 
(OO 
RolesOO 
=OO 
$strOO $
)OO$ %
]OO% &
publicPP 
asyncPP 
TaskPP 
<PP 
IActionResultPP '
>PP' (
CheckAvailabilityPP) :
(PP: ;
intPP; >
doctorIdPP? G
,PPG H
DateOnlyPPI Q
datePPR V
,PPV W
stringPPX ^
timeSlotPP_ g
)PPg h
{QQ 	
varRR 
resultRR 
=RR 
awaitRR 
_serviceRR '
.RR' (
IsAvailableRR( 3
(RR3 4
dateRR4 8
,RR8 9
doctorIdRR: B
,RRB C
timeSlotRRD L
)RRL M
;RRM N
returnSS 
OkSS 
(SS 
newSS 
{SS 
	availableSS %
=SS& '
resultSS( .
}SS/ 0
)SS0 1
;SS1 2
}TT 	
[VV 	
HttpGetVV	 
(VV 
$strVV 1
)VV1 2
]VV2 3
[WW 	
	AuthorizeWW	 
(WW !
AuthenticationSchemesWW (
=WW) *
JwtBearerDefaultsWW+ <
.WW< = 
AuthenticationSchemeWW= Q
)WWQ R
]WWR S
[XX 	
	AuthorizeXX	 
(XX 
RolesXX 
=XX 
$strXX #
)XX# $
]XX$ %
publicYY 
asyncYY 
TaskYY 
<YY 
IActionResultYY '
>YY' (
GetDoctorScheduleYY) :
(YY: ;
intYY; >
doctorIdYY? G
,YYG H
DateOnlyYYI Q
dateYYR V
)YYV W
{ZZ 	
var[[ 
result[[ 
=[[ 
await[[ 
_service[[ '
.[[' (
GetDoctorSchedule[[( 9
([[9 :
date[[: >
,[[> ?
doctorId[[@ H
)[[H I
;[[I J
return\\ 
Ok\\ 
(\\ 
result\\ 
)\\ 
;\\ 
}]] 	
[`` 	
HttpGet``	 
(`` 
$str`` 
)`` 
]``  
[aa 	
	Authorizeaa	 
(aa !
AuthenticationSchemesaa (
=aa) *
JwtBearerDefaultsaa+ <
.aa< = 
AuthenticationSchemeaa= Q
)aaQ R
]aaR S
[bb 	
	Authorizebb	 
(bb 
Rolesbb 
=bb 
$strbb $
)bb$ %
]bb% &
publiccc 
asynccc 
Taskcc 
<cc 
IActionResultcc '
>cc' (
GetMySchedulecc) 6
(cc6 7
[cc7 8
	FromQuerycc8 A
]ccA B
DateOnlyccC K
dateccL P
)ccP Q
{dd 	
varee 
patientIdClaimee 
=ee  
Useree! %
.ee% &
	FindFirstee& /
(ee/ 0
$stree0 ;
)ee; <
?ee< =
.ee= >
Valueee> C
;eeC D
ifgg 
(gg 
stringgg 
.gg 
IsNullOrEmptygg $
(gg$ %
patientIdClaimgg% 3
)gg3 4
)gg4 5
returnhh 
Unauthorizedhh #
(hh# $
)hh$ %
;hh% &
varjj 
	patientIdjj 
=jj 
intjj 
.jj  
Parsejj  %
(jj% &
patientIdClaimjj& 4
)jj4 5
;jj5 6
varll 
resultll 
=ll 
awaitll 
_servicell '
.ll' (
GetPatientSchedulell( :
(ll: ;
datell; ?
,ll? @
	patientIdllA J
)llJ K
;llK L
returnmm 
Okmm 
(mm 
resultmm 
)mm 
;mm 
}nn 	
[pp 	
HttpGetpp	 
(pp 
$strpp 
)pp 
]pp 
[qq 	
	Authorizeqq	 
(qq !
AuthenticationSchemesqq (
=qq) *
JwtBearerDefaultsqq+ <
.qq< = 
AuthenticationSchemeqq= Q
)qqQ R
]qqR S
[rr 	
	Authorizerr	 
(rr 
Rolesrr 
=rr 
$strrr $
)rr$ %
]rr% &
publicss 
asyncss 
Taskss 
<ss 
IActionResultss '
>ss' (
GetMyAppointmentsss) :
(ss: ;
)ss; <
{tt 	
varuu 
patientIdClaimuu 
=uu  
Useruu! %
.uu% &
	FindFirstuu& /
(uu/ 0
$struu0 ;
)uu; <
?uu< =
.uu= >
Valueuu> C
;uuC D
ifww 
(ww 
stringww 
.ww 
IsNullOrEmptyww $
(ww$ %
patientIdClaimww% 3
)ww3 4
)ww4 5
returnxx 
Unauthorizedxx #
(xx# $
)xx$ %
;xx% &
varzz 
	patientIdzz 
=zz 
intzz 
.zz  
Parsezz  %
(zz% &
patientIdClaimzz& 4
)zz4 5
;zz5 6
var|| 
result|| 
=|| 
await|| 
_service|| '
.||' (#
GetAppointmentByPatient||( ?
(||? @
	patientId||@ I
)||I J
;||J K
return}} 
Ok}} 
(}} 
result}} 
)}} 
;}} 
}~~ 	
[
ÅÅ 	
HttpGet
ÅÅ	 
(
ÅÅ 
$str
ÅÅ (
)
ÅÅ( )
]
ÅÅ) *
[
ÇÇ 	
	Authorize
ÇÇ	 
(
ÇÇ #
AuthenticationSchemes
ÇÇ (
=
ÇÇ) *
JwtBearerDefaults
ÇÇ+ <
.
ÇÇ< ="
AuthenticationScheme
ÇÇ= Q
)
ÇÇQ R
]
ÇÇR S
[
ÉÉ 	
	Authorize
ÉÉ	 
(
ÉÉ 
Roles
ÉÉ 
=
ÉÉ 
$str
ÉÉ #
)
ÉÉ# $
]
ÉÉ$ %
public
ÑÑ 
async
ÑÑ 
Task
ÑÑ 
<
ÑÑ 
IActionResult
ÑÑ '
>
ÑÑ' (#
GetDoctorAppointments
ÑÑ) >
(
ÑÑ> ?
int
ÑÑ? B
doctorId
ÑÑC K
)
ÑÑK L
{
ÖÖ 	
var
ÜÜ 
result
ÜÜ 
=
ÜÜ 
await
ÜÜ 
_service
ÜÜ '
.
ÜÜ' ($
GetAppointmentByDoctor
ÜÜ( >
(
ÜÜ> ?
doctorId
ÜÜ? G
)
ÜÜG H
;
ÜÜH I
return
áá 
Ok
áá 
(
áá 
result
áá 
)
áá 
;
áá 
}
àà 	
}
ää 
}ãã õ0
hC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AdminPatientController.cs
public 
class "
AdminPatientController #
:$ %
ControllerBase& 4
{ 
private		 
readonly		 
IPatientService		 $
_patientService		% 4
;		4 5
public 
"
AdminPatientController !
(! "
IPatientService" 1
patientService2 @
)@ A
{ 
_patientService 
= 
patientService (
;( )
} 
[ 
HttpGet 
( 
$str 
) 
] 
[ 
	Authorize 
( !
AuthenticationSchemes $
=% &
JwtBearerDefaults' 8
.8 9 
AuthenticationScheme9 M
)M N
]N O
[ 
	Authorize 
( 
Roles 
= 
$str 
) 
]  
public 

async 
Task 
< 
IActionResult #
># $
GetPatientById% 3
(3 4
int4 7
id8 :
): ;
{ 
var 
result 
= 
await 
_patientService *
.* +
GetByIdAsync+ 7
(7 8
id8 :
): ;
;; <
return 
Ok 
( 
result 
) 
; 
} 
[ 
HttpGet 
( 
$str 
) 
] 
[ 
	Authorize 
( !
AuthenticationSchemes $
=% &
JwtBearerDefaults' 8
.8 9 
AuthenticationScheme9 M
)M N
]N O
[ 
	Authorize 
( 
Roles 
= 
$str 
) 
]  
public   

async   
Task   
<   
IActionResult   #
>  # $
GetAllPatient  % 2
(  2 3
[  3 4
	FromQuery  4 =
]  = >
PatientFilter  ? L
filter  M S
)  S T
{!! 
if"" 

("" 
!"" 

ModelState"" 
."" 
IsValid"" 
)""  
return## 

BadRequest## 
(## 

ModelState## (
)##( )
;##) *
var%% 
result%% 
=%% 
await%% 
_patientService%% *
.%%* +
GetAllAsync%%+ 6
(%%6 7
filter%%7 =
)%%= >
;%%> ?
return&& 
Ok&& 
(&& 
result&& 
)&& 
;&& 
}'' 
[)) 
HttpPut)) 
()) 
$str)) 
))) 
])) 
[** 
	Authorize** 
(** !
AuthenticationSchemes** $
=**% &
JwtBearerDefaults**' 8
.**8 9 
AuthenticationScheme**9 M
)**M N
]**N O
[++ 
	Authorize++ 
(++ 
Roles++ 
=++ 
$str++ 
)++ 
]++  
public,, 

async,, 
Task,, 
<,, 
IActionResult,, #
>,,# $
UpdatePatient,,% 2
(,,2 3
int,,3 6
id,,7 9
,,,9 :
[,,; <
FromBody,,< D
],,D E
UpdatePatientDto,,F V
dto,,W Z
),,Z [
{-- 
if.. 

(.. 
!.. 

ModelState.. 
... 
IsValid.. 
)..  
return// 

BadRequest// 
(// 

ModelState// (
)//( )
;//) *
await11 
_patientService11 
.11 
UpdateAsync11 )
(11) *
id11* ,
,11, -
dto11. 1
)111 2
;112 3
return22 
Ok22 
(22 
)22 
;22 
}33 
[55 
	HttpPatch55 
(55 
$str55 &
)55& '
]55' (
[66 
	Authorize66 
(66 !
AuthenticationSchemes66 $
=66% &
JwtBearerDefaults66' 8
.668 9 
AuthenticationScheme669 M
)66M N
]66N O
[77 
	Authorize77 
(77 
Roles77 
=77 
$str77 
)77 
]77  
public88 

async88 
Task88 
<88 
IActionResult88 #
>88# $
UpdatePatientStatus88% 8
(888 9
int889 <
id88= ?
,88? @
[88A B
FromBody88B J
]88J K
bool88L P
isActive88Q Y
)88Y Z
{99 
await:: 
_patientService:: 
.:: 
UpdateStatusAsync:: /
(::/ 0
id::0 2
,::2 3
isActive::4 <
)::< =
;::= >
return;; 
Ok;; 
(;; 
);; 
;;; 
}<< 
[>> 

HttpDelete>> 
(>> 
$str>>  
)>>  !
]>>! "
[?? 
	Authorize?? 
(?? !
AuthenticationSchemes?? $
=??% &
JwtBearerDefaults??' 8
.??8 9 
AuthenticationScheme??9 M
)??M N
]??N O
[@@ 
	Authorize@@ 
(@@ 
Roles@@ 
=@@ 
$str@@ 
)@@ 
]@@  
publicAA 

asyncAA 
TaskAA 
<AA 
IActionResultAA #
>AA# $
DeletePatientAA% 2
(AA2 3
intAA3 6
idAA7 9
)AA9 :
{BB 
awaitCC 
_patientServiceCC 
.CC 
DeleteAsyncCC )
(CC) *
idCC* ,
)CC, -
;CC- .
returnDD 
OkDD 
(DD 
)DD 
;DD 
}EE 
}FF Ë
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AdminHealthRecordController.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Controllers $
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		 
[

 
ApiController

 
]

 
public 

class '
AdminHealthRecordController ,
:- .
ControllerBase/ =
{ 
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
public '
AdminHealthRecordController *
(* + 
IHealthRecordService+ ?
healthRecordService@ S
)S T
{ 	 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
[ 	

HttpDelete	 
( 
$str #
)# $
]$ %
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
)Q R
]R S
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
DeleteHealthRecord) ;
(; <
int< ?
id@ B
)B C
{ 	
await  
_healthRecordService &
.& '
DeleteAsync' 2
(2 3
id3 5
)5 6
;6 7
return 
Ok 
( 
) 
; 
} 	
} 
} µ2
gC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AdminDoctorController.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Controllers $
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		 
[

 
ApiController

 
]

 
public 

class !
AdminDoctorController &
:' (
ControllerBase) 7
{ 
private 
readonly 
IDoctorService '
_doctorService( 6
;6 7
public !
AdminDoctorController $
($ %
IDoctorService% 3
doctorService4 A
)A B
{ 	
_doctorService 
= 
doctorService *
;* +
} 	
[ 	
HttpGet	 
( 
$str  
)  !
]! "
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
)Q R
]R S
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
GetDoctorById) 6
(6 7
int7 :
id; =
)= >
{ 	
var 
result 
= 
await 
_doctorService -
.- .
GetByIdAsync. :
(: ;
id; =
)= >
;> ?
return 
Ok 
( 
result 
) 
; 
} 	
[!! 	
HttpGet!!	 
(!! 
$str!! 
)!! 
]!! 
["" 	
	Authorize""	 
("" !
AuthenticationSchemes"" (
="") *
JwtBearerDefaults""+ <
.""< = 
AuthenticationScheme""= Q
)""Q R
]""R S
[## 	
	Authorize##	 
(## 
Roles## 
=## 
$str## "
)##" #
]### $
public$$ 
async$$ 
Task$$ 
<$$ 
IActionResult$$ '
>$$' (
GetAllDoctor$$) 5
($$5 6
[$$6 7
	FromQuery$$7 @
]$$@ A
DoctorFilter$$B N
filter$$O U
)$$U V
{%% 	
if&& 
(&& 
!&& 

ModelState&& 
.&& 
IsValid&& #
)&&# $
return'' 

BadRequest'' !
(''! "

ModelState''" ,
)'', -
;''- .
var)) 
result)) 
=)) 
await)) 
_doctorService)) -
.))- .
GetAllAsync)). 9
())9 :
filter)): @
)))@ A
;))A B
return** 
Ok** 
(** 
result** 
)** 
;** 
}++ 	
[-- 	
HttpPut--	 
(-- 
$str--  
)--  !
]--! "
[.. 	
	Authorize..	 
(.. !
AuthenticationSchemes.. (
=..) *
JwtBearerDefaults..+ <
...< = 
AuthenticationScheme..= Q
)..Q R
]..R S
[// 	
	Authorize//	 
(// 
Roles// 
=// 
$str// "
)//" #
]//# $
public00 
async00 
Task00 
<00 
IActionResult00 '
>00' (
UpdateDoctor00) 5
(005 6
int006 9
id00: <
,00< =
[00> ?
FromBody00? G
]00G H
UpdateDoctorDto00I X
dto00Y \
)00\ ]
{11 	
if22 
(22 
!22 

ModelState22 
.22 
IsValid22 #
)22# $
return33 

BadRequest33 !
(33! "

ModelState33" ,
)33, -
;33- .
await55 
_doctorService55  
.55  !
UpdateAsync55! ,
(55, -
id55- /
,55/ 0
dto551 4
)554 5
;555 6
return66 
Ok66 
(66 
)66 
;66 
}77 	
[99 	
	HttpPatch99	 
(99 
$str99 )
)99) *
]99* +
[:: 	
	Authorize::	 
(:: !
AuthenticationSchemes:: (
=::) *
JwtBearerDefaults::+ <
.::< = 
AuthenticationScheme::= Q
)::Q R
]::R S
[;; 	
	Authorize;;	 
(;; 
Roles;; 
=;; 
$str;; "
);;" #
];;# $
public<< 
async<< 
Task<< 
<<< 
IActionResult<< '
><<' (
UpdateDoctorStatus<<) ;
(<<; <
int<<< ?
id<<@ B
,<<B C
[<<D E
FromBody<<E M
]<<M N
bool<<O S
isActive<<T \
)<<\ ]
{== 	
await>> 
_doctorService>>  
.>>  !
UpdateStatusAsync>>! 2
(>>2 3
id>>3 5
,>>5 6
isActive>>7 ?
)>>? @
;>>@ A
return?? 
Ok?? 
(?? 
)?? 
;?? 
}@@ 	
[BB 	

HttpDeleteBB	 
(BB 
$strBB #
)BB# $
]BB$ %
[CC 	
	AuthorizeCC	 
(CC !
AuthenticationSchemesCC (
=CC) *
JwtBearerDefaultsCC+ <
.CC< = 
AuthenticationSchemeCC= Q
)CCQ R
]CCR S
[DD 	
	AuthorizeDD	 
(DD 
RolesDD 
=DD 
$strDD "
)DD" #
]DD# $
publicEE 
asyncEE 
TaskEE 
<EE 
IActionResultEE '
>EE' (
DeleteDoctorEE) 5
(EE5 6
intEE6 9
idEE: <
)EE< =
{FF 	
awaitGG 
_doctorServiceGG  
.GG  !
DeleteAsyncGG! ,
(GG, -
idGG- /
)GG/ 0
;GG0 1
returnHH 
OkHH 
(HH 
)HH 
;HH 
}II 	
}JJ 
}KK Ö
lC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AdminAppointmentController.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Controllers $
{		 
[

 
Route

 

(


 
$str

 
)

 
]

 
[ 
ApiController 
] 
public 

class &
AdminAppointmentController +
:, -
ControllerBase. <
{ 
private 
readonly 
IAppointmentService ,
_appointmentService- @
;@ A
public &
AdminAppointmentController )
() *
IAppointmentService* =
appointmentService> P
)P Q
{ 	
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
[ 	
HttpGet	 
( 
$str  
)  !
]! "
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
)Q R
]R S
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
GetAllAppointment) :
(: ;
[; <
	FromQuery< E
]E F
AppointmentFilterG X
filterY _
)_ `
{ 	
if 
( 
! 

ModelState 
. 
IsValid #
)# $
return 

BadRequest !
(! "

ModelState" ,
), -
;- .
var 
result 
= 
await 
_appointmentService 2
.2 3
GetAllAsync3 >
(> ?
filter? E
)E F
;F G
return 
Ok 
( 
result 
) 
; 
}   	
["" 	
HttpGet""	 
("" 
$str"" '
)""' (
]""( )
[## 	
	Authorize##	 
(## !
AuthenticationSchemes## (
=##) *
JwtBearerDefaults##+ <
.##< = 
AuthenticationScheme##= Q
)##Q R
]##R S
[$$ 	
	Authorize$$	 
($$ 
Roles$$ 
=$$ 
$str$$ "
)$$" #
]$$# $
public%% 
async%% 
Task%% 
<%% 
IActionResult%% '
>%%' (
GetDailyReport%%) 7
(%%7 8
)%%8 9
{&& 	
var'' 
result'' 
='' 
await'' 
_appointmentService'' 2
.''2 3
GetDailyReport''3 A
(''A B
)''B C
;''C D
return(( 
Ok(( 
((( 
result(( 
)(( 
;(( 
})) 	
}** 
}++ 