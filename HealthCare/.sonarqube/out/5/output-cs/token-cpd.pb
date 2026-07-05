≠
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
? 
> 
GetByIdAsync *
(* +
int+ .
id/ 1
)1 2
;2 3
Task		 
<		 
PagedResult		 
<		 
PatientListDto		 '
?		' (
>		( )
>		) *
GetAllAsync		+ 6
(		6 7
PatientFilter		7 D
filter		E K
)		K L
;		L M
Task

 
AddAsync

 
(

 
CreatePatientDto

 &
dto

' *
)

* +
;

+ ,
Task 
UpdateAsync 
( 
int 
id 
,  
UpdatePatientDto  0
dto1 4
)4 5
;5 6
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
Task 
< 
IEnumerable 
< 
PatientListDto '
>' (
>( )
SearchByNameAsync* ;
(; <
string< B
nameC G
)G H
;H I
Task 
UpdateStatusAsync 
( 
int "
id# %
,% &
bool' +
isActive, 4
)4 5
;5 6
} 
} ¡
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
,		/ 0
int		0 3
doctorId		4 <
)		< =
;		= >
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
?  !
>! "
GetByIdAsync# /
(/ 0
int0 3
id4 6
)6 7
;7 8
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
intA D
	patientIdE N
)N O
;O P
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
} í
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
;O P
Task 
< 
List 
< 
CreateLeaveDto  
>  !
>! "$
GetLeavesByDoctorIdAsync# ;
(; <
int< ?
doctorId@ H
)H I
;I J
Task 
< 
DoctorListDto 
> 
GetMyProfileAsync -
(- .
int. 1
doctorId2 :
): ;
;; <
} 
} ‘	
fC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Interfaces\IAuthService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface 
IAuthService !
{		 
Task  
RegisterPatientAsync !
(! "
CreatePatientDto" 2
dto3 6
)6 7
;7 8
Task 
RegisterDoctorAsync  
(  !
DoctorRegisterDto! 2
dto3 6
)6 7
;7 8
Task 
< 
AuthorResponseDto 
> 

LoginAsync  *
(* +
LoginDto+ 3
dto4 7
)7 8
;8 9
Task 
ChangePasswordAsync  
(  !
string! '
userId( .
,. /
ChangePasswordDto0 A
dtoB E
)E F
;F G
Task 
< 
bool 
> 
EmailExistsAsync #
(# $
string$ *
email+ 0
)0 1
;1 2
} 
} Ñ
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Interfaces\IAppointmentService.cs
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
	interface 
IAppointmentService (
{ 
Task 
AddAsync 
(  
CreateAppointmentDto *
dto+ .
,. /
int/ 2
id3 5
)5 6
;6 7
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
Task 
UpdateAsync 
( 
int 
id 
,   
UpdateAppointmentDto  4
dto5 8
)8 9
;9 :
Task 
UpdateStatusAsync 
( 
int "
id# %
,% & 
UpdateAppointmentDto' ;
dto< ?
)? @
;@ A
Task 
< 
AppointmentListDto 
?  
>  !
GetByIdAsync" .
(. /
int/ 2
id3 5
)5 6
;6 7
Task 
< 
PagedResult 
< 
AppointmentListDto +
>+ ,
>, -
GetAllAsync. 9
(9 :
AppointmentFilter: K
filterL R
)R S
;S T
Task 
< 
bool 
> 
IsAvailable 
( 
DateOnly '
date( ,
,, -
int. 1
doctorId2 :
,: ;
string< B
timeSlotC K
)K L
;L M
Task 
< 
List 
<  
AppointmentReportDto &
>& '
>' (
GetDailyReport) 7
(7 8
DateOnly8 @
	startDateA J
,J K
DateOnlyL T
endDateU \
)\ ]
;] ^
Task 
< 
List 
< 
string 
> 
> 
AvailableTimeSlots -
(- .
DateOnly. 6
date7 ;
,; <
int= @
doctorIdA I
)I J
;J K
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &
GetDoctorSchedule' 8
(8 9
DateOnly9 A
dateB F
,F G
intH K
idL N
)N O
;O P
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &
GetPatientSchedule' 9
(9 :
DateOnly: B
dateC G
,G H
intI L
idM O
)O P
;P Q
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &#
GetAppointmentByPatient' >
(> ?
int? B
idC E
)E F
;F G
Task 
< 
List 
< 
AppointmentListDto $
>$ %
>% &"
GetAppointmentByDoctor' =
(= >
int> A
idB D
)D E
;E F
Task *
CancelAppointmentsByDoctorDate +
(+ ,
int, /
doctorId0 8
,8 9
DateOnly: B
dateC G
)G H
;H I
Task 
< !
AppointmentSummaryDto "
>" #
GetSummaryAsync$ 3
(3 4
)4 5
;5 6
} 
} àT
mC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\PatientService.cs
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
class 
PatientService 
:  !
IPatientService" 1
{ 
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_repository. 9
;9 :
private 
readonly 
IPatientRepository +
?+ ,
_patientRepository- ?
;? @
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
private 
readonly 
IMapper  
_mapper! (
;( )
public 
PatientService 
( 
IRepository )
<) *
Patient* 1
>1 2

repository3 =
,= >
HealthCareDbContext? R
contextS Z
,Z [
IMapper\ c
mapperd j
,j k
IPatientRepositoryl ~
patientRepositor	 è
)
è ê
{ 	
_patientRepository 
=  
patientRepositor! 1
;1 2
_repository 
= 

repository $
;$ %
_context 
= 
context 
; 
_mapper 
= 
mapper 
; 
} 	
public 
async 
Task 
AddAsync "
(" #
CreatePatientDto# 3
dto4 7
)7 8
{ 	
var 
patient 
= 
_mapper !
.! "
Map" %
<% &
Patient& -
>- .
(. /
dto/ 2
)2 3
;3 4
await   
_repository   
.   
AddAsync   &
(  & '
patient  ' .
)  . /
;  / 0
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
UpdatePatientDto$$. >
dto$$? B
)$$B C
{%% 	
var&& 
patient&& 
=&& 
await&& 
_repository&&  +
.&&+ ,
GetProfileAsync&&, ;
(&&; <
id&&< >
)&&> ?
;&&? @
if'' 
('' 
patient'' 
=='' 
null'' 
)''  
throw(( 
new(( $
PatientNotFoundException(( 2
(((2 3
$str((3 F
)((F G
;((G H
_mapper)) 
.)) 
Map)) 
()) 
dto)) 
,)) 
patient)) $
)))$ %
;))% &
await++ 
_repository++ 
.++ 
UpdateAsync++ )
(++) *
patient++* 1
)++1 2
;++2 3
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
var11 
patient11 
=11 
await11 
_repository11  +
.11+ ,
GetProfileAsync11, ;
(11; <
id11< >
)11> ?
;11? @
if22 
(22 
patient22 
==22 
null22 
)22  
throw33 
new33 $
PatientNotFoundException33 2
(332 3
$str333 F
)33F G
;33G H
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
<88 
PatientListDto88 (
?88( )
>88) *
GetByIdAsync88+ 7
(887 8
int888 ;
id88< >
)88> ?
{99 	
var:: 
patient:: 
=:: 
await:: 
_patientRepository::  2
.::2 3
GetMyProfileAsync::3 D
(::D E
id::E G
)::G H
;::H I
if<< 
(<< 
patient<< 
==<< 
null<< 
)<<  
throw== 
new== $
PatientNotFoundException== 2
(==2 3
$str==3 F
)==F G
;==G H
return?? 
patient?? 
;?? 
}@@ 	
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
?KK4 5
>KK5 6
>KK6 7
GetAllAsyncKK8 C
(KKC D
PatientFilterKKD Q
filterKKR X
)KKX Y
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
?ff1 2
>ff2 3
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
.qq+ ,
GetProfileAsyncqq, ;
(qq; <
idqq< >
)qq> ?
;qq? @
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
}zz 	
}|| 
}}} ∆Q
rC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\HealthRecordService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
HealthRecordService $
:% & 
IHealthRecordService' ;
{ 
private 
readonly #
IHealthRecordRepository 0
_repository1 <
;< =
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
public 
HealthRecordService "
(" ##
IHealthRecordRepository# :

repository; E
,E F
IMapperG N
mapperO U
,U V
HealthCareDbContextW j
contextk r
)r s
{ 	
_repository 
= 

repository $
;$ %
_mapper 
= 
mapper 
; 
_context 
= 
context 
; 
} 	
public 
async 
Task 
AddAsync "
(" #!
CreateHealthRecordDto# 8
dto9 <
,< =
int> A
doctorIdB J
)J K
{   	
var!! 
appointment!! 
=!! 
await!! #
_context!!$ ,
.!!, -
Appointments!!- 9
."" 
FirstOrDefaultAsync"" $
(""$ %
a""% &
=>""' )
a""* +
.""+ ,
AppointmentId"", 9
=="": <
dto""= @
.""@ A
AppointmentId""A N
)""N O
;""O P
if$$ 
($$ 
appointment$$ 
==$$ 
null$$ #
)$$# $
throw%% 
new%% %
InvalidOperationException%% 3
(%%3 4
$str%%4 I
)%%I J
;%%J K
var'' 
record'' 
='' 
_mapper''  
.''  !
Map''! $
<''$ %
HealthRecord''% 1
>''1 2
(''2 3
dto''3 6
)''6 7
;''7 8
record)) 
.)) 
	PatientId)) 
=)) 
appointment)) *
.))* +
	PatientId))+ 4
;))4 5
record** 
.** 
DoctorId** 
=** 
doctorId** &
;**& '
await,, 
_context,, 
.,, 
HealthRecords,, (
.,,( )
AddAsync,,) 1
(,,1 2
record,,2 8
),,8 9
;,,9 :
await-- 
_context-- 
.-- 
SaveChangesAsync-- +
(--+ ,
)--, -
;--- .
}.. 	
public00 
async00 
Task00 
UpdateAsync00 %
(00% &
int00& )
id00* ,
,00, -!
UpdateHealthRecordDto00- B
dto00C F
)00F G
{11 	
var22 
record22 
=22 
await22 
_repository22 *
.22* +
GetProfileAsync22+ :
(22: ;
id22; =
)22= >
;22> ?
if33 
(33 
record33 
==33 
null33 
)33 
throw44 
new44 )
HealthRecordNotFoundException44 7
(447 8
id448 :
)44: ;
;44; <
_mapper66 
.66 
Map66 
(66 
dto66 
,66 
record66 "
)66" #
;66# $
await88 
_repository88 
.88 
UpdateAsync88 )
(88) *
record88* 0
)880 1
;881 2
await99 
_context99 
.99 
SaveChangesAsync99 +
(99+ ,
)99, -
;99- .
}:: 	
public== 
async== 
Task== 
DeleteAsync== %
(==% &
int==& )
id==* ,
)==, -
{>> 	
var@@ 
record@@ 
=@@ 
await@@ 
_repository@@ *
.@@* +
GetProfileAsync@@+ :
(@@: ;
id@@; =
)@@= >
;@@> ?
ifAA 
(AA 
recordAA 
==AA 
nullAA 
)AA 
throwBB 
newBB )
HealthRecordNotFoundExceptionBB 7
(BB7 8
idBB8 :
)BB: ;
;BB; <
awaitCC 
_repositoryCC 
.CC 
DeleteAsyncCC )
(CC) *
idCC* ,
)CC, -
;CC- .
awaitDD 
_contextDD 
.DD 
SaveChangesAsyncDD +
(DD+ ,
)DD, -
;DD- .
}EE 	
publicGG 
asyncGG 
TaskGG 
<GG 
HealthRecordListDtoGG -
?GG- .
>GG. /
GetByIdAsyncGG0 <
(GG< =
intGG= @
idGGA C
)GGC D
{HH 	
varII 
recordII 
=II 
awaitII 
_repositoryII *
.II* +
GetProfileAsyncII+ :
(II: ;
idII; =
)II= >
;II> ?
returnJJ 
recordJJ 
==JJ 
nullJJ !
?JJ" #
nullJJ$ (
:JJ) *
_mapperJJ+ 2
.JJ2 3
MapJJ3 6
<JJ6 7
HealthRecordListDtoJJ7 J
>JJJ K
(JJK L
recordJJL R
)JJR S
;JJS T
}KK 	
publicMM 
asyncMM 
TaskMM 
<MM 
PagedResultMM %
<MM% &
HealthRecordListDtoMM& 9
>MM9 :
>MM: ;
GetAllAsyncMM< G
(MMG H
HealthRecordFilterMMH Z
filterMM[ a
)MMa b
{NN 	

ExpressionPP 
<PP 
FuncPP 
<PP 
HealthRecordPP (
,PP( )
boolPP* .
>PP. /
>PP/ 0
?PP0 1
	predicatePP2 ;
=PP< =
nullPP> B
;PPB C
ifRR 
(RR 
filterRR 
.RR 
	VisitDateRR  
.RR  !
HasValueRR! )
)RR) *
{SS 
varTT 
startTT 
=TT 
filterTT "
.TT" #
	VisitDateTT# ,
.TT, -
ValueTT- 2
.TT2 3

ToDateTimeTT3 =
(TT= >
TimeOnlyTT> F
.TTF G
MinValueTTG O
)TTO P
;TTP Q
varUU 
endUU 
=UU 
startUU 
.UU  
AddDaysUU  '
(UU' (
$numUU( )
)UU) *
;UU* +
	predicateWW 
=WW 
hrWW 
=>WW !
hrWW" $
.WW$ %
	VisitDateWW% .
>=WW/ 1
startWW2 7
&&WW8 :
hrWW; =
.WW= >
	VisitDateWW> G
<WWH I
endWWJ M
;WWM N
}XX 
Func[[ 
<[[ 

IQueryable[[ 
<[[ 
HealthRecord[[ (
>[[( )
,[[) *
IOrderedQueryable[[+ <
<[[< =
HealthRecord[[= I
>[[I J
>[[J K
orderBy[[L S
=[[T U
q\\ 
=>\\ 
q\\ 
.\\ 
OrderBy\\ 
(\\ 
hr\\ !
=>\\" $
hr\\% '
.\\' (
	VisitDate\\( 1
)\\1 2
;\\2 3
var__ 
pagedResult__ 
=__ 
await__ #
_repository__$ /
.__/ 0
GetAllAsync__0 ;
(__; <
filter`` 
.`` 

PageNumber`` !
,``! "
filteraa 
.aa 
PageSizeaa 
,aa  
	predicatebb 
,bb 
orderBycc 
)dd 
;dd 
returngg 
newgg 
PagedResultgg "
<gg" #
HealthRecordListDtogg# 6
>gg6 7
{hh 
Itemsii 
=ii 
_mapperii 
.ii  
Mapii  #
<ii# $
IEnumerableii$ /
<ii/ 0
HealthRecordListDtoii0 C
>iiC D
>iiD E
(iiE F
pagedResultiiF Q
.iiQ R
ItemsiiR W
)iiW X
,iiX Y

PageNumberjj 
=jj 
pagedResultjj (
.jj( )

PageNumberjj) 3
,jj3 4
PageSizekk 
=kk 
pagedResultkk &
.kk& '
PageSizekk' /
,kk/ 0

TotalCountll 
=ll 
pagedResultll (
.ll( )

TotalCountll) 3
}mm 
;mm 
}nn 	
publicqq 
asyncqq 
Taskqq 
<qq 
Listqq 
<qq 
HealthRecordListDtoqq 2
>qq2 3
>qq3 4$
GetHealthRecordByPatientqq5 M
(qqM N
intqqN Q
	patientIdqqR [
)qq[ \
{rr 	
varss 
recordsss 
=ss 
awaitss 
_repositoryss  +
.ss+ ,$
GetHealthRecordByPatientss, D
(ssD E
	patientIdssE N
)ssN O
;ssO P
returnuu 
_mapperuu 
.uu 
Mapuu 
<uu 
Listuu #
<uu# $
HealthRecordListDtouu$ 7
>uu7 8
>uu8 9
(uu9 :
recordsuu: A
)uuA B
;uuB C
}vv 	
publicxx 
asyncxx 
Taskxx 
<xx 
Listxx 
<xx 
HealthRecordListDtoxx 2
>xx2 3
>xx3 4(
GetHealthRecordByAppointmentxx5 Q
(xxQ R
intxxR U
idxxV X
)xxX Y
{yy 	
varzz 
recordszz 
=zz 
awaitzz 
_repositoryzz  +
.zz+ ,(
GetHealthRecordByAppointmentzz, H
(zzH I
idzzI K
)zzK L
;zzL M
return|| 
_mapper|| 
.|| 
Map|| 
<|| 
List|| #
<||# $
HealthRecordListDto||$ 7
>||7 8
>||8 9
(||9 :
records||: A
)||A B
;||B C
}}} 	
} 
}ÄÄ Ä§
lC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\DoctorService.cs
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
class 
DoctorService 
:  
IDoctorService! /
{ 
private 
readonly 
IDoctorRepository *
_repository+ 6
;6 7
private 
readonly "
IAppointmentRepository /"
_appointmentRepository0 F
;F G
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
private 
readonly 
IMapper  
_mapper! (
;( )
public 
DoctorService 
( 
IDoctorRepository .

repository/ 9
,9 :
HealthCareDbContext; N
contextO V
,V W
IMapperX _
mapper` f
,f g"
IAppointmentRepositoryh ~"
appointmentRepository	 î
)
î ï
{ 	
_repository 
= 

repository $
;$ %
_context 
= 
context 
; 
_mapper 
= 
mapper 
; "
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
} 	
public 
async 
Task 
AddAsync "
(" #
DoctorRegisterDto# 4
dto5 8
)8 9
{ 	
var   
doctor   
=   
_mapper    
.    !
Map  ! $
<  $ %
Doctor  % +
>  + ,
(  , -
dto  - 0
)  0 1
;  1 2
await!! 
_repository!! 
.!! 
AddAsync!! &
(!!& '
doctor!!' -
)!!- .
;!!. /
await"" 
_context"" 
."" 
SaveChangesAsync"" +
(""+ ,
)"", -
;""- .
}## 	
public%% 
async%% 
Task%% 
UpdateAsync%% %
(%%% &
int%%& )
id%%* ,
,%%, -
UpdateDoctorDto%%. =
dto%%> A
)%%A B
{&& 	
var'' 
doctor'' 
='' 
await'' 
_repository'' *
.''* +
GetProfileAsync''+ :
('': ;
id''; =
)''= >
;''> ?
if(( 
((( 
doctor(( 
==(( 
null(( 
)(( 
throw)) 
new)) #
DoctorNotFoundException)) 1
())1 2
$str))2 D
)))D E
;))E F
_mapper** 
.** 
Map** 
(** 
dto** 
,** 
doctor** #
)**# $
;**$ %
await,, 
_repository,, 
.,, 
UpdateAsync,, )
(,,) *
doctor,,* 0
),,0 1
;,,1 2
await-- 
_context-- 
.-- 
SaveChangesAsync-- +
(--+ ,
)--, -
;--- .
}.. 	
public00 
async00 
Task00 
DeleteAsync00 %
(00% &
int00& )
id00* ,
)00, -
{11 	
var22 
doctor22 
=22 
await22 
_repository22 *
.22* +
GetProfileAsync22+ :
(22: ;
id22; =
)22= >
;22> ?
if33 
(33 
doctor33 
==33 
null33 
)33 
throw44 
new44 #
DoctorNotFoundException44 1
(441 2
$str442 D
)44D E
;44E F
await55 
_repository55 
.55 
DeleteAsync55 )
(55) *
id55* ,
)55, -
;55- .
await66 
_context66 
.66 
SaveChangesAsync66 +
(66+ ,
)66, -
;66- .
}77 	
public99 
async99 
Task99 
<99 
DoctorListDto99 '
?99' (
>99( )
GetByIdAsync99* 6
(996 7
int997 :
id99; =
)99= >
{:: 	
var;; 
doctor;; 
=;; 
await;; 
_repository;; *
.;;* +
GetProfileAsync;;+ :
(;;: ;
id;;; =
);;= >
;;;> ?
return<< 
doctor<< 
==<< 
null<< !
?<<" #
null<<$ (
:<<) *
_mapper<<+ 2
.<<2 3
Map<<3 6
<<<6 7
DoctorListDto<<7 D
?<<D E
><<E F
(<<F G
doctor<<G M
)<<M N
;<<N O
}== 	
public?? 
async?? 
Task?? 
<?? 
PagedResult?? %
<??% &
DoctorListDto??& 3
>??3 4
>??4 5
GetAllAsync??6 A
(??A B
DoctorFilter??B N
filter??O U
)??U V
{@@ 	

IQueryableAA 
<AA 
DoctorAA 
>AA 
queryAA $
=AA% &
_contextAA' /
.AA/ 0
DoctorsAA0 7
.AA7 8
AsQueryableAA8 C
(AAC D
)AAD E
;AAE F
ifDD 
(DD 
!DD 
stringDD 
.DD 
IsNullOrWhiteSpaceDD *
(DD* +
filterDD+ 1
.DD1 2
FullNameDD2 :
)DD: ;
)DD; <
{EE 
queryFF 
=FF 
queryFF 
.FF 
WhereFF #
(FF# $
dFF$ %
=>FF& (
dFF) *
.FF* +
FullNameFF+ 3
.FF3 4
ContainsFF4 <
(FF< =
filterFF= C
.FFC D
FullNameFFD L
)FFL M
)FFM N
;FFN O
}GG 
ifJJ 
(JJ 
!JJ 
stringJJ 
.JJ 
IsNullOrWhiteSpaceJJ *
(JJ* +
filterJJ+ 1
.JJ1 2
SpecialisationJJ2 @
)JJ@ A
)JJA B
{KK 
queryLL 
=LL 
queryLL 
.LL 
WhereLL #
(LL# $
dLL$ %
=>LL& (
dLL) *
.LL* +
SpecialisationLL+ 9
==LL: <
filterLL= C
.LLC D
SpecialisationLLD R
)LLR S
;LLS T
}MM 
ifPP 
(PP 
filterPP 
.PP 
MinExperiencePP $
.PP$ %
HasValuePP% -
)PP- .
{QQ 
queryRR 
=RR 
queryRR 
.RR 
WhereRR #
(RR# $
dRR$ %
=>RR& (
dRR) *
.RR* +
YearsOfExperienceRR+ <
>=RR= ?
filterRR@ F
.RRF G
MinExperienceRRG T
.RRT U
ValueRRU Z
)RRZ [
;RR[ \
}SS 
ifVV 
(VV 
filterVV 
.VV 
IsActiveVV 
.VV  
HasValueVV  (
)VV( )
{WW 
queryXX 
=XX 
queryXX 
.XX 
WhereXX #
(XX# $
dXX$ %
=>XX& (
dXX) *
.XX* +
IsActiveXX+ 3
==XX4 6
filterXX7 =
.XX= >
IsActiveXX> F
.XXF G
ValueXXG L
)XXL M
;XXM N
}YY 
var[[ 

totalCount[[ 
=[[ 
await[[ "
query[[# (
.[[( )

CountAsync[[) 3
([[3 4
)[[4 5
;[[5 6
var]] 
items]] 
=]] 
await]] 
query]] #
.^^ 
OrderByDescending^^  
(^^  !
d^^! "
=>^^# %
d^^& '
.^^' (
YearsOfExperience^^( 9
)^^9 :
.__ 
Skip__ 
(__ 
(__ 
filter__ 
.__ 

PageNumber__ &
-__' (
$num__) *
)__* +
*__, -
filter__. 4
.__4 5
PageSize__5 =
)__= >
.`` 
Take`` 
(`` 
filter`` 
.`` 
PageSize`` #
)``# $
.aa 
ToListAsyncaa 
(aa 
)aa 
;aa 
returncc 
newcc 
PagedResultcc "
<cc" #
DoctorListDtocc# 0
>cc0 1
{dd 
Itemsee 
=ee 
_mapperee 
.ee  
Mapee  #
<ee# $
IEnumerableee$ /
<ee/ 0
DoctorListDtoee0 =
>ee= >
>ee> ?
(ee? @
itemsee@ E
)eeE F
,eeF G

PageNumberff 
=ff 
filterff #
.ff# $

PageNumberff$ .
,ff. /
PageSizegg 
=gg 
filtergg !
.gg! "
PageSizegg" *
,gg* +

TotalCounthh 
=hh 

totalCounthh '
}ii 
;ii 
}jj 	
publickk 
asynckk 
Taskkk 
UpdateStatusAsynckk +
(kk+ ,
intkk, /
idkk0 2
,kk2 3
boolkk4 8
isActivekk9 A
)kkA B
{ll 	
varmm 
doctormm 
=mm 
awaitmm 
_repositorymm *
.mm* +
GetProfileAsyncmm+ :
(mm: ;
idmm; =
)mm= >
;mm> ?
ifoo 
(oo 
doctoroo 
isoo 
nulloo 
)oo 
throwpp 
newpp %
InvalidOperationExceptionpp 3
(pp3 4
$strpp4 G
)ppG H
;ppH I
doctorrr 
.rr 
IsActiverr 
=rr 
isActiverr &
;rr& '
awaittt 
_repositorytt 
.tt 
UpdateAsynctt )
(tt) *
doctortt* 0
)tt0 1
;tt1 2
awaituu 
_contextuu 
.uu 
SaveChangesAsyncuu +
(uu+ ,
)uu, -
;uu- .
}vv 	
publicxx 
asyncxx 
Taskxx 
<xx 
Listxx 
<xx 
stringxx %
>xx% &
>xx& '
GetSlotsxx( 0
(xx0 1
intxx1 4
doctorIdxx5 =
)xx= >
{yy 	
varzz 
slotszz 
=zz 
awaitzz 
_repositoryzz )
.zz) *
GetSlotszz* 2
(zz2 3
doctorIdzz3 ;
)zz; <
;zz< =
if|| 
(|| 
slots|| 
.|| 
Count|| 
==|| 
$num||  
)||  !
throw}} 
new}} %
InvalidOperationException}} 3
(}}3 4
$str}}4 _
)}}_ `
;}}` a
return 
slots 
; 
}
ÄÄ 	
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
CreateSlots
ÇÇ %
(
ÇÇ% &
int
ÇÇ& )
id
ÇÇ* ,
,
ÇÇ, -
List
ÇÇ. 2
<
ÇÇ2 3
string
ÇÇ3 9
>
ÇÇ9 :
	timeslots
ÇÇ; D
)
ÇÇD E
{
ÉÉ 	
await
ÑÑ 
_repository
ÑÑ 
.
ÑÑ 
CreateSlots
ÑÑ )
(
ÑÑ) *
id
ÑÑ* ,
,
ÑÑ, -
	timeslots
ÑÑ. 7
)
ÑÑ7 8
;
ÑÑ8 9
await
ÖÖ 
_context
ÖÖ 
.
ÖÖ 
SaveChangesAsync
ÖÖ +
(
ÖÖ+ ,
)
ÖÖ, -
;
ÖÖ- .
}
ÜÜ 	
public
àà 
async
àà 
Task
àà 
<
àà 
List
àà 
<
àà 
string
àà %
>
àà% &
>
àà& '%
AvailableTimeSlotsCheck
àà( ?
(
àà? @
DateOnly
àà@ H
date
ààI M
,
ààM N
int
ààO R
doctorId
ààS [
)
àà[ \
{
ââ 	
var
ää 
allSlots
ää 
=
ää 
await
ää  
_repository
ää! ,
.
ää, -
GetSlots
ää- 5
(
ää5 6
doctorId
ää6 >
)
ää> ?
;
ää? @
var
ãã 
bookedSlots
ãã 
=
ãã 
await
ãã #$
_appointmentRepository
ãã$ :
.
ãã: ; 
AvailableTimeSlots
ãã; M
(
ããM N
date
ããN R
,
ããR S
doctorId
ããT \
)
ãã\ ]
;
ãã] ^
return
åå 
allSlots
åå 
.
åå 
Except
åå "
(
åå" #
bookedSlots
åå# .
)
åå. /
.
åå/ 0
ToList
åå0 6
(
åå6 7
)
åå7 8
;
åå8 9
}
çç 	
public
èè 
async
èè 
Task
èè 
<
èè "
CreateLeaveResultDto
èè .
>
èè. /
CreateLeave
èè0 ;
(
èè; <
int
èè< ?
id
èè@ B
,
èèB C
List
èèD H
<
èèH I
CreateLeaveDto
èèI W
>
èèW X
leaves
èèY _
)
èè_ `
{
êê 	
var
ëë 
result
ëë 
=
ëë 
new
ëë "
CreateLeaveResultDto
ëë 1
(
ëë1 2
)
ëë2 3
;
ëë3 4
var
íí 
existingLeaves
íí 
=
íí  
await
íí! &
_repository
íí' 2
.
íí2 3!
GetLeavesByDoctorId
íí3 F
(
ííF G
id
ííG I
)
ííI J
;
ííJ K
var
ìì  
existingLeaveDates
ìì "
=
ìì# $
existingLeaves
ìì% 3
.
ìì3 4
Select
ìì4 :
(
ìì: ;
l
ìì; <
=>
ìì= ?
l
ìì@ A
.
ììA B
	LeaveDate
ììB K
)
ììK L
.
ììL M
	ToHashSet
ììM V
(
ììV W
)
ììW X
;
ììX Y
var
ïï 
leavesToCreate
ïï 
=
ïï  
new
ïï! $
List
ïï% )
<
ïï) *
CreateLeaveDto
ïï* 8
>
ïï8 9
(
ïï9 :
)
ïï: ;
;
ïï; <
foreach
óó 
(
óó 
var
óó 
leave
óó 
in
óó !
leaves
óó" (
)
óó( )
{
òò 
if
ôô 
(
ôô  
existingLeaveDates
ôô &
.
ôô& '
Contains
ôô' /
(
ôô/ 0
leave
ôô0 5
.
ôô5 6
	LeaveDate
ôô6 ?
)
ôô? @
)
ôô@ A
{
öö 
result
õõ 
.
õõ 
SkippedDates
õõ '
.
õõ' (
Add
õõ( +
(
õõ+ ,
leave
õõ, 1
.
õõ1 2
	LeaveDate
õõ2 ;
)
õõ; <
;
õõ< =
continue
úú 
;
úú 
}
ùù 
var
üü 
availableSlots
üü "
=
üü# $
await
üü% *%
AvailableTimeSlotsCheck
üü+ B
(
üüB C
leave
üüC H
.
üüH I
	LeaveDate
üüI R
,
üüR S
id
üüT V
)
üüV W
;
üüW X
var
†† 
allSlots
†† 
=
†† 
await
†† $
GetSlots
††% -
(
††- .
id
††. 0
)
††0 1
;
††1 2
if
¢¢ 
(
¢¢ 
availableSlots
¢¢ "
.
¢¢" #
Count
¢¢# (
!=
¢¢) +
allSlots
¢¢, 4
.
¢¢4 5
Count
¢¢5 :
)
¢¢: ;
{
££ 
await
•• $
_appointmentRepository
•• 0
.
••0 1,
CancelAppointmentsByDoctorDate
••1 O
(
••O P
id
••P R
,
••R S
leave
••T Y
.
••Y Z
	LeaveDate
••Z c
)
••c d
;
••d e
await
¶¶ 
_context
¶¶ "
.
¶¶" #
SaveChangesAsync
¶¶# 3
(
¶¶3 4
)
¶¶4 5
;
¶¶5 6
}
®® 
leavesToCreate
™™ 
.
™™ 
Add
™™ "
(
™™" #
leave
™™# (
)
™™( )
;
™™) *
}
´´ 
if
≠≠ 
(
≠≠ 
leavesToCreate
≠≠ 
.
≠≠ 
Count
≠≠ $
>
≠≠% &
$num
≠≠' (
)
≠≠( )
{
ÆÆ 
await
ØØ 
_repository
ØØ !
.
ØØ! "
CreateLeaves
ØØ" .
(
ØØ. /
id
ØØ/ 1
,
ØØ1 2
leavesToCreate
ØØ3 A
)
ØØA B
;
ØØB C
await
∞∞ 
_context
∞∞ 
.
∞∞ 
SaveChangesAsync
∞∞ /
(
∞∞/ 0
)
∞∞0 1
;
∞∞1 2
}
±± 
return
≥≥ 
result
≥≥ 
;
≥≥ 
}
¥¥ 	
public
∂∂ 
async
∂∂ 
Task
∂∂ 
<
∂∂ 
List
∂∂ 
<
∂∂ 
CreateLeaveDto
∂∂ -
>
∂∂- .
>
∂∂. /&
GetLeavesByDoctorIdAsync
∂∂0 H
(
∂∂H I
int
∂∂I L
doctorId
∂∂M U
)
∂∂U V
{
∑∑ 	
var
∏∏ 
leaves
∏∏ 
=
∏∏ 
await
∏∏ 
_repository
∏∏ *
.
∏∏* +!
GetLeavesByDoctorId
∏∏+ >
(
∏∏> ?
doctorId
∏∏? G
)
∏∏G H
;
∏∏H I
return
∫∫ 
leaves
∫∫ 
.
ªª 
OrderBy
ªª 
(
ªª 
l
ªª 
=>
ªª 
l
ªª 
.
ªª  
	LeaveDate
ªª  )
)
ªª) *
.
ªª* +
Select
ªª+ 1
(
ªª1 2
l
ªª2 3
=>
ªª4 6
new
ªª7 :
CreateLeaveDto
ªª; I
{
ºº 
	LeaveDate
ΩΩ 
=
ΩΩ 
l
ΩΩ  !
.
ΩΩ! "
	LeaveDate
ΩΩ" +
,
ΩΩ+ ,
Reason
ææ 
=
ææ 
l
ææ 
.
ææ 
Reason
ææ %
}
øø 
)
øø 
.
¿¿ 
ToList
¿¿ 
(
¿¿ 
)
¿¿ 
;
¿¿ 
}
¡¡ 	
public
¬¬ 
async
¬¬ 
Task
¬¬ 
<
¬¬ 
DoctorListDto
¬¬ '
>
¬¬' (
GetMyProfileAsync
¬¬) :
(
¬¬: ;
int
¬¬; >
doctorId
¬¬? G
)
¬¬G H
{
√√ 	
var
ƒƒ 
doctor
ƒƒ 
=
ƒƒ 
await
ƒƒ 
(
ƒƒ  
from
≈≈ 
d
≈≈ 
in
≈≈ 
_context
≈≈ "
.
≈≈" #
Doctors
≈≈# *
join
∆∆ 
u
∆∆ 
in
∆∆ 
_context
∆∆ "
.
∆∆" #
Users
∆∆# (
on
«« 
d
«« 
.
«« 
UserId
«« 
equals
««  &
u
««' (
.
««( )
Id
««) +
where
…… 
d
…… 
.
…… 
DoctorId
……  
==
……! #
doctorId
……$ ,
select
ÀÀ 
new
ÀÀ 
DoctorListDto
ÀÀ (
{
ÃÃ 
DoctorId
ÕÕ 
=
ÕÕ 
d
ÕÕ  
.
ÕÕ  !
DoctorId
ÕÕ! )
,
ÕÕ) *
FullName
ŒŒ 
=
ŒŒ 
d
ŒŒ  
.
ŒŒ  !
FullName
ŒŒ! )
,
ŒŒ) *
Specialisation
œœ "
=
œœ# $
d
œœ% &
.
œœ& '
Specialisation
œœ' 5
,
œœ5 6
YearsOfExperience
–– %
=
––& '
d
––( )
.
––) *
YearsOfExperience
––* ;
,
––; <
ConsultationFee
—— #
=
——$ %
d
——& '
.
——' (
ConsultationFee
——( 7
,
——7 8
IsActive
““ 
=
““ 
d
““  
.
““  !
IsActive
““! )
,
““) *
Email
”” 
=
”” 
u
”” 
.
”” 
Email
”” #
}
‘‘ 
)
÷÷ 
.
÷÷ !
FirstOrDefaultAsync
÷÷ !
(
÷÷! "
)
÷÷" #
;
÷÷# $
if
ÿÿ 
(
ÿÿ 
doctor
ÿÿ 
==
ÿÿ 
null
ÿÿ 
)
ÿÿ 
throw
ŸŸ 
new
ŸŸ %
DoctorNotFoundException
ŸŸ 1
(
ŸŸ1 2
$str
ŸŸ2 D
)
ŸŸD E
;
ŸŸE F
return
€€ 
doctor
€€ 
;
€€ 
}
‹‹ 	
public
ﬁﬁ 
async
ﬁﬁ 
Task
ﬁﬁ 
<
ﬁﬁ 
List
ﬁﬁ 
<
ﬁﬁ 
DoctorListDto
ﬁﬁ ,
>
ﬁﬁ, -
>
ﬁﬁ- .
AvailableDoctors
ﬁﬁ/ ?
(
ﬁﬁ? @
string
ﬁﬁ@ F
specialisation
ﬁﬁG U
,
ﬁﬁU V
DateOnly
ﬁﬁW _
date
ﬁﬁ` d
)
ﬁﬁd e
=>
ﬁﬁf h
await
ﬂﬂ 
_repository
ﬂﬂ 
.
ﬂﬂ 
AvailableDoctors
ﬂﬂ .
(
ﬂﬂ. /
specialisation
ﬂﬂ/ =
,
ﬂﬂ= >
date
ﬂﬂ? C
)
ﬂﬂC D
;
ﬂﬂD E
}
‚‚ 
}„„ Œß
jC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Services\Implementations\AuthService.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
readonly 
UserManager $
<$ %
IdentityUser% 1
>1 2
_userManager3 ?
;? @
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 
IPatientRepository +
_patientRepo, 8
;8 9
private 
readonly 
IDoctorRepository *
_doctorRepo+ 6
;6 7
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
private 
readonly 
HealthCareDbContext ,
_context- 5
;5 6
public 
AuthService 
( 
UserManager 
< 
IdentityUser $
>$ %
userManager& 1
,1 2
IMapper 
mapper 
, 
IPatientRepository 
patientRepo *
,* +
IDoctorRepository 

doctorRepo (
,( )
IConfiguration   
configuration   (
,  ( )
HealthCareDbContext!! 
context!!  '
)!!' (
{"" 	
_userManager## 
=## 
userManager## &
;##& '
_mapper$$ 
=$$ 
mapper$$ 
;$$ 
_patientRepo%% 
=%% 
patientRepo%% &
;%%& '
_doctorRepo&& 
=&& 

doctorRepo&& $
;&&$ %
_configuration'' 
='' 
configuration'' *
;''* +
_context(( 
=(( 
context(( 
;(( 
})) 	
private,, 
async,, 
Task,, 
<,, 
IdentityUser,, '
>,,' (
CreateUserAsync,,) 8
(,,8 9
string,,9 ?
email,,@ E
,,,E F
string,,G M
password,,N V
,,,V W
string,,X ^
role,,_ c
),,c d
{-- 	
var.. 
existingUser.. 
=.. 
await.. $
_userManager..% 1
...1 2
FindByEmailAsync..2 B
(..B C
email..C H
)..H I
;..I J
if00 
(00 
existingUser00 
!=00 
null00  $
)00$ %
throw11 
new11 
	Exception11 #
(11# $
$str11$ :
)11: ;
;11; <
var33 
user33 
=33 
new33 
IdentityUser33 '
{44 
UserName55 
=55 
email55  
,55  !
Email66 
=66 
email66 
}77 
;77 
var99 
result99 
=99 
await99 
_userManager99 +
.99+ ,
CreateAsync99, 7
(997 8
user998 <
,99< =
password99> F
)99F G
;99G H
if;; 
(;; 
!;; 
result;; 
.;; 
	Succeeded;; !
);;! "
throw<< 
new<< 
	Exception<< #
(<<# $
string<<$ *
.<<* +
Join<<+ /
(<</ 0
$str<<0 4
,<<4 5
result<<6 <
.<<< =
Errors<<= C
.<<C D
Select<<D J
(<<J K
e<<K L
=><<M O
e<<P Q
.<<Q R
Description<<R ]
)<<] ^
)<<^ _
)<<_ `
;<<` a
await>> 
_userManager>> 
.>> 
AddToRoleAsync>> -
(>>- .
user>>. 2
,>>2 3
role>>4 8
)>>8 9
;>>9 :
return@@ 
user@@ 
;@@ 
}AA 	
publicDD 
asyncDD 
TaskDD  
RegisterPatientAsyncDD .
(DD. /
CreatePatientDtoDD/ ?
dtoDD@ C
)DDC D
{EE 	
varGG 
existsGG 
=GG 
awaitGG 
_contextGG '
.GG' (
UsersGG( -
.HH 
AnyAsyncHH 
(HH 
uHH 
=>HH  "
uHH# $
.HH$ %
EmailHH% *
==HH+ -
dtoHH. 1
.HH1 2
EmailHH2 7
)HH7 8
;HH8 9
ifJJ 
(JJ 
existsJJ 
)JJ 
throwKK 
newKK 
	ExceptionKK #
(KK# $
$strKK$ :
)KK: ;
;KK; <
ifNN 
(NN 
stringNN 
.NN 
IsNullOrEmptyNN $
(NN$ %
dtoNN% (
.NN( )
EmailNN) .
)NN. /
)NN/ 0
{OO 
throwPP 
newPP 
ArgumentExceptionPP +
(PP+ ,
$strPP, ?
)PP? @
;PP@ A
}QQ 
ifTT 
(TT 
dtoTT 
.TT 
PasswordTT 
!=TT 
dtoTT  #
.TT# $
ConfirmPasswordTT$ 3
)TT3 4
throwUU 
newUU 
	ExceptionUU #
(UU# $
$strUU$ <
)UU< =
;UU= >
varXX 
userXX 
=XX 
awaitXX 
CreateUserAsyncXX ,
(XX, -
dtoXX- 0
.XX0 1
EmailXX1 6
,XX6 7
dtoXX8 ;
.XX; <
PasswordXX< D
,XXD E
$strXXF O
)XXO P
;XXP Q
varZZ 
patientZZ 
=ZZ 
_mapperZZ !
.ZZ! "
MapZZ" %
<ZZ% &
PatientZZ& -
>ZZ- .
(ZZ. /
dtoZZ/ 2
)ZZ2 3
;ZZ3 4
patient]] 
.]] 
UserId]] 
=]] 
user]] !
.]]! "
Id]]" $
;]]$ %
await__ 
_patientRepo__ 
.__ 
AddAsync__ '
(__' (
patient__( /
)__/ 0
;__0 1
await`` 
_context`` 
.`` 
SaveChangesAsync`` +
(``+ ,
)``, -
;``- .
}aa 	
publicdd 
asyncdd 
Taskdd 
RegisterDoctorAsyncdd -
(dd- .
DoctorRegisterDtodd. ?
dtodd@ C
)ddC D
{ee 	
ifgg 
(gg 
dtogg 
.gg 
Passwordgg 
!=gg 
dtogg  #
.gg# $
ConfirmPasswordgg$ 3
)gg3 4
throwhh 
newhh 
ArgumentExceptionhh +
(hh+ ,
$strhh, E
)hhE F
;hhF G
varkk 
userkk 
=kk 
awaitkk 
CreateUserAsynckk ,
(kk, -
dtokk- 0
.kk0 1
Emailkk1 6
,kk6 7
dtokk8 ;
.kk; <
Passwordkk< D
,kkD E
$strkkF N
)kkN O
;kkO P
varmm 
doctormm 
=mm 
_mappermm  
.mm  !
Mapmm! $
<mm$ %
Doctormm% +
>mm+ ,
(mm, -
dtomm- 0
)mm0 1
;mm1 2
doctoroo 
.oo 
UserIdoo 
=oo 
useroo  
.oo  !
Idoo! #
;oo# $
awaitpp 
_doctorRepopp 
.pp 
AddAsyncpp &
(pp& '
doctorpp' -
)pp- .
;pp. /
awaitqq 
_contextqq 
.qq 
SaveChangesAsyncqq +
(qq+ ,
)qq, -
;qq- .
awaitss 
_doctorReposs 
.ss 
CreateSlotsss )
(ss) *
doctorss+ 1
.ss1 2
DoctorIdss2 :
,ss: ;
dtoss< ?
.ss? @
	TimeSlotsss@ I
??ssJ L
newssM P
ListssQ U
<ssU V
stringssV \
>ss\ ]
(ss] ^
)ss^ _
)ss_ `
;ss` a
awaittt 
_contexttt 
.tt 
SaveChangesAsynctt +
(tt+ ,
)tt, -
;tt- .
}uu 	
publicxx 
asyncxx 
Taskxx 
<xx 
AuthorResponseDtoxx +
>xx+ ,

LoginAsyncxx- 7
(xx7 8
LoginDtoxx8 @
dtoxxA D
)xxD E
{yy 	
varzz 
userzz 
=zz 
awaitzz 
_userManagerzz )
.zz) *
FindByEmailAsynczz* :
(zz: ;
dtozz; >
.zz> ?
Emailzz? D
)zzD E
;zzE F
if|| 
(|| 
user|| 
==|| 
null|| 
)|| 
throw}} 
new}} '
InvalidCredentialsException}} 5
(}}5 6
)}}6 7
;}}7 8
var 
validPassword 
= 
await  %
_userManager& 2
.2 3
CheckPasswordAsync3 E
(E F
userF J
,J K
dtoL O
.O P
PasswordP X
)X Y
;Y Z
if
ÅÅ 
(
ÅÅ 
!
ÅÅ 
validPassword
ÅÅ 
)
ÅÅ 
throw
ÇÇ 
new
ÇÇ )
InvalidCredentialsException
ÇÇ 5
(
ÇÇ5 6
)
ÇÇ6 7
;
ÇÇ7 8
var
ÑÑ 
roles
ÑÑ 
=
ÑÑ 
await
ÑÑ 
_userManager
ÑÑ *
.
ÑÑ* +
GetRolesAsync
ÑÑ+ 8
(
ÑÑ8 9
user
ÑÑ9 =
)
ÑÑ= >
;
ÑÑ> ?
if
ÜÜ 
(
ÜÜ 
!
ÜÜ 
roles
ÜÜ 
.
ÜÜ 
Any
ÜÜ 
(
ÜÜ 
)
ÜÜ 
)
ÜÜ 
throw
áá 
new
áá '
InvalidOperationException
áá 3
(
áá3 4
$str
áá4 O
)
ááO P
;
ááP Q
var
ää 
role
ää 
=
ää 
roles
ää 
[
ää 
$num
ää 
]
ää 
;
ää  
string
åå 
token
åå 
;
åå 
if
éé 
(
éé 
role
éé 
==
éé 
$str
éé !
)
éé! "
{
èè 
var
êê 
patient
êê 
=
êê 
await
êê #
_patientRepo
êê$ 0
.
êê0 1
GetByUserIdAsync
êê1 A
(
êêA B
user
êêB F
.
êêF G
Id
êêG I
)
êêI J
;
êêJ K
if
íí 
(
íí 
patient
íí 
==
íí 
null
íí #
)
íí# $
throw
ìì 
new
ìì &
PatientNotFoundException
ìì 6
(
ìì6 7
$str
ìì7 Q
)
ììQ R
;
ììR S
token
ïï 
=
ïï 
GenerateJwtToken
ïï (
(
ïï( )
user
ïï) -
,
ïï- .
role
ïï/ 3
,
ïï3 4
	patientId
ïï5 >
:
ïï> ?
patient
ïï@ G
.
ïïG H
	PatientId
ïïH Q
)
ïïQ R
;
ïïR S
}
ññ 
else
óó 
if
óó 
(
óó 
role
óó 
==
óó 
$str
óó %
)
óó% &
{
òò 
var
ôô 
doctor
ôô 
=
ôô 
await
ôô "
_doctorRepo
ôô# .
.
ôô. /
GetByUserIdAsync
ôô/ ?
(
ôô? @
user
ôô@ D
.
ôôD E
Id
ôôE G
)
ôôG H
;
ôôH I
await
öö 
_context
öö 
.
öö 
SaveChangesAsync
öö /
(
öö/ 0
)
öö0 1
;
öö1 2
await
úú 
_context
úú 
.
úú 
SaveChangesAsync
úú /
(
úú/ 0
)
úú0 1
;
úú1 2
if
ûû 
(
ûû 
doctor
ûû 
==
ûû 
null
ûû "
)
ûû" #
throw
üü 
new
üü '
InvalidOperationException
üü 7
(
üü7 8
$str
üü8 Q
)
üüQ R
;
üüR S
token
°° 
=
°° 
GenerateJwtToken
°° (
(
°°( )
user
°°) -
,
°°- .
role
°°/ 3
,
°°3 4
doctorId
°°5 =
:
°°= >
doctor
°°? E
.
°°E F
DoctorId
°°F N
)
°°N O
;
°°O P
}
¢¢ 
else
££ 
if
££ 
(
££ 
role
££ 
==
££ 
$str
££ $
)
££$ %
{
§§ 
token
¶¶ 
=
¶¶ 
GenerateJwtToken
¶¶ (
(
¶¶( )
user
¶¶) -
,
¶¶- .
role
¶¶/ 3
)
¶¶3 4
;
¶¶4 5
}
ßß 
else
®® 
{
©© 
throw
™™ 
new
™™ '
InvalidOperationException
™™ 3
(
™™3 4
$str
™™4 B
)
™™B C
;
™™C D
}
´´ 
return
≠≠ 
new
≠≠ 
AuthorResponseDto
≠≠ (
{
ÆÆ 
AccessToken
ØØ 
=
ØØ 
token
ØØ #
,
ØØ# $
Role
∞∞ 
=
∞∞ 
role
∞∞ 
}
±± 
;
±± 
}
≤≤ 	
public
∂∂ 
async
∂∂ 
Task
∂∂ 
<
∂∂ 
bool
∂∂ 
>
∂∂ 
EmailExistsAsync
∂∂  0
(
∂∂0 1
string
∂∂1 7
email
∂∂8 =
)
∂∂= >
{
∑∑ 	
return
∏∏ 
await
∏∏ 
_context
∏∏ !
.
∏∏! "
Users
∏∏" '
.
ππ 
AnyAsync
ππ 
(
ππ 
u
ππ 
=>
ππ 
u
ππ  
.
ππ  !
Email
ππ! &
==
ππ' )
email
ππ* /
)
ππ/ 0
;
ππ0 1
}
∫∫ 	
public
ææ 
async
ææ 
Task
ææ !
ChangePasswordAsync
ææ -
(
ææ- .
string
ææ. 4
userId
ææ5 ;
,
ææ; <
ChangePasswordDto
ææ= N
dto
ææO R
)
ææR S
{
øø 	
var
¿¿ 
user
¿¿ 
=
¿¿ 
await
¿¿ 
_userManager
¿¿ )
.
¿¿) *
FindByIdAsync
¿¿* 7
(
¿¿7 8
userId
¿¿8 >
)
¿¿> ?
;
¿¿? @
if
¬¬ 
(
¬¬ 
user
¬¬ 
==
¬¬ 
null
¬¬ 
)
¬¬ 
throw
√√ 
new
√√ #
UserNotFoundException
√√ /
(
√√/ 0
)
√√0 1
;
√√1 2
if
≈≈ 
(
≈≈ 
string
≈≈ 
.
≈≈  
IsNullOrWhiteSpace
≈≈ )
(
≈≈) *
dto
≈≈* -
.
≈≈- .
CurrentPassword
≈≈. =
)
≈≈= >
)
≈≈> ?
throw
∆∆ 
new
∆∆ 
ArgumentException
∆∆ +
(
∆∆+ ,
$str
∆∆, J
)
∆∆J K
;
∆∆K L
if
»» 
(
»» 
string
»» 
.
»»  
IsNullOrWhiteSpace
»» )
(
»») *
dto
»»* -
.
»»- .
NewPassword
»». 9
)
»»9 :
)
»»: ;
throw
…… 
new
…… 
ArgumentException
…… +
(
……+ ,
$str
……, F
)
……F G
;
……G H
var
ÀÀ 
result
ÀÀ 
=
ÀÀ 
await
ÀÀ 
_userManager
ÀÀ +
.
ÀÀ+ ,!
ChangePasswordAsync
ÀÀ, ?
(
ÀÀ? @
user
ÃÃ 
,
ÃÃ 
dto
ÕÕ 
.
ÕÕ 
CurrentPassword
ÕÕ #
,
ÕÕ# $
dto
ŒŒ 
.
ŒŒ 
NewPassword
ŒŒ 
)
œœ 
;
œœ 
if
““ 
(
““ 
!
““ 
result
““ 
.
““ 
	Succeeded
““ !
)
““! "
throw
”” 
new
”” '
InvalidOperationException
”” 3
(
””3 4
string
‘‘ 
.
‘‘ 
Join
‘‘ 
(
‘‘  
$str
‘‘  $
,
‘‘$ %
result
‘‘& ,
.
‘‘, -
Errors
‘‘- 3
.
‘‘3 4
Select
‘‘4 :
(
‘‘: ;
e
‘‘; <
=>
‘‘= ?
e
‘‘@ A
.
‘‘A B
Description
‘‘B M
)
‘‘M N
)
‘‘N O
)
’’ 
;
’’ 
}
÷÷ 	
private
ŸŸ 
string
ŸŸ 
GenerateJwtToken
ŸŸ '
(
ŸŸ' (
IdentityUser
⁄⁄ 
user
⁄⁄ 
,
⁄⁄ 
string
€€ 
role
€€ 
,
€€ 
int
‹‹ 
?
‹‹ 
	patientId
‹‹ 
=
‹‹ 
null
‹‹ !
,
‹‹! "
int
›› 
?
›› 
doctorId
›› 
=
›› 
null
››  
)
››  !
{
ﬁﬁ 	
var
ﬂﬂ 
jwtSettings
ﬂﬂ 
=
ﬂﬂ 
_configuration
ﬂﬂ ,
.
ﬂﬂ, -

GetSection
ﬂﬂ- 7
(
ﬂﬂ7 8
$str
ﬂﬂ8 =
)
ﬂﬂ= >
;
ﬂﬂ> ?
var
·· 
key
·· 
=
·· 
new
·· "
SymmetricSecurityKey
·· .
(
··. /
Encoding
‚‚ 
.
‚‚ 
UTF8
‚‚ 
.
‚‚ 
GetBytes
‚‚ &
(
‚‚& '
jwtSettings
‚‚' 2
[
‚‚2 3
$str
‚‚3 8
]
‚‚8 9
!
‚‚9 :
)
‚‚: ;
)
„„ 
;
„„ 
var
ÂÂ 
credentials
ÂÂ 
=
ÂÂ 
new
ÂÂ ! 
SigningCredentials
ÂÂ" 4
(
ÂÂ4 5
key
ÂÂ5 8
,
ÂÂ8 9 
SecurityAlgorithms
ÂÂ: L
.
ÂÂL M

HmacSha256
ÂÂM W
)
ÂÂW X
;
ÂÂX Y
var
ÁÁ 
claims
ÁÁ 
=
ÁÁ 
new
ÁÁ 
List
ÁÁ !
<
ÁÁ! "
Claim
ÁÁ" '
>
ÁÁ' (
{
ËË 
new
ÈÈ 
Claim
ÈÈ 
(
ÈÈ 

ClaimTypes
ÈÈ $
.
ÈÈ$ %
NameIdentifier
ÈÈ% 3
,
ÈÈ3 4
user
ÈÈ5 9
.
ÈÈ9 :
Id
ÈÈ: <
)
ÈÈ< =
,
ÈÈ= >
new
ÍÍ 
Claim
ÍÍ 
(
ÍÍ 

ClaimTypes
ÍÍ $
.
ÍÍ$ %
Email
ÍÍ% *
,
ÍÍ* +
user
ÍÍ, 0
.
ÍÍ0 1
Email
ÍÍ1 6
!
ÍÍ6 7
)
ÍÍ7 8
,
ÍÍ8 9
new
ÎÎ 
Claim
ÎÎ 
(
ÎÎ 

ClaimTypes
ÎÎ $
.
ÎÎ$ %
Role
ÎÎ% )
,
ÎÎ) *
role
ÎÎ+ /
)
ÎÎ/ 0
,
ÎÎ0 1
new
ÏÏ 
Claim
ÏÏ 
(
ÏÏ %
JwtRegisteredClaimNames
ÏÏ 1
.
ÏÏ1 2
Jti
ÏÏ2 5
,
ÏÏ5 6
Guid
ÏÏ7 ;
.
ÏÏ; <
NewGuid
ÏÏ< C
(
ÏÏC D
)
ÏÏD E
.
ÏÏE F
ToString
ÏÏF N
(
ÏÏN O
)
ÏÏO P
)
ÏÏP Q
}
ÌÌ 
;
ÌÌ 
if
 
(
 
	patientId
 
.
 
HasValue
 "
)
" #
claims
ÒÒ 
.
ÒÒ 
Add
ÒÒ 
(
ÒÒ 
new
ÒÒ 
Claim
ÒÒ $
(
ÒÒ$ %
$str
ÒÒ% 0
,
ÒÒ0 1
	patientId
ÒÒ2 ;
.
ÒÒ; <
Value
ÒÒ< A
.
ÒÒA B
ToString
ÒÒB J
(
ÒÒJ K
)
ÒÒK L
)
ÒÒL M
)
ÒÒM N
;
ÒÒN O
if
ÛÛ 
(
ÛÛ 
doctorId
ÛÛ 
.
ÛÛ 
HasValue
ÛÛ !
)
ÛÛ! "
claims
ÙÙ 
.
ÙÙ 
Add
ÙÙ 
(
ÙÙ 
new
ÙÙ 
Claim
ÙÙ $
(
ÙÙ$ %
$str
ÙÙ% /
,
ÙÙ/ 0
doctorId
ÙÙ1 9
.
ÙÙ9 :
Value
ÙÙ: ?
.
ÙÙ? @
ToString
ÙÙ@ H
(
ÙÙH I
)
ÙÙI J
)
ÙÙJ K
)
ÙÙK L
;
ÙÙL M
var
ˆˆ 
expiryMinutes
ˆˆ 
=
ˆˆ 
int
ˆˆ  #
.
ˆˆ# $
Parse
ˆˆ$ )
(
ˆˆ) *
jwtSettings
ˆˆ* 5
[
ˆˆ5 6
$str
ˆˆ6 T
]
ˆˆT U
!
ˆˆU V
)
ˆˆV W
;
ˆˆW X
var
¯¯ 
token
¯¯ 
=
¯¯ 
new
¯¯ 
JwtSecurityToken
¯¯ ,
(
¯¯, -
issuer
˘˘ 
:
˘˘ 
jwtSettings
˘˘ #
[
˘˘# $
$str
˘˘$ ,
]
˘˘, -
,
˘˘- .
audience
˙˙ 
:
˙˙ 
jwtSettings
˙˙ %
[
˙˙% &
$str
˙˙& 0
]
˙˙0 1
,
˙˙1 2
claims
˚˚ 
:
˚˚ 
claims
˚˚ 
,
˚˚ 
expires
¸¸ 
:
¸¸ 
DateTime
¸¸ !
.
¸¸! "
UtcNow
¸¸" (
.
¸¸( )

AddMinutes
¸¸) 3
(
¸¸3 4
expiryMinutes
¸¸4 A
)
¸¸A B
,
¸¸B C 
signingCredentials
˝˝ "
:
˝˝" #
credentials
˝˝$ /
)
˛˛ 
;
˛˛ 
return
ÄÄ 
new
ÄÄ %
JwtSecurityTokenHandler
ÄÄ .
(
ÄÄ. /
)
ÄÄ/ 0
.
ÄÄ0 1

WriteToken
ÄÄ1 ;
(
ÄÄ; <
token
ÄÄ< A
)
ÄÄA B
;
ÄÄB C
}
ÅÅ 	
}
ÑÑ 
}ÖÖ âØ
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
int= @
	patientIdA J
)J K
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
;; <
appointment!! 
.!! 
	PatientId!! !
=!!" #
	patientId!!$ -
;!!- .
appointment## 
.## 
Status## 
=##  
$str##! *
;##* +
await%% 
_repository%% 
.%% 
AddAsync%% &
(%%& '
appointment%%' 2
)%%2 3
;%%3 4
await'' 
_context'' 
.'' 
SaveChangesAsync'' +
(''+ ,
)'', -
;''- .
}(( 	
public** 
async** 
Task** 
UpdateAsync** %
(**% &
int**& )
id*** ,
,**, - 
UpdateAppointmentDto**. B
dto**C F
)**F G
{++ 	
var,, 
appointment,, 
=,, 
await,, #
_repository,,$ /
.,,/ 0
GetProfileAsync,,0 ?
(,,? @
id,,@ B
),,B C
;,,C D
if-- 
(-- 
appointment-- 
==-- 
null-- #
)--# $
throw.. 
new.. (
AppointmentNotFoundException.. 6
(..6 7
id..7 9
)..9 :
;..: ;
_mapper// 
.// 
Map// 
(// 
dto// 
,// 
appointment// (
)//( )
;//) *
await11 
_repository11 
.11 
UpdateAsync11 )
(11) *
appointment11* 5
)115 6
;116 7
await22 
_context22 
.22 
SaveChangesAsync22 +
(22+ ,
)22, -
;22- .
}33 	
public55 
async55 
Task55 
DeleteAsync55 %
(55% &
int55& )
id55* ,
)55, -
{66 	
var77 
appointment77 
=77 
await77 #
_repository77$ /
.77/ 0
GetProfileAsync770 ?
(77? @
id77@ B
)77B C
;77C D
if88 
(88 
appointment88 
==88 
null88 #
)88# $
throw99 
new99 (
AppointmentNotFoundException99 6
(996 7
id997 9
)999 :
;99: ;
await:: 
_repository:: 
.:: 
DeleteAsync:: )
(::) *
id::* ,
)::, -
;::- .
await;; 
_context;; 
.;; 
SaveChangesAsync;; +
(;;+ ,
);;, -
;;;- .
}<< 	
public>> 
async>> 
Task>> 
<>> 
AppointmentListDto>> ,
?>>, -
>>>- .
GetByIdAsync>>/ ;
(>>; <
int>>< ?
id>>@ B
)>>B C
{?? 	
var@@ 
appointment@@ 
=@@ 
await@@ #
_repository@@$ /
.@@/ 0
GetProfileAsync@@0 ?
(@@? @
id@@@ B
)@@B C
;@@C D
returnAA 
appointmentAA 
==AA !
nullAA" &
?AA' (
nullAA) -
:AA. /
_mapperAA0 7
.AA7 8
MapAA8 ;
<AA; <
AppointmentListDtoAA< N
?AAN O
>AAO P
(AAP Q
appointmentAAQ \
)AA\ ]
;AA] ^
}BB 	
publicDD 
asyncDD 
TaskDD 
<DD 
PagedResultDD %
<DD% &
AppointmentListDtoDD& 8
>DD8 9
>DD9 :
GetAllAsyncDD; F
(DDF G
AppointmentFilterDDG X
filterDDY _
)DD_ `
{EE 	

ExpressionGG 
<GG 
FuncGG 
<GG 
AppointmentGG '
,GG' (
boolGG) -
>GG- .
>GG. /
?GG/ 0
	predicateGG1 :
=GG; <
nullGG= A
;GGA B
ifII 
(II 
!II 
stringII 
.II 
IsNullOrWhiteSpaceII *
(II* +
filterII+ 1
.II1 2
StatusII2 8
)II8 9
&&II: <
filterII= C
.IIC D
ScheduledDateIID Q
.IIQ R
HasValueIIR Z
)IIZ [
{JJ 
	predicateKK 
=KK 
aKK 
=>KK  
aLL 
.LL 
StatusLL 
==LL 
filterLL  &
.LL& '
StatusLL' -
&&LL. 0
aMM 
.MM 
ScheduledDateMM #
==MM$ &
filterMM' -
.MM- .
ScheduledDateMM. ;
.MM; <
ValueMM< A
;MMA B
}NN 
elseOO 
ifOO 
(OO 
!OO 
stringOO 
.OO 
IsNullOrWhiteSpaceOO /
(OO/ 0
filterOO0 6
.OO6 7
StatusOO7 =
)OO= >
)OO> ?
{PP 
	predicateQQ 
=QQ 
aQQ 
=>QQ  
aQQ! "
.QQ" #
StatusQQ# )
==QQ* ,
filterQQ- 3
.QQ3 4
StatusQQ4 :
;QQ: ;
}RR 
elseSS 
ifSS 
(SS 
filterSS 
.SS 
ScheduledDateSS )
.SS) *
HasValueSS* 2
)SS2 3
{TT 
	predicateUU 
=UU 
aUU 
=>UU  
aUU! "
.UU" #
ScheduledDateUU# 0
==UU1 3
filterUU4 :
.UU: ;
ScheduledDateUU; H
.UUH I
ValueUUI N
;UUN O
}VV 
FuncYY 
<YY 

IQueryableYY 
<YY 
AppointmentYY '
>YY' (
,YY( )
IOrderedQueryableYY* ;
<YY; <
AppointmentYY< G
>YYG H
>YYH I
orderByYYJ Q
=YYR S
qZZ 
=>ZZ 
qZZ 
.ZZ 
OrderByZZ 
(ZZ 
aZZ  
=>ZZ! #
aZZ$ %
.ZZ% &
ScheduledDateZZ& 3
)ZZ3 4
;ZZ4 5
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
AppointmentListDtoee# 5
>ee5 6
{ff 
Itemsgg 
=gg 
_mappergg 
.gg  
Mapgg  #
<gg# $
IEnumerablegg$ /
<gg/ 0
AppointmentListDtogg0 B
>ggB C
>ggC D
(ggD E
pagedResultggE P
.ggP Q
ItemsggQ V
)ggV W
,ggW X

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
}ll 	
publicnn 
asyncnn 
Tasknn 
UpdateStatusAsyncnn +
(nn+ ,
intnn, /
idnn0 2
,nn2 3 
UpdateAppointmentDtonn4 H
dtonnI L
)nnL M
{oo 	
varpp 
appointmentpp 
=pp 
awaitpp #
_repositorypp$ /
.pp/ 0
GetProfileAsyncpp0 ?
(pp? @
idpp@ B
)ppB C
;ppC D
ifrr 
(rr 
appointmentrr 
isrr 
nullrr #
)rr# $
throwss 
newss %
InvalidOperationExceptionss 3
(ss3 4
$strss4 H
)ssH I
;ssI J
appointmentuu 
.uu 
Statusuu 
=uu  
dtouu! $
.uu$ %
Statusuu% +
;uu+ ,
appointmentvv 
.vv 
CancellationReasonvv *
=vv+ ,
dtovv- 0
.vv0 1
CancellationReasonvv1 C
;vvC D
awaitxx 
_repositoryxx 
.xx 
UpdateAsyncxx )
(xx) *
appointmentxx* 5
)xx5 6
;xx6 7
awaityy 
_contextyy 
.yy 
SaveChangesAsyncyy +
(yy+ ,
)yy, -
;yy- .
}zz 	
public{{ 
async{{ 
Task{{ 
<{{ 
List{{ 
<{{ 
string{{ %
>{{% &
>{{& '
AvailableTimeSlots{{( :
({{: ;
DateOnly{{; C
date{{D H
,{{H I
int{{J M
doctorId{{N V
){{V W
{|| 	
if}} 
(}} 
date}} 
<}} 
DateOnly}} 
.}}  
FromDateTime}}  ,
(}}, -
DateTime}}- 5
.}}5 6
Today}}6 ;
)}}; <
)}}< =
throw~~ 
new~~ %
InvalidOperationException~~ 3
(~~3 4
$str~~4 `
)~~` a
;~~a b
var
ÄÄ 
allSlots
ÄÄ 
=
ÄÄ 
await
ÄÄ  
_doctorService
ÄÄ! /
.
ÄÄ/ 0
GetSlots
ÄÄ0 8
(
ÄÄ8 9
doctorId
ÄÄ9 A
)
ÄÄA B
;
ÄÄB C
var
ÅÅ 
bookedSlots
ÅÅ 
=
ÅÅ 
await
ÅÅ #
_repository
ÅÅ$ /
.
ÅÅ/ 0 
AvailableTimeSlots
ÅÅ0 B
(
ÅÅB C
date
ÅÅC G
,
ÅÅG H
doctorId
ÅÅI Q
)
ÅÅQ R
;
ÅÅR S
var
ÉÉ 
	freeSlots
ÉÉ 
=
ÉÉ 
allSlots
ÉÉ $
.
ÉÉ$ %
Except
ÉÉ% +
(
ÉÉ+ ,
bookedSlots
ÉÉ, 7
)
ÉÉ7 8
.
ÉÉ8 9
ToList
ÉÉ9 ?
(
ÉÉ? @
)
ÉÉ@ A
;
ÉÉA B
return
ÖÖ 
	freeSlots
ÖÖ 
;
ÖÖ 
}
ÜÜ 	
public
àà 
async
àà 
Task
àà 
<
àà 
bool
àà 
>
àà 
IsAvailable
àà  +
(
àà+ ,
DateOnly
àà, 4
date
àà5 9
,
àà9 :
int
àà; >
doctorId
àà? G
,
ààG H
string
ààI O
timeSlot
ààP X
)
ààX Y
{
ââ 	
var
ää 
	available
ää 
=
ää 
await
ää !
_repository
ää" -
.
ää- .
IsAvailable
ää. 9
(
ää9 :
date
ää: >
,
ää> ?
doctorId
ää@ H
,
ääH I
timeSlot
ääJ R
)
ääR S
;
ääS T
if
åå 
(
åå 
!
åå 
	available
åå 
)
åå 
throw
çç 
new
çç '
InvalidOperationException
çç 3
(
çç3 4
$str
çç4 W
)
ççW X
;
ççX Y
return
èè 
true
èè 
;
èè 
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
íí "
AppointmentReportDto
íí 3
>
íí3 4
>
íí4 5
GetDailyReport
íí6 D
(
ííD E
DateOnly
ííF N
	startDate
ííO X
,
ííX Y
DateOnly
ííZ b
endDate
ííc j
)
ííj k
{
ìì 	
var
îî 
report
îî 
=
îî 
await
îî 
_repository
îî *
.
îî* +
GetDailyReport
îî+ 9
(
îî9 :
	startDate
îî: C
,
îîC D
endDate
îîE L
)
îîL M
;
îîM N
return
ññ 
report
ññ 
.
ññ 
Count
ññ 
==
ññ  "
$num
ññ# $
?
óó 
new
óó 
List
óó 
<
óó "
AppointmentReportDto
óó /
>
óó/ 0
(
óó0 1
)
óó1 2
:
òò 
report
òò 
;
òò 
}
ôô 	
public
õõ 
async
õõ 
Task
õõ 
<
õõ 
List
õõ 
<
õõ  
AppointmentListDto
õõ 1
>
õõ1 2
>
õõ2 3
GetDoctorSchedule
õõ4 E
(
õõE F
DateOnly
õõF N
date
õõO S
,
õõS T
int
õõU X
id
õõY [
)
õõ[ \
{
úú 	
var
ùù 
schedule
ùù 
=
ùù 
await
ùù  
_repository
ùù! ,
.
ùù, -
GetDoctorSchedule
ùù- >
(
ùù> ?
date
ùù? C
,
ùùC D
id
ùùE G
)
ùùG H
;
ùùH I
return
ûû 
schedule
ûû 
.
ûû 
Count
ûû !
==
ûû" $
$num
ûû% &
?
ûû' (
new
ûû) ,
List
ûû- 1
<
ûû1 2 
AppointmentListDto
ûû2 D
>
ûûD E
(
ûûE F
)
ûûF G
:
ûûH I
schedule
ûûJ R
;
ûûR S
}
üü 	
public
°° 
async
°° 
Task
°° 
<
°° 
List
°° 
<
°°  
AppointmentListDto
°° 1
>
°°1 2
>
°°2 3 
GetPatientSchedule
°°4 F
(
°°F G
DateOnly
°°G O
date
°°P T
,
°°T U
int
°°V Y
id
°°Z \
)
°°\ ]
{
¢¢ 	
var
££ 
schedule
££ 
=
££ 
await
££  
_repository
££! ,
.
££, - 
GetPatientSchedule
££- ?
(
££? @
date
££@ D
,
££D E
id
££F H
)
££H I
;
££I J
return
§§ 
schedule
§§ 
.
§§ 
Count
§§ !
==
§§" $
$num
§§% &
?
§§' (
new
§§) ,
List
§§- 1
<
§§1 2 
AppointmentListDto
§§2 D
>
§§D E
(
§§E F
)
§§F G
:
§§H I
schedule
§§J R
;
§§R S
}
•• 	
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
List
ßß 
<
ßß  
AppointmentListDto
ßß 1
>
ßß1 2
>
ßß2 3%
GetAppointmentByPatient
ßß4 K
(
ßßK L
int
ßßL O
id
ßßP R
)
ßßR S
{
®® 	
var
©© 
appointments
©© 
=
©© 
await
©© $
_repository
©©% 0
.
©©0 1%
GetAppointmentByPatient
©©1 H
(
©©H I
id
©©I K
)
©©K L
;
©©L M
return
™™ 
appointments
™™ 
.
™™  
Count
™™  %
==
™™& (
$num
™™) *
?
™™+ ,
new
™™- 0
List
™™1 5
<
™™5 6 
AppointmentListDto
™™6 H
>
™™H I
(
™™I J
)
™™J K
:
™™L M
appointments
™™N Z
;
™™Z [
}
´´ 	
public
≠≠ 
async
≠≠ 
Task
≠≠ 
<
≠≠ 
List
≠≠ 
<
≠≠  
AppointmentListDto
≠≠ 1
>
≠≠1 2
>
≠≠2 3$
GetAppointmentByDoctor
≠≠4 J
(
≠≠J K
int
≠≠K N
id
≠≠O Q
)
≠≠Q R
{
ÆÆ 	
var
ØØ 
appointments
ØØ 
=
ØØ 
await
ØØ $
_repository
ØØ% 0
.
ØØ0 1$
GetAppointmentByDoctor
ØØ1 G
(
ØØG H
id
ØØH J
)
ØØJ K
;
ØØK L
return
∞∞ 
appointments
∞∞ 
.
∞∞  
Count
∞∞  %
==
∞∞& (
$num
∞∞) *
?
∞∞+ ,
new
∞∞- 0
List
∞∞1 5
<
∞∞5 6 
AppointmentListDto
∞∞6 H
>
∞∞H I
(
∞∞I J
)
∞∞J K
:
∞∞L M
appointments
∞∞N Z
;
∞∞Z [
}
±± 	
public
≥≥ 
async
≥≥ 
Task
≥≥ ,
CancelAppointmentsByDoctorDate
≥≥ 8
(
≥≥8 9
int
≥≥9 <
doctorId
≥≥= E
,
≥≥E F
DateOnly
≥≥G O
date
≥≥P T
)
≥≥T U
{
¥¥ 	
await
µµ 
_repository
µµ 
.
µµ ,
CancelAppointmentsByDoctorDate
µµ <
(
µµ< =
doctorId
µµ= E
,
µµE F
date
µµG K
)
µµK L
;
µµL M
await
∂∂ 
_context
∂∂ 
.
∂∂ 
SaveChangesAsync
∂∂ +
(
∂∂+ ,
)
∂∂, -
;
∂∂- .
}
∑∑ 	
public
ππ 
async
ππ 
Task
ππ 
<
ππ #
AppointmentSummaryDto
ππ /
>
ππ/ 0
GetSummaryAsync
ππ1 @
(
ππ@ A
)
ππA B
{
∫∫ 	
var
ªª 
doctors
ªª 
=
ªª 
await
ªª 
_context
ªª  (
.
ªª( )
Doctors
ªª) 0
.
ªª0 1

CountAsync
ªª1 ;
(
ªª; <
)
ªª< =
;
ªª= >
var
ºº 
patients
ºº 
=
ºº 
await
ºº  
_context
ºº! )
.
ºº) *
Patients
ºº* 2
.
ºº2 3

CountAsync
ºº3 =
(
ºº= >
)
ºº> ?
;
ºº? @
var
ææ 
totalAppointments
ææ !
=
ææ" #
await
ææ$ )
_context
ææ* 2
.
ææ2 3
Appointments
ææ3 ?
.
ææ? @

CountAsync
ææ@ J
(
ææJ K
)
ææK L
;
ææL M
var
¿¿ 
pending
¿¿ 
=
¿¿ 
await
¿¿ 
_context
¿¿  (
.
¿¿( )
Appointments
¿¿) 5
.
¿¿5 6

CountAsync
¿¿6 @
(
¿¿@ A
a
¿¿A B
=>
¿¿C E
a
¿¿F G
.
¿¿G H
Status
¿¿H N
==
¿¿O Q
$str
¿¿R [
)
¿¿[ \
;
¿¿\ ]
var
¡¡ 
	confirmed
¡¡ 
=
¡¡ 
await
¡¡ !
_context
¡¡" *
.
¡¡* +
Appointments
¡¡+ 7
.
¡¡7 8

CountAsync
¡¡8 B
(
¡¡B C
a
¡¡C D
=>
¡¡E G
a
¡¡H I
.
¡¡I J
Status
¡¡J P
==
¡¡Q S
$str
¡¡T _
)
¡¡_ `
;
¡¡` a
var
¬¬ 
	completed
¬¬ 
=
¬¬ 
await
¬¬ !
_context
¬¬" *
.
¬¬* +
Appointments
¬¬+ 7
.
¬¬7 8

CountAsync
¬¬8 B
(
¬¬B C
a
¬¬C D
=>
¬¬E G
a
¬¬H I
.
¬¬I J
Status
¬¬J P
==
¬¬Q S
$str
¬¬T _
)
¬¬_ `
;
¬¬` a
var
√√ 
	cancelled
√√ 
=
√√ 
await
√√ !
_context
√√" *
.
√√* +
Appointments
√√+ 7
.
√√7 8

CountAsync
√√8 B
(
√√B C
a
√√C D
=>
√√E G
a
√√H I
.
√√I J
Status
√√J P
==
√√Q S
$str
√√T _
)
√√_ `
;
√√` a
var
≈≈ 
revenue
≈≈ 
=
≈≈ 
await
≈≈ 
_context
≈≈  (
.
≈≈( )
Appointments
≈≈) 5
.
∆∆ 
Where
∆∆ 
(
∆∆ 
a
∆∆ 
=>
∆∆ 
a
∆∆ 
.
∆∆ 
Status
∆∆ $
==
∆∆% '
$str
∆∆( 3
)
∆∆3 4
.
«« 
SumAsync
«« 
(
«« 
a
«« 
=>
«« 
a
««  
.
««  !
Doctor
««! '
.
««' (
ConsultationFee
««( 7
)
««7 8
;
««8 9
return
…… 
new
…… #
AppointmentSummaryDto
…… ,
{
   
TotalDoctors
ÀÀ 
=
ÀÀ 
doctors
ÀÀ &
,
ÀÀ& '
TotalPatients
ÃÃ 
=
ÃÃ 
patients
ÃÃ  (
,
ÃÃ( )
TotalAppointments
ÕÕ !
=
ÕÕ" #
totalAppointments
ÕÕ$ 5
,
ÕÕ5 6
PendingCount
œœ 
=
œœ 
pending
œœ &
,
œœ& '
ConfirmedCount
–– 
=
––  
	confirmed
––! *
,
––* +
CompletedCount
—— 
=
——  
	completed
——! *
,
——* +
CancelledCount
““ 
=
““  
	cancelled
““! *
,
““* +
TotalRevenue
‘‘ 
=
‘‘ 
revenue
‘‘ &
}
’’ 
;
’’ 
}
÷÷ 	
}
◊◊ 
}ÿÿ ∂
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Interfaces\IRepository.cs
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
	interface 
IRepository  
<  !
T! "
>" #
where$ )
T* +
:, -
class. 3
{ 
Task		 
<		 
T		 
>		 
AddAsync		 
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
UpdateAsync

 
(

 
T

 
entity

 !
,

! "
CancellationToken

# 4
ct

5 7
=

8 9
default

: A
)

A B
;

B C
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
Task 
< 
T 
? 
> 
GetProfileAsync  
(  !
int! $
id% '
)' (
;( )
Task 
< 
PagedResult 
< 
T 
> 
> 
GetAllAsync (
(( )
int* -

pageNumber. 8
,8 9
int9 <
pageSize= E
,E F

Expression 
< 
Func 
< 
T 
, 
bool #
># $
>$ %
?% &
	predicate' 0
=1 2
null3 7
,7 8
Func 
< 

IQueryable 
< 
T 
> 
, 
IOrderedQueryable  1
<1 2
T2 3
>3 4
>4 5
?5 6
orderBy7 >
=? @
nullA E
)E F
; 	
} 
} ã
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
? 
> 
GetByUserIdAsync %
(% &
string& ,
?, -
userId. 4
)4 5
;5 6
Task		 
<		 
PatientListDto		 
?		 
>		 
GetMyProfileAsync		 .
(		. /
int		/ 2
id		3 5
)		5 6
;		6 7
} 
} ‹
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
} ‡
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
(7 8
DateOnly8 @
	startDateA J
,J K
DateOnlyL T
endDateU \
)\ ]
;] ^
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
} í-
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
?(( 
>(( 
GetProfileAsync(( -
(((- .
int((. 1
id((2 4
)((4 5
=>((5 7
await** 
_dbSet** 
.** 
	FindAsync** #
(**# $
id**$ &
)**& '
;**' (
public,, 
async,, 
Task,, 
<,, 
PagedResult,, %
<,,% &
T,,& '
>,,' (
>,,( )
GetAllAsync,,* 5
(,,5 6
int-- 

pageNumber-- 
,-- 
int.. 
pageSize.. 
,.. 

Expression// 
<// 
Func// 
<// 
T// 
,// 
bool//  $
>//$ %
>//% &
?//& '
	predicate//( 1
=//2 3
null//4 8
,//8 9
Func00 
<00 

IQueryable00 
<00 
T00 
>00 
,00  
IOrderedQueryable00! 2
<002 3
T003 4
>004 5
>005 6
?006 7
orderBy008 ?
=00@ A
null00B F
)00F G
{11 	

IQueryable22 
<22 
T22 
>22 
query22 
=22  !
_dbSet22" (
;22( )
if44 
(44 
	predicate44 
!=44 
null44 !
)44! "
query55 
=55 
query55 
.55 
Where55 #
(55# $
	predicate55$ -
)55- .
;55. /
if77 
(77 
orderBy77 
!=77 
null77 
)77  
query88 
=88 
orderBy88 
(88  
query88  %
)88% &
;88& '
var:: 

totalCount:: 
=:: 
await:: "
query::# (
.::( )

CountAsync::) 3
(::3 4
)::4 5
;::5 6
var<< 
items<< 
=<< 
await<< 
query<< #
.== 
Skip== 
(== 
(== 

pageNumber== !
-==" #
$num==$ %
)==% &
*==' (
pageSize==) 1
)==1 2
.>> 
Take>> 
(>> 
pageSize>> 
)>> 
.?? 
ToListAsync?? 
(?? 
)?? 
;?? 
returnAA 
newAA 
PagedResultAA "
<AA" #
TAA# $
>AA$ %
{BB 
ItemsCC 
=CC 
itemsCC 
,CC 

PageNumberDD 
=DD 

pageNumberDD '
,DD' (
PageSizeEE 
=EE 
pageSizeEE #
,EE# $

TotalCountFF 
=FF 

totalCountFF '
}GG 
;GG 
}HH 	
}KK 
}LL Á
tC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Repositories\Implementations\PatientRepository.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Repositories %
.% &
Implementations& 5
{		 
public

 

class

 
PatientRepository

 "
:

# $

Repository

% /
<

/ 0
Patient

0 7
>

7 8
,

8 9
IPatientRepository

9 K
{ 
public 
PatientRepository  
(  !
HealthCareDbContext! 4
context5 <
)< =
:> ?
base@ D
(D E
contextE L
)L M
{N O
}P Q
public 
async 
Task 
< 
Patient !
?! "
>" #
GetByUserIdAsync$ 4
(4 5
string5 ;
?; <
userId= C
)C D
{ 	
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
UserId, 2
==3 5
userId6 <
)< =
;= >
} 	
public 
async 
Task 
< 
PatientListDto (
?( )
>) *
GetMyProfileAsync+ <
(< =
int= @
idA C
)C D
{ 	
var 
patient 
= 
await 
(  !
from 
p 
in 
_context "
." #
Patients# +
join 
u 
in 
_context "
." #
Users# (
on) +
p, -
.- .
UserId. 4
equals5 ;
u< =
.= >
Id> @
where 
p 
. 
	PatientId !
==" $
id% '
select 
new 
PatientListDto )
{ 
	PatientId 
= 
p  !
.! "
	PatientId" +
,+ ,
FullName 
= 
p  
.  !
FullName! )
,) *
PhoneNumber 
=  !
p" #
.# $
PhoneNumber$ /
,/ 0
Gender 
= 
p 
. 
Gender %
,% &
HasInsurance    
=  ! "
p  # $
.  $ %
InsuranceId  % 0
!=  1 3
null  4 8
,  8 9
Email!! 
=!! 
u!! 
.!! 
Email!! #
}"" 	
)## 
.## 
FirstOrDefaultAsync## !
(##! "
)##" #
;### $
if%% 
(%% 
patient%% 
==%% 
null%% 
)%%  
throw&& 
new&& $
PatientNotFoundException&& 2
(&&2 3
$str&&3 F
)&&F G
;&&G H
return(( 
patient(( 
;(( 
})) 	
}.. 
}// ﬁ
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
. 
Include 
( 
hr 
=> 
hr  
.  !
Doctor! '
)' (
. 
ToListAsync 
( 
) 
; 
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -(
GetHealthRecordByAppointment. J
(J K
intK N
idO Q
)Q R
=>S U
await 
_dbSet 
. 
Where 
( 
hr 
=> 
hr 
.  
AppointmentId  -
==. 0
id1 3
)3 4
. 
ToListAsync 
( 
) 
; 
} 
} Æ7
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
public55 
async55 
Task55 
<55 
List55 
<55 
DoctorListDto55 ,
>55, -
>55- .
AvailableDoctors55/ ?
(55? @
string55@ F
specialisation55G U
,55U V
DateOnly55W _
date55` d
)55d e
{66 	
return77 
await77 
_dbSet77 
.88 
Where88 
(88 
d88 
=>88 
d88 
.88 
Specialisation88 ,
==88- /
specialisation880 >
&&88? A
d99 
.99 
IsActive99 &
&&99' )
!:: 
d:: 
.:: 
DoctorLeaves:: +
.::+ ,
Any::, /
(::/ 0
l::0 1
=>::2 4
l::5 6
.::6 7
	LeaveDate::7 @
==::A C
date::D H
)::H I
)::I J
.<< 
Where<< 
(<< 
d<< 
=><< 
d== 
.== 
AvailableSlots== $
.==$ %
Any==% (
(==( )
slot==) -
=>==. 0
!>> 
d>> 
.>> 
Appointments>> '
.>>' (
Any>>( +
(>>+ ,
a>>, -
=>>>. 0
a?? 
.?? 
TimeSlot?? &
==??' )
slot??* .
.??. /
TimeSlot??/ 7
&&??8 :
a@@ 
.@@ 
ScheduledDate@@ +
==@@, .
date@@/ 3
&&@@4 6
aAA 
.AA 
StatusAA $
!=AA% '
$strAA( 3
)BB 
)CC 
)DD 
.FF 
SelectFF 
(FF 
dFF 
=>FF 
newFF  
DoctorListDtoFF! .
{GG 
DoctorIdHH 
=HH 
dHH  
.HH  !
DoctorIdHH! )
,HH) *
FullNameII 
=II 
dII  
.II  !
FullNameII! )
,II) *
SpecialisationJJ "
=JJ# $
dJJ% &
.JJ& '
SpecialisationJJ' 5
,JJ5 6
ConsultationFeeKK #
=KK$ %
dKK& '
.KK' (
ConsultationFeeKK( 7
,KK7 8
IsActiveLL 
=LL 
dLL  
.LL  !
IsActiveLL! )
}MM 
)MM 
.NN 
ToListAsyncNN 
(NN 
)NN 
;NN 
}OO 	
}QQ 
}TT …l
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
public 
async 
Task 
< 
List 
< 
string %
>% &
>& '
AvailableTimeSlots( :
(: ;
DateOnly; C
dateD H
,H I
intJ M
doctorIdN V
)V W
=>X Z
await 
_dbSet 
. 
Where 
( 
a 
=> 
a 
. 
ScheduledDate +
==, .
date/ 3
&& 
a 
. 
DoctorId &
==' )
doctorId* 2
&& 
a 
. 
Status $
!=% '
AppointmentStatus( 9
.9 :
	Cancelled: C
)C D
. 
Select 
( 
a 
=> 
a 
. 
TimeSlot '
)' (
. 
ToListAsync 
( 
) 
; 
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
(!!D E
DateOnly!!E M
	startDate!!N W
,!!W X
DateOnly!!Y a
endDate!!b i
)!!i j
{"" 	
return## 
await## 
_dbSet## 
.$$ 
Where$$ 
($$ 
a$$ 
=>$$ 
a%% 
.%% 
ScheduledDate%% #
>=%%$ &
	startDate%%' 0
&&%%1 3
a&& 
.&& 
ScheduledDate&& #
<=&&$ &
endDate&&' .
)&&. /
.(( 
GroupBy(( 
((( 
a(( 
=>(( 
a(( 
.((  
ScheduledDate((  -
)((- .
.** 
Select** 
(** 
g** 
=>** 
new**   
AppointmentReportDto**! 5
{++ 
Date,, 
=,, 
g,, 
.,, 
Key,,  
,,,  !
PendingCount--  
=--! "
g--# $
.--$ %
Count--% *
(--* +
a--+ ,
=>--- /
a--0 1
.--1 2
Status--2 8
==--9 ;
AppointmentStatus--< M
.--M N
Pending--N U
)--U V
,--V W
ConfirmedCount.. "
=..# $
g..% &
...& '
Count..' ,
(.., -
a..- .
=>../ 1
a..2 3
...3 4
Status..4 :
==..; =
AppointmentStatus..> O
...O P
	Confirmed..P Y
)..Y Z
,..Z [
CancelledCount// "
=//# $
g//% &
.//& '
Count//' ,
(//, -
a//- .
=>/// 1
a//2 3
.//3 4
Status//4 :
==//; =
AppointmentStatus//> O
.//O P
	Cancelled//P Y
)//Y Z
,//Z [
CompletedCount00 "
=00# $
g00% &
.00& '
Count00' ,
(00, -
a00- .
=>00/ 1
a002 3
.003 4
Status004 :
==00; =
AppointmentStatus00> O
.00O P
	Completed00P Y
)00Y Z
,00Z [
DailyRevenue11  
=11! "
g11# $
.11$ %
Where11% *
(11* +
a11+ ,
=>11- /
a110 1
.111 2
Status112 8
==119 ;
AppointmentStatus11< M
.11M N
	Completed11N W
)11W X
.22 
Sum22 
(22 
a22 
=>22 
a22 
.22 
Doctor22 "
.22" #
ConsultationFee22# 2
)222 3
}44 
)44 
.66 
OrderBy66 
(66 
r66 
=>66 
r66 
.66  
Date66  $
)66$ %
.77 
ToListAsync77 
(77 
)77 
;77 
}88 	
public:: 
async:: 
Task:: 
<:: 
List:: 
<:: 
AppointmentListDto:: 1
>::1 2
>::2 3
GetDoctorSchedule::4 E
(::E F
DateOnly::F N
date::O S
,::S T
int::U X
id::Y [
)::[ \
=>::] _
await;; 
_dbSet;; 
.<< 
Where<< 
(<< 
a<< 
=><< 
a<< 
.<< 
ScheduledDate<< +
==<<, .
date<</ 3
&&<<4 6
a<<7 8
.<<8 9
DoctorId<<9 A
==<<B D
id<<E G
)<<G H
.== 
Select== 
(== 
a== 
=>== 
new==  
AppointmentListDto==! 3
{>> 
AppointmentId?? !
=??" #
a??$ %
.??% &
AppointmentId??& 3
,??3 4
PatientName@@ 
=@@  !
a@@" #
.@@# $
Patient@@$ +
.@@+ ,
FullName@@, 4
,@@4 5

DoctorNameAA 
=AA  
aAA! "
.AA" #
DoctorAA# )
.AA) *
FullNameAA* 2
,AA2 3
ScheduledDateBB !
=BB" #
aBB$ %
.BB% &
ScheduledDateBB& 3
,BB3 4
TimeSlotCC 
=CC 
aCC  
.CC  !
TimeSlotCC! )
,CC) *
StatusDD 
=DD 
aDD 
.DD 
StatusDD %
}EE 
)EE 
.FF 
ToListAsyncFF 
(FF 
)FF 
;FF 
publicHH 
asyncHH 
TaskHH 
<HH 
ListHH 
<HH 
AppointmentListDtoHH 1
>HH1 2
>HH2 3
GetPatientScheduleHH4 F
(HHF G
DateOnlyHHG O
dateHHP T
,HHT U
intHHV Y
idHHZ \
)HH\ ]
=>HH^ `
awaitII 
_dbSetII 
.JJ 
WhereJJ 
(JJ 
aJJ 
=>JJ 
aJJ 
.JJ 
ScheduledDateJJ +
==JJ, .
dateJJ/ 3
&&JJ4 6
aJJ7 8
.JJ8 9
	PatientIdJJ9 B
==JJC E
idJJF H
)JJH I
.KK 
SelectKK 
(KK 
aKK 
=>KK 
newKK  
AppointmentListDtoKK! 3
{LL 
AppointmentIdMM !
=MM" #
aMM$ %
.MM% &
AppointmentIdMM& 3
,MM3 4
PatientNameNN 
=NN  !
aNN" #
.NN# $
PatientNN$ +
.NN+ ,
FullNameNN, 4
,NN4 5

DoctorNameOO 
=OO  
aOO! "
.OO" #
DoctorOO# )
.OO) *
FullNameOO* 2
,OO2 3
ScheduledDatePP !
=PP" #
aPP$ %
.PP% &
ScheduledDatePP& 3
,PP3 4
TimeSlotQQ 
=QQ 
aQQ  
.QQ  !
TimeSlotQQ! )
,QQ) *
StatusRR 
=RR 
aRR 
.RR 
StatusRR %
}SS 
)SS 
.TT 
ToListAsyncTT 
(TT 
)TT 
;TT 
publicVV 
asyncVV 
TaskVV 
<VV 
ListVV 
<VV 
AppointmentListDtoVV 1
>VV1 2
>VV2 3#
GetAppointmentByPatientVV4 K
(VVK L
intVVL O
idVVP R
)VVR S
=>VVT V
awaitWW 
_dbSetWW 
.XX 
WhereXX 
(XX 
aXX 
=>XX 
aXX 
.XX 
	PatientIdXX '
==XX( *
idXX+ -
&&XX. 0
aXX1 2
.XX2 3
ScheduledDateXX3 @
>=XXA C
DateOnlyXXD L
.XXL M
FromDateTimeXXM Y
(XXY Z
DateTimeXXZ b
.XXb c
TodayXXc h
)XXh i
)XXi j
.YY 
SelectYY 
(YY 
aYY 
=>YY 
newYY  
AppointmentListDtoYY! 3
{ZZ 
AppointmentId[[ !
=[[" #
a[[$ %
.[[% &
AppointmentId[[& 3
,[[3 4
PatientName\\ 
=\\  !
a\\" #
.\\# $
Patient\\$ +
.\\+ ,
FullName\\, 4
,\\4 5

DoctorName]] 
=]]  
a]]! "
.]]" #
Doctor]]# )
.]]) *
FullName]]* 2
,]]2 3
ScheduledDate^^ !
=^^" #
a^^$ %
.^^% &
ScheduledDate^^& 3
,^^3 4
TimeSlot__ 
=__ 
a__  
.__  !
TimeSlot__! )
,__) *
Status`` 
=`` 
a`` 
.`` 
Status`` %
}aa 
)aa 
.bb 
ToListAsyncbb 
(bb 
)bb 
;bb 
publicdd 
asyncdd 
Taskdd 
<dd 
Listdd 
<dd 
AppointmentListDtodd 1
>dd1 2
>dd2 3"
GetAppointmentByDoctordd4 J
(ddJ K
intddK N
idddO Q
)ddQ R
=>ddS U
awaitee 
_dbSetee 
.ff 
Whereff 
(ff 
aff 
=>ff 
aff 
.ff 
DoctorIdff &
==ff' )
idff* ,
&&ff- /
aff0 1
.ff1 2
ScheduledDateff2 ?
>=ff@ B
DateOnlyffC K
.ffK L
FromDateTimeffL X
(ffX Y
DateTimeffY a
.ffa b
Todayffb g
)ffg h
)ffh i
.gg 
Selectgg 
(gg 
agg 
=>gg 
newgg  
AppointmentListDtogg! 3
{hh 
AppointmentIdii !
=ii" #
aii$ %
.ii% &
AppointmentIdii& 3
,ii3 4
PatientNamejj 
=jj  !
ajj" #
.jj# $
Patientjj$ +
.jj+ ,
FullNamejj, 4
,jj4 5

DoctorNamekk 
=kk  
akk! "
.kk" #
Doctorkk# )
.kk) *
FullNamekk* 2
,kk2 3
ScheduledDatell !
=ll" #
all$ %
.ll% &
ScheduledDatell& 3
,ll3 4
TimeSlotmm 
=mm 
amm  
.mm  !
TimeSlotmm! )
,mm) *
Statusnn 
=nn 
ann 
.nn 
Statusnn %
}oo 
)oo 
.pp 
ToListAsyncpp 
(pp 
)pp 
;pp 
publicrr 
asyncrr 
Taskrr *
CancelAppointmentsByDoctorDaterr 8
(rr8 9
intrr9 <
doctorIdrr= E
,rrE F
DateOnlyrrG O
daterrP T
)rrT U
{ss 	
vartt 
appointmentstt 
=tt 
awaittt $
_dbSettt% +
.uu 
Whereuu 
(uu 
auu 
=>uu 
auu 
.uu 
DoctorIduu &
==uu' )
doctorIduu* 2
&&vv 
avv 
.vv 
ScheduledDatevv +
==vv, .
datevv/ 3
&&ww 
aww 
.ww 
Statusww $
!=ww% '
AppointmentStatusww( 9
.ww9 :
	Cancelledww: C
)wwC D
.xx 
ToListAsyncxx 
(xx 
)xx 
;xx 
foreachzz 
(zz 
varzz 
appointmentzz $
inzz% '
appointmentszz( 4
)zz4 5
{{{ 
appointment|| 
.|| 
Status|| "
=||# $
AppointmentStatus||% 6
.||6 7
	Cancelled||7 @
;||@ A
appointment}} 
.}} 
CancellationReason}} .
=}}/ 0
$str}}1 B
;}}B C
}~~ 
}
ÄÄ 	
}
ÇÇ 
}ÉÉ ¸q
MC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Program.cs
	namespace 	

HealthCare
 
. 
Api 
{ 
public 

partial 
class 
Program  
{ 
private 
static 
async 
Task !
Main" &
(& '
string' -
[- .
]. /
args0 4
)4 5
{ 	
var 
builder 
= 
WebApplication (
.( )
CreateBuilder) 6
(6 7
args7 ;
); <
;< =
builder 
. 
Services 
. 
AddAutoMapper *
(* +
cfg+ .
=>/ 1
{ 
cfg 
. 

AddProfile 
< 
MappingProfile -
>- .
(. /
)/ 0
;0 1
} 
) 
; 
builder   
.   
Services   
.   
AddProblemDetails   .
(  . /
)  / 0
;  0 1
builder!! 
.!! 
Services!! 
.!! 
AddExceptionHandler!! 0
<!!0 1"
GlobalExceptionHandler!!1 G
>!!G H
(!!H I
)!!I J
;!!J K
builder## 
.## 
Services## 
.## 
AddControllers## +
(##+ ,
)##, -
;##- .
builder$$ 
.$$ 
Services$$ 
.$$ #
AddEndpointsApiExplorer$$ 4
($$4 5
)$$5 6
;$$6 7
builder'' 
.'' 
Services'' 
.'' 
AddDbContext'' )
<'') *
HealthCareDbContext''* =
>''= >
(''> ?
options''? F
=>''G I
options(( 
.(( 
UseSqlServer(( $
((($ %
builder((% ,
.((, -
Configuration((- :
.((: ;
GetConnectionString((; N
(((N O
$str((O W
)((W X
)((X Y
))) 
;)) 
builder,, 
.,, 
Services,, 
.,, 
AddCors,, $
(,,$ %
options,,% ,
=>,,- /
{-- 
options.. 
... 
	AddPolicy.. !
(..! "
$str.." ,
,.., -
policy... 4
=>..5 7
policy..8 >
...> ?
AllowAnyOrigin..? M
(..M N
)..N O
...O P
AllowAnyHeader..P ^
(..^ _
).._ `
...` a
AllowAnyMethod..a o
(..o p
)..p q
)..q r
;..r s
}// 
)// 
;// 
builder22 
.22 
Services22 
.22 
AddIdentity22 (
<22( )
IdentityUser22) 5
,225 6
IdentityRole227 C
>22C D
(22D E
options22E L
=>22M O
{33 
options44 
.44 
User44 
.44 
RequireUniqueEmail44 /
=440 1
true442 6
;446 7
options55 
.55 
Password55  
.55  !
RequireDigit55! -
=55. /
true550 4
;554 5
options66 
.66 
Password66  
.66  !
RequireUppercase66! 1
=662 3
true664 8
;668 9
options77 
.77 
Password77  
.77  !"
RequireNonAlphanumeric77! 7
=778 9
true77: >
;77> ?
options88 
.88 
Password88  
.88  !
RequiredLength88! /
=880 1
$num882 3
;883 4
}99 
)99 
.:: $
AddEntityFrameworkStores:: (
<::( )
HealthCareDbContext::) <
>::< =
(::= >
)::> ?
.::? @$
AddDefaultTokenProviders::@ X
(::X Y
)::Y Z
;::Z [
builder>> 
.>> 
Services>> 
.>> 
AddAuthentication>> .
(>>. /
JwtBearerDefaults>>/ @
.>>@ A 
AuthenticationScheme>>A U
)>>U V
.?? 
AddJwtBearer?? 
(?? 
options?? !
=>??" $
{@@ 
varAA 
jwtAA 
=AA 
builderAA !
.AA! "
ConfigurationAA" /
.AA/ 0

GetSectionAA0 :
(AA: ;
$strAA; @
)AA@ A
;AAA B
optionsCC 
.CC %
TokenValidationParametersCC 1
=CC2 3
newCC4 7%
TokenValidationParametersCC8 Q
{DD 
ValidateIssuerEE "
=EE# $
trueEE% )
,EE) *
ValidIssuerFF 
=FF  !
jwtFF" %
[FF% &
$strFF& .
]FF. /
,FF/ 0
ValidateAudienceGG $
=GG% &
trueGG' +
,GG+ ,
ValidAudienceHH !
=HH" #
jwtHH$ '
[HH' (
$strHH( 2
]HH2 3
,HH3 4
ValidateLifetimeII $
=II% &
trueII' +
,II+ ,$
ValidateIssuerSigningKeyJJ ,
=JJ- .
trueJJ/ 3
,JJ3 4
IssuerSigningKeyKK $
=KK% &
newKK' * 
SymmetricSecurityKeyKK+ ?
(KK? @
EncodingKK@ H
.KKH I
UTF8KKI M
.KKM N
GetBytesKKN V
(KKV W
jwtKKW Z
[KKZ [
$strKK[ `
]KK` a
!KKa b
)KKb c
)KKc d
,KKd e
RoleClaimTypeLL !
=LL" #

ClaimTypesLL$ .
.LL. /
RoleLL/ 3
,LL3 4
NameClaimTypeMM !
=MM" #

ClaimTypesMM$ .
.MM. /
NameIdentifierMM/ =
,MM= >
	ClockSkewNN 
=NN 
TimeSpanNN  (
.NN( )
ZeroNN) -
}OO 
;OO 
optionsPP 
.PP 
EventsPP 
=PP  
newPP! $
JwtBearerEventsPP% 4
{QQ "
OnAuthenticationFailedRR *
=RR+ ,
contextRR- 4
=>RR5 7
{SS 
ConsoleUU 
.UU  
	WriteLineUU  )
(UU) *
$"UU* ,
$strUU, 7
{UU7 8
contextUU8 ?
.UU? @
	ExceptionUU@ I
.UUI J
MessageUUJ Q
}UUQ R
"UUR S
)UUS T
;UUT U
returnVV 
TaskVV #
.VV# $
CompletedTaskVV$ 1
;VV1 2
}WW 
}XX 
;XX 
}YY 
)YY 
;YY 
builder[[ 
.[[ 
Services[[ 
.[[ 
AddAuthorization[[ -
([[- .
)[[. /
;[[/ 0
builder\\ 
.\\ 
Services\\ 
.\\ 
	AddScoped\\ &
(\\& '
typeof\\' -
(\\- .
IRepository\\. 9
<\\9 :
>\\: ;
)\\; <
,\\< =
typeof\\> D
(\\D E

Repository\\E O
<\\O P
>\\P Q
)\\Q R
)\\R S
;\\S T
builder]] 
.]] 
Services]] 
.]] 
	AddScoped]] &
<]]& '
IPatientRepository]]' 9
,]]9 :
PatientRepository]]; L
>]]L M
(]]M N
)]]N O
;]]O P
builder^^ 
.^^ 
Services^^ 
.^^ 
	AddScoped^^ &
<^^& '
IDoctorRepository^^' 8
,^^8 9
DoctorRepository^^: J
>^^J K
(^^K L
)^^L M
;^^M N
builder__ 
.__ 
Services__ 
.__ 
	AddScoped__ &
<__& '"
IAppointmentRepository__' =
,__= >!
AppointmentRepository__? T
>__T U
(__U V
)__V W
;__W X
builder`` 
.`` 
Services`` 
.`` 
	AddScoped`` &
<``& '#
IHealthRecordRepository``' >
,``> ?"
HealthRecordRepository``@ V
>``V W
(``W X
)``X Y
;``Y Z
builderbb 
.bb 
Servicesbb 
.bb 
AddSwaggerGenbb *
(bb* +
)bb+ ,
;bb, -
buildercc 
.cc 
Servicescc 
.cc 
	AddScopedcc &
<cc& '
IAuthServicecc' 3
,cc3 4
AuthServicecc5 @
>cc@ A
(ccA B
)ccB C
;ccC D
builderdd 
.dd 
Servicesdd 
.dd 
	AddScopeddd &
<dd& '
IPatientServicedd' 6
,dd6 7
PatientServicedd8 F
>ddF G
(ddG H
)ddH I
;ddI J
builderee 
.ee 
Servicesee 
.ee 
	AddScopedee &
<ee& '
IDoctorServiceee' 5
,ee5 6
DoctorServiceee7 D
>eeD E
(eeE F
)eeF G
;eeG H
builderff 
.ff 
Servicesff 
.ff 
	AddScopedff &
<ff& '
IAppointmentServiceff' :
,ff: ;
AppointmentServiceff< N
>ffN O
(ffO P
)ffP Q
;ffQ R
buildergg 
.gg 
Servicesgg 
.gg 
	AddScopedgg &
<gg& ' 
IHealthRecordServicegg' ;
,gg; <
HealthRecordServicegg= P
>ggP Q
(ggQ R
)ggR S
;ggS T
builderhh 
.hh 
Serviceshh 
.hh #
AddEndpointsApiExplorerhh 4
(hh4 5
)hh5 6
;hh6 7
buildermm 
.mm 
Servicesmm 
.mm 
AddSwaggerGenmm *
(mm* +
optionsmm+ 2
=>mm3 5
{nn 
optionsoo 
.oo 

SwaggerDocoo "
(oo" #
$stroo# '
,oo' (
newoo) ,
OpenApiInfooo- 8
{pp 
Titleqq 
=qq 
$strqq +
,qq+ ,
Versionrr 
=rr 
$strrr "
}ss 
)ss 
;ss 
optionsuu 
.uu !
AddSecurityDefinitionuu -
(uu- .
$struu. 6
,uu6 7
newuu8 ;!
OpenApiSecuritySchemeuu< Q
{vv 
Typeww 
=ww 
SecuritySchemeTypeww -
.ww- .
Httpww. 2
,ww2 3
Schemexx 
=xx 
$strxx %
,xx% &
BearerFormatyy  
=yy! "
$stryy# (
,yy( )
Descriptionzz 
=zz  !
$strzz" M
}{{ 
){{ 
;{{ 
options}} 
.}} "
AddSecurityRequirement}} .
(}}. /
document}}/ 7
=>}}8 :
new}}; >&
OpenApiSecurityRequirement}}? Y
{~~ 
[ 
new *
OpenApiSecuritySchemeReference 7
(7 8
$str8 @
,@ A
documentB J
)J K
]K L
=M N
[O P
]P Q
}
ÄÄ 
)
ÄÄ 
;
ÄÄ 
}
ÅÅ 
)
ÅÅ 
;
ÅÅ 
var
ÉÉ 
app
ÉÉ 
=
ÉÉ 
builder
ÉÉ 
.
ÉÉ 
Build
ÉÉ #
(
ÉÉ# $
)
ÉÉ$ %
;
ÉÉ% &
app
ÑÑ 
.
ÑÑ !
UseExceptionHandler
ÑÑ #
(
ÑÑ# $
)
ÑÑ$ %
;
ÑÑ% &
using
ÜÜ 
(
ÜÜ 
var
ÜÜ 
scope
ÜÜ 
=
ÜÜ 
app
ÜÜ "
.
ÜÜ" #
Services
ÜÜ# +
.
ÜÜ+ ,
CreateScope
ÜÜ, 7
(
ÜÜ7 8
)
ÜÜ8 9
)
ÜÜ9 :
{
áá 
var
àà 
services
àà 
=
àà 
scope
àà $
.
àà$ %
ServiceProvider
àà% 4
;
àà4 5
var
ââ 
roleManager
ââ 
=
ââ  !
scope
ââ" '
.
ââ' (
ServiceProvider
ââ( 7
.
ââ7 8 
GetRequiredService
ââ8 J
<
ââJ K
RoleManager
ââK V
<
ââV W
IdentityRole
ââW c
>
ââc d
>
ââd e
(
ââe f
)
ââf g
;
ââg h
var
ää 
userManager
ää 
=
ää  !
services
ää" *
.
ää* + 
GetRequiredService
ää+ =
<
ää= >
UserManager
ää> I
<
ääI J
IdentityUser
ääJ V
>
ääV W
>
ääW X
(
ääX Y
)
ääY Z
;
ääZ [
await
ãã 

RoleSeeder
ãã  
.
ãã  !
SeedRoleAsync
ãã! .
(
ãã. /
roleManager
ãã/ :
)
ãã: ;
;
ãã; <
await
åå 
AdminSeeder
åå !
.
åå! "
SeedAdminAsync
åå" 0
(
åå0 1
userManager
åå2 =
,
åå= >
roleManager
åå> I
,
ååI J
builder
ååJ Q
.
ååQ R
Configuration
ååR _
)
åå_ `
;
åå` a
}
éé 
if
êê 
(
êê 
app
êê 
.
êê 
Environment
êê 
.
êê  
IsDevelopment
êê  -
(
êê- .
)
êê. /
)
êê/ 0
{
ëë 
app
íí 
.
íí 

UseSwagger
íí 
(
íí 
)
íí  
;
íí  !
app
ìì 
.
ìì 
UseSwaggerUI
ìì  
(
ìì  !
)
ìì! "
;
ìì" #
}
ïï 
app
óó 
.
óó 
UseCors
óó 
(
óó 
$str
óó "
)
óó" #
;
óó# $
app
ôô 
.
ôô !
UseHttpsRedirection
ôô #
(
ôô# $
)
ôô$ %
;
ôô% &
app
öö 
.
öö 

UseRouting
öö 
(
öö 
)
öö 
;
öö 
app
úú 
.
úú 
UseAuthentication
úú !
(
úú! "
)
úú" #
;
úú# $
app
ûû 
.
ûû 
UseAuthorization
ûû  
(
ûû  !
)
ûû! "
;
ûû" #
app
†† 
.
†† 
MapControllers
†† 
(
†† 
)
††  
;
††  !
await
¢¢ 
app
¢¢ 
.
¢¢ 
RunAsync
¢¢ 
(
¢¢ 
)
¢¢  
;
¢¢  !
}
££ 	
}
§§ 
}•• à
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
 
public 
Patient 
? 
Patient 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} È
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
} ∫
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
}++ ≤
rC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260629134507_UserAndPatientConn.cs
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
class 
UserAndPatientConn +
:, -
	Migration. 7
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
} †
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260621053507_SeedAdmin.cs
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
class 
	SeedAdmin "
:# $
	Migration% .
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
} ò
eC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260621053159_Roles.cs
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
class 
Roles 
:  
	Migration! *
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
} Áƒ
rC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Migrations\20260621052233_InitialUserRemoved.cs
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
InitialUserRemoved		 +
:		, -
	Migration		. 7
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
,  U V
Discriminator!! !
=!!" #
table!!$ )
.!!) *
Column!!* 0
<!!0 1
string!!1 7
>!!7 8
(!!8 9
type!!9 =
:!!= >
$str!!? M
,!!M N
	maxLength!!O X
:!!X Y
$num!!Z \
,!!\ ]
nullable!!^ f
:!!f g
false!!h m
)!!m n
,!!n o
UserName"" 
="" 
table"" $
.""$ %
Column""% +
<""+ ,
string"", 2
>""2 3
(""3 4
type""4 8
:""8 9
$str"": I
,""I J
	maxLength""K T
:""T U
$num""V Y
,""Y Z
nullable""[ c
:""c d
true""e i
)""i j
,""j k
NormalizedUserName## &
=##' (
table##) .
.##. /
Column##/ 5
<##5 6
string##6 <
>##< =
(##= >
type##> B
:##B C
$str##D S
,##S T
	maxLength##U ^
:##^ _
$num##` c
,##c d
nullable##e m
:##m n
true##o s
)##s t
,##t u
Email$$ 
=$$ 
table$$ !
.$$! "
Column$$" (
<$$( )
string$$) /
>$$/ 0
($$0 1
type$$1 5
:$$5 6
$str$$7 F
,$$F G
	maxLength$$H Q
:$$Q R
$num$$S V
,$$V W
nullable$$X `
:$$` a
true$$b f
)$$f g
,$$g h
NormalizedEmail%% #
=%%$ %
table%%& +
.%%+ ,
Column%%, 2
<%%2 3
string%%3 9
>%%9 :
(%%: ;
type%%; ?
:%%? @
$str%%A P
,%%P Q
	maxLength%%R [
:%%[ \
$num%%] `
,%%` a
nullable%%b j
:%%j k
true%%l p
)%%p q
,%%q r
EmailConfirmed&& "
=&&# $
table&&% *
.&&* +
Column&&+ 1
<&&1 2
bool&&2 6
>&&6 7
(&&7 8
type&&8 <
:&&< =
$str&&> C
,&&C D
nullable&&E M
:&&M N
false&&O T
)&&T U
,&&U V
PasswordHash''  
=''! "
table''# (
.''( )
Column'') /
<''/ 0
string''0 6
>''6 7
(''7 8
type''8 <
:''< =
$str''> M
,''M N
nullable''O W
:''W X
true''Y ]
)''] ^
,''^ _
SecurityStamp(( !
=((" #
table(($ )
.(() *
Column((* 0
<((0 1
string((1 7
>((7 8
(((8 9
type((9 =
:((= >
$str((? N
,((N O
nullable((P X
:((X Y
true((Z ^
)((^ _
,((_ `
ConcurrencyStamp)) $
=))% &
table))' ,
.)), -
Column))- 3
<))3 4
string))4 :
>)): ;
()); <
type))< @
:))@ A
$str))B Q
,))Q R
nullable))S [
:))[ \
true))] a
)))a b
,))b c
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
$str**= L
,**L M
nullable**N V
:**V W
true**X \
)**\ ]
,**] ^ 
PhoneNumberConfirmed++ (
=++) *
table+++ 0
.++0 1
Column++1 7
<++7 8
bool++8 <
>++< =
(++= >
type++> B
:++B C
$str++D I
,++I J
nullable++K S
:++S T
false++U Z
)++Z [
,++[ \
TwoFactorEnabled,, $
=,,% &
table,,' ,
.,,, -
Column,,- 3
<,,3 4
bool,,4 8
>,,8 9
(,,9 :
type,,: >
:,,> ?
$str,,@ E
,,,E F
nullable,,G O
:,,O P
false,,Q V
),,V W
,,,W X

LockoutEnd-- 
=--  
table--! &
.--& '
Column--' -
<--- .
DateTimeOffset--. <
>--< =
(--= >
type--> B
:--B C
$str--D T
,--T U
nullable--V ^
:--^ _
true--` d
)--d e
,--e f
LockoutEnabled.. "
=..# $
table..% *
...* +
Column..+ 1
<..1 2
bool..2 6
>..6 7
(..7 8
type..8 <
:..< =
$str..> C
,..C D
nullable..E M
:..M N
false..O T
)..T U
,..U V
AccessFailedCount// %
=//& '
table//( -
.//- .
Column//. 4
<//4 5
int//5 8
>//8 9
(//9 :
type//: >
://> ?
$str//@ E
,//E F
nullable//G O
://O P
false//Q V
)//V W
}00 
,00 
constraints11 
:11 
table11 "
=>11# %
{22 
table33 
.33 

PrimaryKey33 $
(33$ %
$str33% 5
,335 6
x337 8
=>339 ;
x33< =
.33= >
Id33> @
)33@ A
;33A B
}44 
)44 
;44 
migrationBuilder66 
.66 
CreateTable66 (
(66( )
name77 
:77 
$str77 (
,77( )
columns88 
:88 
table88 
=>88 !
new88" %
{99 
Id:: 
=:: 
table:: 
.:: 
Column:: %
<::% &
int::& )
>::) *
(::* +
type::+ /
:::/ 0
$str::1 6
,::6 7
nullable::8 @
:::@ A
false::B G
)::G H
.;; 

Annotation;; #
(;;# $
$str;;$ 8
,;;8 9
$str;;: @
);;@ A
,;;A B
RoleId<< 
=<< 
table<< "
.<<" #
Column<<# )
<<<) *
string<<* 0
><<0 1
(<<1 2
type<<2 6
:<<6 7
$str<<8 G
,<<G H
nullable<<I Q
:<<Q R
false<<S X
)<<X Y
,<<Y Z
	ClaimType== 
=== 
table==  %
.==% &
Column==& ,
<==, -
string==- 3
>==3 4
(==4 5
type==5 9
:==9 :
$str==; J
,==J K
nullable==L T
:==T U
true==V Z
)==Z [
,==[ \

ClaimValue>> 
=>>  
table>>! &
.>>& '
Column>>' -
<>>- .
string>>. 4
>>>4 5
(>>5 6
type>>6 :
:>>: ;
$str>>< K
,>>K L
nullable>>M U
:>>U V
true>>W [
)>>[ \
}?? 
,?? 
constraints@@ 
:@@ 
table@@ "
=>@@# %
{AA 
tableBB 
.BB 

PrimaryKeyBB $
(BB$ %
$strBB% :
,BB: ;
xBB< =
=>BB> @
xBBA B
.BBB C
IdBBC E
)BBE F
;BBF G
tableCC 
.CC 

ForeignKeyCC $
(CC$ %
nameDD 
:DD 
$strDD F
,DDF G
columnEE 
:EE 
xEE  !
=>EE" $
xEE% &
.EE& '
RoleIdEE' -
,EE- .
principalTableFF &
:FF& '
$strFF( 5
,FF5 6
principalColumnGG '
:GG' (
$strGG) -
,GG- .
onDeleteHH  
:HH  !
ReferentialActionHH" 3
.HH3 4
CascadeHH4 ;
)HH; <
;HH< =
}II 
)II 
;II 
migrationBuilderKK 
.KK 
CreateTableKK (
(KK( )
nameLL 
:LL 
$strLL (
,LL( )
columnsMM 
:MM 
tableMM 
=>MM !
newMM" %
{NN 
IdOO 
=OO 
tableOO 
.OO 
ColumnOO %
<OO% &
intOO& )
>OO) *
(OO* +
typeOO+ /
:OO/ 0
$strOO1 6
,OO6 7
nullableOO8 @
:OO@ A
falseOOB G
)OOG H
.PP 

AnnotationPP #
(PP# $
$strPP$ 8
,PP8 9
$strPP: @
)PP@ A
,PPA B
UserIdQQ 
=QQ 
tableQQ "
.QQ" #
ColumnQQ# )
<QQ) *
stringQQ* 0
>QQ0 1
(QQ1 2
typeQQ2 6
:QQ6 7
$strQQ8 G
,QQG H
nullableQQI Q
:QQQ R
falseQQS X
)QQX Y
,QQY Z
	ClaimTypeRR 
=RR 
tableRR  %
.RR% &
ColumnRR& ,
<RR, -
stringRR- 3
>RR3 4
(RR4 5
typeRR5 9
:RR9 :
$strRR; J
,RRJ K
nullableRRL T
:RRT U
trueRRV Z
)RRZ [
,RR[ \

ClaimValueSS 
=SS  
tableSS! &
.SS& '
ColumnSS' -
<SS- .
stringSS. 4
>SS4 5
(SS5 6
typeSS6 :
:SS: ;
$strSS< K
,SSK L
nullableSSM U
:SSU V
trueSSW [
)SS[ \
}TT 
,TT 
constraintsUU 
:UU 
tableUU "
=>UU# %
{VV 
tableWW 
.WW 

PrimaryKeyWW $
(WW$ %
$strWW% :
,WW: ;
xWW< =
=>WW> @
xWWA B
.WWB C
IdWWC E
)WWE F
;WWF G
tableXX 
.XX 

ForeignKeyXX $
(XX$ %
nameYY 
:YY 
$strYY F
,YYF G
columnZZ 
:ZZ 
xZZ  !
=>ZZ" $
xZZ% &
.ZZ& '
UserIdZZ' -
,ZZ- .
principalTable[[ &
:[[& '
$str[[( 5
,[[5 6
principalColumn\\ '
:\\' (
$str\\) -
,\\- .
onDelete]]  
:]]  !
ReferentialAction]]" 3
.]]3 4
Cascade]]4 ;
)]]; <
;]]< =
}^^ 
)^^ 
;^^ 
migrationBuilder`` 
.`` 
CreateTable`` (
(``( )
nameaa 
:aa 
$straa (
,aa( )
columnsbb 
:bb 
tablebb 
=>bb !
newbb" %
{cc 
LoginProviderdd !
=dd" #
tabledd$ )
.dd) *
Columndd* 0
<dd0 1
stringdd1 7
>dd7 8
(dd8 9
typedd9 =
:dd= >
$strdd? N
,ddN O
nullableddP X
:ddX Y
falseddZ _
)dd_ `
,dd` a
ProviderKeyee 
=ee  !
tableee" '
.ee' (
Columnee( .
<ee. /
stringee/ 5
>ee5 6
(ee6 7
typeee7 ;
:ee; <
$stree= L
,eeL M
nullableeeN V
:eeV W
falseeeX ]
)ee] ^
,ee^ _
ProviderDisplayNameff '
=ff( )
tableff* /
.ff/ 0
Columnff0 6
<ff6 7
stringff7 =
>ff= >
(ff> ?
typeff? C
:ffC D
$strffE T
,ffT U
nullableffV ^
:ff^ _
trueff` d
)ffd e
,ffe f
UserIdgg 
=gg 
tablegg "
.gg" #
Columngg# )
<gg) *
stringgg* 0
>gg0 1
(gg1 2
typegg2 6
:gg6 7
$strgg8 G
,ggG H
nullableggI Q
:ggQ R
falseggS X
)ggX Y
}hh 
,hh 
constraintsii 
:ii 
tableii "
=>ii# %
{jj 
tablekk 
.kk 

PrimaryKeykk $
(kk$ %
$strkk% :
,kk: ;
xkk< =
=>kk> @
newkkA D
{kkE F
xkkG H
.kkH I
LoginProviderkkI V
,kkV W
xkkX Y
.kkY Z
ProviderKeykkZ e
}kkf g
)kkg h
;kkh i
tablell 
.ll 

ForeignKeyll $
(ll$ %
namemm 
:mm 
$strmm F
,mmF G
columnnn 
:nn 
xnn  !
=>nn" $
xnn% &
.nn& '
UserIdnn' -
,nn- .
principalTableoo &
:oo& '
$stroo( 5
,oo5 6
principalColumnpp '
:pp' (
$strpp) -
,pp- .
onDeleteqq  
:qq  !
ReferentialActionqq" 3
.qq3 4
Cascadeqq4 ;
)qq; <
;qq< =
}rr 
)rr 
;rr 
migrationBuildertt 
.tt 
CreateTablett (
(tt( )
nameuu 
:uu 
$struu '
,uu' (
columnsvv 
:vv 
tablevv 
=>vv !
newvv" %
{ww 
UserIdxx 
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
,xxY Z
RoleIdyy 
=yy 
tableyy "
.yy" #
Columnyy# )
<yy) *
stringyy* 0
>yy0 1
(yy1 2
typeyy2 6
:yy6 7
$stryy8 G
,yyG H
nullableyyI Q
:yyQ R
falseyyS X
)yyX Y
}zz 
,zz 
constraints{{ 
:{{ 
table{{ "
=>{{# %
{|| 
table}} 
.}} 

PrimaryKey}} $
(}}$ %
$str}}% 9
,}}9 :
x}}; <
=>}}= ?
new}}@ C
{}}D E
x}}F G
.}}G H
UserId}}H N
,}}N O
x}}P Q
.}}Q R
RoleId}}R X
}}}Y Z
)}}Z [
;}}[ \
table~~ 
.~~ 

ForeignKey~~ $
(~~$ %
name 
: 
$str E
,E F
column
ÄÄ 
:
ÄÄ 
x
ÄÄ  !
=>
ÄÄ" $
x
ÄÄ% &
.
ÄÄ& '
RoleId
ÄÄ' -
,
ÄÄ- .
principalTable
ÅÅ &
:
ÅÅ& '
$str
ÅÅ( 5
,
ÅÅ5 6
principalColumn
ÇÇ '
:
ÇÇ' (
$str
ÇÇ) -
,
ÇÇ- .
onDelete
ÉÉ  
:
ÉÉ  !
ReferentialAction
ÉÉ" 3
.
ÉÉ3 4
Cascade
ÉÉ4 ;
)
ÉÉ; <
;
ÉÉ< =
table
ÑÑ 
.
ÑÑ 

ForeignKey
ÑÑ $
(
ÑÑ$ %
name
ÖÖ 
:
ÖÖ 
$str
ÖÖ E
,
ÖÖE F
column
ÜÜ 
:
ÜÜ 
x
ÜÜ  !
=>
ÜÜ" $
x
ÜÜ% &
.
ÜÜ& '
UserId
ÜÜ' -
,
ÜÜ- .
principalTable
áá &
:
áá& '
$str
áá( 5
,
áá5 6
principalColumn
àà '
:
àà' (
$str
àà) -
,
àà- .
onDelete
ââ  
:
ââ  !
ReferentialAction
ââ" 3
.
ââ3 4
Cascade
ââ4 ;
)
ââ; <
;
ââ< =
}
ää 
)
ää 
;
ää 
migrationBuilder
åå 
.
åå 
CreateTable
åå (
(
åå( )
name
çç 
:
çç 
$str
çç (
,
çç( )
columns
éé 
:
éé 
table
éé 
=>
éé !
new
éé" %
{
èè 
UserId
êê 
=
êê 
table
êê "
.
êê" #
Column
êê# )
<
êê) *
string
êê* 0
>
êê0 1
(
êê1 2
type
êê2 6
:
êê6 7
$str
êê8 G
,
êêG H
nullable
êêI Q
:
êêQ R
false
êêS X
)
êêX Y
,
êêY Z
LoginProvider
ëë !
=
ëë" #
table
ëë$ )
.
ëë) *
Column
ëë* 0
<
ëë0 1
string
ëë1 7
>
ëë7 8
(
ëë8 9
type
ëë9 =
:
ëë= >
$str
ëë? N
,
ëëN O
nullable
ëëP X
:
ëëX Y
false
ëëZ _
)
ëë_ `
,
ëë` a
Name
íí 
=
íí 
table
íí  
.
íí  !
Column
íí! '
<
íí' (
string
íí( .
>
íí. /
(
íí/ 0
type
íí0 4
:
íí4 5
$str
íí6 E
,
ííE F
nullable
ííG O
:
ííO P
false
ííQ V
)
ííV W
,
ííW X
Value
ìì 
=
ìì 
table
ìì !
.
ìì! "
Column
ìì" (
<
ìì( )
string
ìì) /
>
ìì/ 0
(
ìì0 1
type
ìì1 5
:
ìì5 6
$str
ìì7 F
,
ììF G
nullable
ììH P
:
ììP Q
true
ììR V
)
ììV W
}
îî 
,
îî 
constraints
ïï 
:
ïï 
table
ïï "
=>
ïï# %
{
ññ 
table
óó 
.
óó 

PrimaryKey
óó $
(
óó$ %
$str
óó% :
,
óó: ;
x
óó< =
=>
óó> @
new
óóA D
{
óóE F
x
óóG H
.
óóH I
UserId
óóI O
,
óóO P
x
óóQ R
.
óóR S
LoginProvider
óóS `
,
óó` a
x
óób c
.
óóc d
Name
óód h
}
óói j
)
óój k
;
óók l
table
òò 
.
òò 

ForeignKey
òò $
(
òò$ %
name
ôô 
:
ôô 
$str
ôô F
,
ôôF G
column
öö 
:
öö 
x
öö  !
=>
öö" $
x
öö% &
.
öö& '
UserId
öö' -
,
öö- .
principalTable
õõ &
:
õõ& '
$str
õõ( 5
,
õõ5 6
principalColumn
úú '
:
úú' (
$str
úú) -
,
úú- .
onDelete
ùù  
:
ùù  !
ReferentialAction
ùù" 3
.
ùù3 4
Cascade
ùù4 ;
)
ùù; <
;
ùù< =
}
ûû 
)
ûû 
;
ûû 
migrationBuilder
†† 
.
†† 
CreateTable
†† (
(
††( )
name
°° 
:
°° 
$str
°° 
,
°°  
columns
¢¢ 
:
¢¢ 
table
¢¢ 
=>
¢¢ !
new
¢¢" %
{
££ 
DoctorId
§§ 
=
§§ 
table
§§ $
.
§§$ %
Column
§§% +
<
§§+ ,
int
§§, /
>
§§/ 0
(
§§0 1
type
§§1 5
:
§§5 6
$str
§§7 <
,
§§< =
nullable
§§> F
:
§§F G
false
§§H M
)
§§M N
.
•• 

Annotation
•• #
(
••# $
$str
••$ 8
,
••8 9
$str
••: @
)
••@ A
,
••A B
UserId
¶¶ 
=
¶¶ 
table
¶¶ "
.
¶¶" #
Column
¶¶# )
<
¶¶) *
string
¶¶* 0
>
¶¶0 1
(
¶¶1 2
type
¶¶2 6
:
¶¶6 7
$str
¶¶8 G
,
¶¶G H
nullable
¶¶I Q
:
¶¶Q R
false
¶¶S X
)
¶¶X Y
,
¶¶Y Z
FullName
ßß 
=
ßß 
table
ßß $
.
ßß$ %
Column
ßß% +
<
ßß+ ,
string
ßß, 2
>
ßß2 3
(
ßß3 4
type
ßß4 8
:
ßß8 9
$str
ßß: I
,
ßßI J
	maxLength
ßßK T
:
ßßT U
$num
ßßV Y
,
ßßY Z
nullable
ßß[ c
:
ßßc d
false
ßße j
)
ßßj k
,
ßßk l
Specialisation
®® "
=
®®# $
table
®®% *
.
®®* +
Column
®®+ 1
<
®®1 2
string
®®2 8
>
®®8 9
(
®®9 :
type
®®: >
:
®®> ?
$str
®®@ N
,
®®N O
	maxLength
®®P Y
:
®®Y Z
$num
®®[ ]
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
®®o p
YearsOfExperience
©© %
=
©©& '
table
©©( -
.
©©- .
Column
©©. 4
<
©©4 5
int
©©5 8
>
©©8 9
(
©©9 :
type
©©: >
:
©©> ?
$str
©©@ E
,
©©E F
nullable
©©G O
:
©©O P
false
©©Q V
)
©©V W
,
©©W X
ConsultationFee
™™ #
=
™™$ %
table
™™& +
.
™™+ ,
Column
™™, 2
<
™™2 3
decimal
™™3 :
>
™™: ;
(
™™; <
type
™™< @
:
™™@ A
$str
™™B Q
,
™™Q R
nullable
™™S [
:
™™[ \
false
™™] b
)
™™b c
,
™™c d
IsActive
´´ 
=
´´ 
table
´´ $
.
´´$ %
Column
´´% +
<
´´+ ,
bool
´´, 0
>
´´0 1
(
´´1 2
type
´´2 6
:
´´6 7
$str
´´8 =
,
´´= >
nullable
´´? G
:
´´G H
false
´´I N
)
´´N O
}
¨¨ 
,
¨¨ 
constraints
≠≠ 
:
≠≠ 
table
≠≠ "
=>
≠≠# %
{
ÆÆ 
table
ØØ 
.
ØØ 

PrimaryKey
ØØ $
(
ØØ$ %
$str
ØØ% 1
,
ØØ1 2
x
ØØ3 4
=>
ØØ5 7
x
ØØ8 9
.
ØØ9 :
DoctorId
ØØ: B
)
ØØB C
;
ØØC D
table
∞∞ 
.
∞∞ 

ForeignKey
∞∞ $
(
∞∞$ %
name
±± 
:
±± 
$str
±± =
,
±±= >
column
≤≤ 
:
≤≤ 
x
≤≤  !
=>
≤≤" $
x
≤≤% &
.
≤≤& '
UserId
≤≤' -
,
≤≤- .
principalTable
≥≥ &
:
≥≥& '
$str
≥≥( 5
,
≥≥5 6
principalColumn
¥¥ '
:
¥¥' (
$str
¥¥) -
,
¥¥- .
onDelete
µµ  
:
µµ  !
ReferentialAction
µµ" 3
.
µµ3 4
Cascade
µµ4 ;
)
µµ; <
;
µµ< =
}
∂∂ 
)
∂∂ 
;
∂∂ 
migrationBuilder
∏∏ 
.
∏∏ 
CreateTable
∏∏ (
(
∏∏( )
name
ππ 
:
ππ 
$str
ππ  
,
ππ  !
columns
∫∫ 
:
∫∫ 
table
∫∫ 
=>
∫∫ !
new
∫∫" %
{
ªª 
	PatientId
ºº 
=
ºº 
table
ºº  %
.
ºº% &
Column
ºº& ,
<
ºº, -
int
ºº- 0
>
ºº0 1
(
ºº1 2
type
ºº2 6
:
ºº6 7
$str
ºº8 =
,
ºº= >
nullable
ºº? G
:
ººG H
false
ººI N
)
ººN O
.
ΩΩ 

Annotation
ΩΩ #
(
ΩΩ# $
$str
ΩΩ$ 8
,
ΩΩ8 9
$str
ΩΩ: @
)
ΩΩ@ A
,
ΩΩA B
UserId
ææ 
=
ææ 
table
ææ "
.
ææ" #
Column
ææ# )
<
ææ) *
string
ææ* 0
>
ææ0 1
(
ææ1 2
type
ææ2 6
:
ææ6 7
$str
ææ8 G
,
ææG H
nullable
ææI Q
:
ææQ R
true
ææS W
)
ææW X
,
ææX Y
FullName
øø 
=
øø 
table
øø $
.
øø$ %
Column
øø% +
<
øø+ ,
string
øø, 2
>
øø2 3
(
øø3 4
type
øø4 8
:
øø8 9
$str
øø: I
,
øøI J
nullable
øøK S
:
øøS T
false
øøU Z
)
øøZ [
,
øø[ \
DateOfBirth
¿¿ 
=
¿¿  !
table
¿¿" '
.
¿¿' (
Column
¿¿( .
<
¿¿. /
DateOnly
¿¿/ 7
>
¿¿7 8
(
¿¿8 9
type
¿¿9 =
:
¿¿= >
$str
¿¿? E
,
¿¿E F
nullable
¿¿G O
:
¿¿O P
false
¿¿Q V
)
¿¿V W
,
¿¿W X
Gender
¡¡ 
=
¡¡ 
table
¡¡ "
.
¡¡" #
Column
¡¡# )
<
¡¡) *
string
¡¡* 0
>
¡¡0 1
(
¡¡1 2
type
¡¡2 6
:
¡¡6 7
$str
¡¡8 G
,
¡¡G H
nullable
¡¡I Q
:
¡¡Q R
false
¡¡S X
)
¡¡X Y
,
¡¡Y Z
PhoneNumber
¬¬ 
=
¬¬  !
table
¬¬" '
.
¬¬' (
Column
¬¬( .
<
¬¬. /
string
¬¬/ 5
>
¬¬5 6
(
¬¬6 7
type
¬¬7 ;
:
¬¬; <
$str
¬¬= L
,
¬¬L M
nullable
¬¬N V
:
¬¬V W
true
¬¬X \
)
¬¬\ ]
,
¬¬] ^
InsuranceId
√√ 
=
√√  !
table
√√" '
.
√√' (
Column
√√( .
<
√√. /
string
√√/ 5
>
√√5 6
(
√√6 7
type
√√7 ;
:
√√; <
$str
√√= L
,
√√L M
nullable
√√N V
:
√√V W
true
√√X \
)
√√\ ]
,
√√] ^
IsActive
ƒƒ 
=
ƒƒ 
table
ƒƒ $
.
ƒƒ$ %
Column
ƒƒ% +
<
ƒƒ+ ,
bool
ƒƒ, 0
>
ƒƒ0 1
(
ƒƒ1 2
type
ƒƒ2 6
:
ƒƒ6 7
$str
ƒƒ8 =
,
ƒƒ= >
nullable
ƒƒ? G
:
ƒƒG H
false
ƒƒI N
)
ƒƒN O
}
≈≈ 
,
≈≈ 
constraints
∆∆ 
:
∆∆ 
table
∆∆ "
=>
∆∆# %
{
«« 
table
»» 
.
»» 

PrimaryKey
»» $
(
»»$ %
$str
»»% 2
,
»»2 3
x
»»4 5
=>
»»6 8
x
»»9 :
.
»»: ;
	PatientId
»»; D
)
»»D E
;
»»E F
table
…… 
.
…… 

ForeignKey
…… $
(
……$ %
name
   
:
   
$str
   >
,
  > ?
column
ÀÀ 
:
ÀÀ 
x
ÀÀ  !
=>
ÀÀ" $
x
ÀÀ% &
.
ÀÀ& '
UserId
ÀÀ' -
,
ÀÀ- .
principalTable
ÃÃ &
:
ÃÃ& '
$str
ÃÃ( 5
,
ÃÃ5 6
principalColumn
ÕÕ '
:
ÕÕ' (
$str
ÕÕ) -
,
ÕÕ- .
onDelete
ŒŒ  
:
ŒŒ  !
ReferentialAction
ŒŒ" 3
.
ŒŒ3 4
Cascade
ŒŒ4 ;
)
ŒŒ; <
;
ŒŒ< =
}
œœ 
)
œœ 
;
œœ 
migrationBuilder
—— 
.
—— 
CreateTable
—— (
(
——( )
name
““ 
:
““ 
$str
““ &
,
““& '
columns
”” 
:
”” 
table
”” 
=>
”” !
new
””" %
{
‘‘ 
Id
’’ 
=
’’ 
table
’’ 
.
’’ 
Column
’’ %
<
’’% &
int
’’& )
>
’’) *
(
’’* +
type
’’+ /
:
’’/ 0
$str
’’1 6
,
’’6 7
nullable
’’8 @
:
’’@ A
false
’’B G
)
’’G H
.
÷÷ 

Annotation
÷÷ #
(
÷÷# $
$str
÷÷$ 8
,
÷÷8 9
$str
÷÷: @
)
÷÷@ A
,
÷÷A B
DoctorId
◊◊ 
=
◊◊ 
table
◊◊ $
.
◊◊$ %
Column
◊◊% +
<
◊◊+ ,
int
◊◊, /
>
◊◊/ 0
(
◊◊0 1
type
◊◊1 5
:
◊◊5 6
$str
◊◊7 <
,
◊◊< =
nullable
◊◊> F
:
◊◊F G
false
◊◊H M
)
◊◊M N
,
◊◊N O
TimeSlot
ÿÿ 
=
ÿÿ 
table
ÿÿ $
.
ÿÿ$ %
Column
ÿÿ% +
<
ÿÿ+ ,
string
ÿÿ, 2
>
ÿÿ2 3
(
ÿÿ3 4
type
ÿÿ4 8
:
ÿÿ8 9
$str
ÿÿ: H
,
ÿÿH I
	maxLength
ÿÿJ S
:
ÿÿS T
$num
ÿÿU W
,
ÿÿW X
nullable
ÿÿY a
:
ÿÿa b
false
ÿÿc h
)
ÿÿh i
,
ÿÿi j
CreatedDate
ŸŸ 
=
ŸŸ  !
table
ŸŸ" '
.
ŸŸ' (
Column
ŸŸ( .
<
ŸŸ. /
DateTimeOffset
ŸŸ/ =
>
ŸŸ= >
(
ŸŸ> ?
type
ŸŸ? C
:
ŸŸC D
$str
ŸŸE U
,
ŸŸU V
nullable
ŸŸW _
:
ŸŸ_ `
false
ŸŸa f
)
ŸŸf g
}
⁄⁄ 
,
⁄⁄ 
constraints
€€ 
:
€€ 
table
€€ "
=>
€€# %
{
‹‹ 
table
›› 
.
›› 

PrimaryKey
›› $
(
››$ %
$str
››% 8
,
››8 9
x
››: ;
=>
››< >
x
››? @
.
››@ A
Id
››A C
)
››C D
;
››D E
table
ﬁﬁ 
.
ﬁﬁ 

ForeignKey
ﬁﬁ $
(
ﬁﬁ$ %
name
ﬂﬂ 
:
ﬂﬂ 
$str
ﬂﬂ B
,
ﬂﬂB C
column
‡‡ 
:
‡‡ 
x
‡‡  !
=>
‡‡" $
x
‡‡% &
.
‡‡& '
DoctorId
‡‡' /
,
‡‡/ 0
principalTable
·· &
:
··& '
$str
··( 1
,
··1 2
principalColumn
‚‚ '
:
‚‚' (
$str
‚‚) 3
,
‚‚3 4
onDelete
„„  
:
„„  !
ReferentialAction
„„" 3
.
„„3 4
Cascade
„„4 ;
)
„„; <
;
„„< =
}
‰‰ 
)
‰‰ 
;
‰‰ 
migrationBuilder
ÊÊ 
.
ÊÊ 
CreateTable
ÊÊ (
(
ÊÊ( )
name
ÁÁ 
:
ÁÁ 
$str
ÁÁ $
,
ÁÁ$ %
columns
ËË 
:
ËË 
table
ËË 
=>
ËË !
new
ËË" %
{
ÈÈ 
Id
ÍÍ 
=
ÍÍ 
table
ÍÍ 
.
ÍÍ 
Column
ÍÍ %
<
ÍÍ% &
int
ÍÍ& )
>
ÍÍ) *
(
ÍÍ* +
type
ÍÍ+ /
:
ÍÍ/ 0
$str
ÍÍ1 6
,
ÍÍ6 7
nullable
ÍÍ8 @
:
ÍÍ@ A
false
ÍÍB G
)
ÍÍG H
.
ÎÎ 

Annotation
ÎÎ #
(
ÎÎ# $
$str
ÎÎ$ 8
,
ÎÎ8 9
$str
ÎÎ: @
)
ÎÎ@ A
,
ÎÎA B
DoctorId
ÏÏ 
=
ÏÏ 
table
ÏÏ $
.
ÏÏ$ %
Column
ÏÏ% +
<
ÏÏ+ ,
int
ÏÏ, /
>
ÏÏ/ 0
(
ÏÏ0 1
type
ÏÏ1 5
:
ÏÏ5 6
$str
ÏÏ7 <
,
ÏÏ< =
nullable
ÏÏ> F
:
ÏÏF G
false
ÏÏH M
)
ÏÏM N
,
ÏÏN O
	LeaveDate
ÌÌ 
=
ÌÌ 
table
ÌÌ  %
.
ÌÌ% &
Column
ÌÌ& ,
<
ÌÌ, -
DateOnly
ÌÌ- 5
>
ÌÌ5 6
(
ÌÌ6 7
type
ÌÌ7 ;
:
ÌÌ; <
$str
ÌÌ= C
,
ÌÌC D
nullable
ÌÌE M
:
ÌÌM N
false
ÌÌO T
)
ÌÌT U
,
ÌÌU V
Reason
ÓÓ 
=
ÓÓ 
table
ÓÓ "
.
ÓÓ" #
Column
ÓÓ# )
<
ÓÓ) *
string
ÓÓ* 0
>
ÓÓ0 1
(
ÓÓ1 2
type
ÓÓ2 6
:
ÓÓ6 7
$str
ÓÓ8 G
,
ÓÓG H
	maxLength
ÓÓI R
:
ÓÓR S
$num
ÓÓT W
,
ÓÓW X
nullable
ÓÓY a
:
ÓÓa b
true
ÓÓc g
)
ÓÓg h
,
ÓÓh i

CreateDate
ÔÔ 
=
ÔÔ  
table
ÔÔ! &
.
ÔÔ& '
Column
ÔÔ' -
<
ÔÔ- .
DateTimeOffset
ÔÔ. <
>
ÔÔ< =
(
ÔÔ= >
type
ÔÔ> B
:
ÔÔB C
$str
ÔÔD T
,
ÔÔT U
nullable
ÔÔV ^
:
ÔÔ^ _
false
ÔÔ` e
)
ÔÔe f
}
 
,
 
constraints
ÒÒ 
:
ÒÒ 
table
ÒÒ "
=>
ÒÒ# %
{
ÚÚ 
table
ÛÛ 
.
ÛÛ 

PrimaryKey
ÛÛ $
(
ÛÛ$ %
$str
ÛÛ% 6
,
ÛÛ6 7
x
ÛÛ8 9
=>
ÛÛ: <
x
ÛÛ= >
.
ÛÛ> ?
Id
ÛÛ? A
)
ÛÛA B
;
ÛÛB C
table
ÙÙ 
.
ÙÙ 

ForeignKey
ÙÙ $
(
ÙÙ$ %
name
ıı 
:
ıı 
$str
ıı @
,
ıı@ A
column
ˆˆ 
:
ˆˆ 
x
ˆˆ  !
=>
ˆˆ" $
x
ˆˆ% &
.
ˆˆ& '
DoctorId
ˆˆ' /
,
ˆˆ/ 0
principalTable
˜˜ &
:
˜˜& '
$str
˜˜( 1
,
˜˜1 2
principalColumn
¯¯ '
:
¯¯' (
$str
¯¯) 3
,
¯¯3 4
onDelete
˘˘  
:
˘˘  !
ReferentialAction
˘˘" 3
.
˘˘3 4
Cascade
˘˘4 ;
)
˘˘; <
;
˘˘< =
}
˙˙ 
)
˙˙ 
;
˙˙ 
migrationBuilder
¸¸ 
.
¸¸ 
CreateTable
¸¸ (
(
¸¸( )
name
˝˝ 
:
˝˝ 
$str
˝˝ $
,
˝˝$ %
columns
˛˛ 
:
˛˛ 
table
˛˛ 
=>
˛˛ !
new
˛˛" %
{
ˇˇ 
AppointmentId
ÄÄ !
=
ÄÄ" #
table
ÄÄ$ )
.
ÄÄ) *
Column
ÄÄ* 0
<
ÄÄ0 1
int
ÄÄ1 4
>
ÄÄ4 5
(
ÄÄ5 6
type
ÄÄ6 :
:
ÄÄ: ;
$str
ÄÄ< A
,
ÄÄA B
nullable
ÄÄC K
:
ÄÄK L
false
ÄÄM R
)
ÄÄR S
.
ÅÅ 

Annotation
ÅÅ #
(
ÅÅ# $
$str
ÅÅ$ 8
,
ÅÅ8 9
$str
ÅÅ: @
)
ÅÅ@ A
,
ÅÅA B
	PatientId
ÇÇ 
=
ÇÇ 
table
ÇÇ  %
.
ÇÇ% &
Column
ÇÇ& ,
<
ÇÇ, -
int
ÇÇ- 0
>
ÇÇ0 1
(
ÇÇ1 2
type
ÇÇ2 6
:
ÇÇ6 7
$str
ÇÇ8 =
,
ÇÇ= >
nullable
ÇÇ? G
:
ÇÇG H
false
ÇÇI N
)
ÇÇN O
,
ÇÇO P
DoctorId
ÉÉ 
=
ÉÉ 
table
ÉÉ $
.
ÉÉ$ %
Column
ÉÉ% +
<
ÉÉ+ ,
int
ÉÉ, /
>
ÉÉ/ 0
(
ÉÉ0 1
type
ÉÉ1 5
:
ÉÉ5 6
$str
ÉÉ7 <
,
ÉÉ< =
nullable
ÉÉ> F
:
ÉÉF G
false
ÉÉH M
)
ÉÉM N
,
ÉÉN O
ScheduledDate
ÑÑ !
=
ÑÑ" #
table
ÑÑ$ )
.
ÑÑ) *
Column
ÑÑ* 0
<
ÑÑ0 1
DateOnly
ÑÑ1 9
>
ÑÑ9 :
(
ÑÑ: ;
type
ÑÑ; ?
:
ÑÑ? @
$str
ÑÑA G
,
ÑÑG H
nullable
ÑÑI Q
:
ÑÑQ R
false
ÑÑS X
)
ÑÑX Y
,
ÑÑY Z
TimeSlot
ÖÖ 
=
ÖÖ 
table
ÖÖ $
.
ÖÖ$ %
Column
ÖÖ% +
<
ÖÖ+ ,
string
ÖÖ, 2
>
ÖÖ2 3
(
ÖÖ3 4
type
ÖÖ4 8
:
ÖÖ8 9
$str
ÖÖ: H
,
ÖÖH I
	maxLength
ÖÖJ S
:
ÖÖS T
$num
ÖÖU W
,
ÖÖW X
nullable
ÖÖY a
:
ÖÖa b
false
ÖÖc h
)
ÖÖh i
,
ÖÖi j
Status
ÜÜ 
=
ÜÜ 
table
ÜÜ "
.
ÜÜ" #
Column
ÜÜ# )
<
ÜÜ) *
string
ÜÜ* 0
>
ÜÜ0 1
(
ÜÜ1 2
type
ÜÜ2 6
:
ÜÜ6 7
$str
ÜÜ8 F
,
ÜÜF G
	maxLength
ÜÜH Q
:
ÜÜQ R
$num
ÜÜS U
,
ÜÜU V
nullable
ÜÜW _
:
ÜÜ_ `
false
ÜÜa f
)
ÜÜf g
,
ÜÜg h 
CancellationReason
áá &
=
áá' (
table
áá) .
.
áá. /
Column
áá/ 5
<
áá5 6
string
áá6 <
>
áá< =
(
áá= >
type
áá> B
:
ááB C
$str
ááD S
,
ááS T
	maxLength
ááU ^
:
áá^ _
$num
áá` c
,
áác d
nullable
ááe m
:
áám n
true
ááo s
)
áás t
,
áát u
CreatedDate
àà 
=
àà  !
table
àà" '
.
àà' (
Column
àà( .
<
àà. /
DateTimeOffset
àà/ =
>
àà= >
(
àà> ?
type
àà? C
:
ààC D
$str
ààE U
,
ààU V
nullable
ààW _
:
àà_ `
false
ààa f
)
ààf g
,
ààg h
UserId
ââ 
=
ââ 
table
ââ "
.
ââ" #
Column
ââ# )
<
ââ) *
int
ââ* -
>
ââ- .
(
ââ. /
type
ââ/ 3
:
ââ3 4
$str
ââ5 :
,
ââ: ;
nullable
ââ< D
:
ââD E
false
ââF K
)
ââK L
}
ää 
,
ää 
constraints
ãã 
:
ãã 
table
ãã "
=>
ãã# %
{
åå 
table
çç 
.
çç 

PrimaryKey
çç $
(
çç$ %
$str
çç% 6
,
çç6 7
x
çç8 9
=>
çç: <
x
çç= >
.
çç> ?
AppointmentId
çç? L
)
ççL M
;
ççM N
table
éé 
.
éé 

ForeignKey
éé $
(
éé$ %
name
èè 
:
èè 
$str
èè @
,
èè@ A
column
êê 
:
êê 
x
êê  !
=>
êê" $
x
êê% &
.
êê& '
DoctorId
êê' /
,
êê/ 0
principalTable
ëë &
:
ëë& '
$str
ëë( 1
,
ëë1 2
principalColumn
íí '
:
íí' (
$str
íí) 3
,
íí3 4
onDelete
ìì  
:
ìì  !
ReferentialAction
ìì" 3
.
ìì3 4
Restrict
ìì4 <
)
ìì< =
;
ìì= >
table
îî 
.
îî 

ForeignKey
îî $
(
îî$ %
name
ïï 
:
ïï 
$str
ïï B
,
ïïB C
column
ññ 
:
ññ 
x
ññ  !
=>
ññ" $
x
ññ% &
.
ññ& '
	PatientId
ññ' 0
,
ññ0 1
principalTable
óó &
:
óó& '
$str
óó( 2
,
óó2 3
principalColumn
òò '
:
òò' (
$str
òò) 4
,
òò4 5
onDelete
ôô  
:
ôô  !
ReferentialAction
ôô" 3
.
ôô3 4
Restrict
ôô4 <
)
ôô< =
;
ôô= >
}
öö 
)
öö 
;
öö 
migrationBuilder
úú 
.
úú 
CreateTable
úú (
(
úú( )
name
ùù 
:
ùù 
$str
ùù %
,
ùù% &
columns
ûû 
:
ûû 
table
ûû 
=>
ûû !
new
ûû" %
{
üü 
RecordId
†† 
=
†† 
table
†† $
.
††$ %
Column
††% +
<
††+ ,
int
††, /
>
††/ 0
(
††0 1
type
††1 5
:
††5 6
$str
††7 <
,
††< =
nullable
††> F
:
††F G
false
††H M
)
††M N
.
°° 

Annotation
°° #
(
°°# $
$str
°°$ 8
,
°°8 9
$str
°°: @
)
°°@ A
,
°°A B
AppointmentId
¢¢ !
=
¢¢" #
table
¢¢$ )
.
¢¢) *
Column
¢¢* 0
<
¢¢0 1
int
¢¢1 4
>
¢¢4 5
(
¢¢5 6
type
¢¢6 :
:
¢¢: ;
$str
¢¢< A
,
¢¢A B
nullable
¢¢C K
:
¢¢K L
false
¢¢M R
)
¢¢R S
,
¢¢S T
	PatientId
££ 
=
££ 
table
££  %
.
££% &
Column
££& ,
<
££, -
int
££- 0
>
££0 1
(
££1 2
type
££2 6
:
££6 7
$str
££8 =
,
££= >
nullable
££? G
:
££G H
false
££I N
)
££N O
,
££O P
DoctorId
§§ 
=
§§ 
table
§§ $
.
§§$ %
Column
§§% +
<
§§+ ,
int
§§, /
>
§§/ 0
(
§§0 1
type
§§1 5
:
§§5 6
$str
§§7 <
,
§§< =
nullable
§§> F
:
§§F G
false
§§H M
)
§§M N
,
§§N O
	VisitDate
•• 
=
•• 
table
••  %
.
••% &
Column
••& ,
<
••, -
DateTime
••- 5
>
••5 6
(
••6 7
type
••7 ;
:
••; <
$str
••= H
,
••H I
nullable
••J R
:
••R S
false
••T Y
)
••Y Z
,
••Z [
	Diagnosis
¶¶ 
=
¶¶ 
table
¶¶  %
.
¶¶% &
Column
¶¶& ,
<
¶¶, -
string
¶¶- 3
>
¶¶3 4
(
¶¶4 5
type
¶¶5 9
:
¶¶9 :
$str
¶¶; J
,
¶¶J K
	maxLength
¶¶L U
:
¶¶U V
$num
¶¶W Z
,
¶¶Z [
nullable
¶¶\ d
:
¶¶d e
false
¶¶f k
)
¶¶k l
,
¶¶l m
Prescription
ßß  
=
ßß! "
table
ßß# (
.
ßß( )
Column
ßß) /
<
ßß/ 0
string
ßß0 6
>
ßß6 7
(
ßß7 8
type
ßß8 <
:
ßß< =
$str
ßß> M
,
ßßM N
	maxLength
ßßO X
:
ßßX Y
$num
ßßZ ]
,
ßß] ^
nullable
ßß_ g
:
ßßg h
false
ßßi n
)
ßßn o
,
ßßo p
Notes
®® 
=
®® 
table
®® !
.
®®! "
Column
®®" (
<
®®( )
string
®®) /
>
®®/ 0
(
®®0 1
type
®®1 5
:
®®5 6
$str
®®7 G
,
®®G H
	maxLength
®®I R
:
®®R S
$num
®®T X
,
®®X Y
nullable
®®Z b
:
®®b c
true
®®d h
)
®®h i
,
®®i j
CreatedDate
©© 
=
©©  !
table
©©" '
.
©©' (
Column
©©( .
<
©©. /
DateTimeOffset
©©/ =
>
©©= >
(
©©> ?
type
©©? C
:
©©C D
$str
©©E U
,
©©U V
nullable
©©W _
:
©©_ `
false
©©a f
)
©©f g
}
™™ 
,
™™ 
constraints
´´ 
:
´´ 
table
´´ "
=>
´´# %
{
¨¨ 
table
≠≠ 
.
≠≠ 

PrimaryKey
≠≠ $
(
≠≠$ %
$str
≠≠% 7
,
≠≠7 8
x
≠≠9 :
=>
≠≠; =
x
≠≠> ?
.
≠≠? @
RecordId
≠≠@ H
)
≠≠H I
;
≠≠I J
table
ÆÆ 
.
ÆÆ 

ForeignKey
ÆÆ $
(
ÆÆ$ %
name
ØØ 
:
ØØ 
$str
ØØ K
,
ØØK L
column
∞∞ 
:
∞∞ 
x
∞∞  !
=>
∞∞" $
x
∞∞% &
.
∞∞& '
AppointmentId
∞∞' 4
,
∞∞4 5
principalTable
±± &
:
±±& '
$str
±±( 6
,
±±6 7
principalColumn
≤≤ '
:
≤≤' (
$str
≤≤) 8
,
≤≤8 9
onDelete
≥≥  
:
≥≥  !
ReferentialAction
≥≥" 3
.
≥≥3 4
Restrict
≥≥4 <
)
≥≥< =
;
≥≥= >
table
¥¥ 
.
¥¥ 

ForeignKey
¥¥ $
(
¥¥$ %
name
µµ 
:
µµ 
$str
µµ A
,
µµA B
column
∂∂ 
:
∂∂ 
x
∂∂  !
=>
∂∂" $
x
∂∂% &
.
∂∂& '
DoctorId
∂∂' /
,
∂∂/ 0
principalTable
∑∑ &
:
∑∑& '
$str
∑∑( 1
,
∑∑1 2
principalColumn
∏∏ '
:
∏∏' (
$str
∏∏) 3
,
∏∏3 4
onDelete
ππ  
:
ππ  !
ReferentialAction
ππ" 3
.
ππ3 4
Restrict
ππ4 <
)
ππ< =
;
ππ= >
table
∫∫ 
.
∫∫ 

ForeignKey
∫∫ $
(
∫∫$ %
name
ªª 
:
ªª 
$str
ªª C
,
ªªC D
column
ºº 
:
ºº 
x
ºº  !
=>
ºº" $
x
ºº% &
.
ºº& '
	PatientId
ºº' 0
,
ºº0 1
principalTable
ΩΩ &
:
ΩΩ& '
$str
ΩΩ( 2
,
ΩΩ2 3
principalColumn
ææ '
:
ææ' (
$str
ææ) 4
,
ææ4 5
onDelete
øø  
:
øø  !
ReferentialAction
øø" 3
.
øø3 4
Restrict
øø4 <
)
øø< =
;
øø= >
}
¿¿ 
)
¿¿ 
;
¿¿ 
migrationBuilder
¬¬ 
.
¬¬ 
CreateIndex
¬¬ (
(
¬¬( )
name
√√ 
:
√√ 
$str
√√ 3
,
√√3 4
table
ƒƒ 
:
ƒƒ 
$str
ƒƒ %
,
ƒƒ% &
columns
≈≈ 
:
≈≈ 
new
≈≈ 
[
≈≈ 
]
≈≈ 
{
≈≈  
$str
≈≈! +
,
≈≈+ ,
$str
≈≈- <
}
≈≈= >
)
≈≈> ?
;
≈≈? @
migrationBuilder
«« 
.
«« 
CreateIndex
«« (
(
««( )
name
»» 
:
»» 
$str
»» 4
,
»»4 5
table
…… 
:
…… 
$str
…… %
,
……% &
columns
   
:
   
new
   
[
   
]
   
{
    
$str
  ! ,
,
  , -
$str
  . =
}
  > ?
)
  ? @
;
  @ A
migrationBuilder
ÃÃ 
.
ÃÃ 
CreateIndex
ÃÃ (
(
ÃÃ( )
name
ÕÕ 
:
ÕÕ 
$str
ÕÕ 8
,
ÕÕ8 9
table
ŒŒ 
:
ŒŒ 
$str
ŒŒ %
,
ŒŒ% &
columns
œœ 
:
œœ 
new
œœ 
[
œœ 
]
œœ 
{
œœ  
$str
œœ! +
,
œœ+ ,
$str
œœ- <
,
œœ< =
$str
œœ> H
}
œœI J
,
œœJ K
unique
–– 
:
–– 
true
–– 
,
–– 
filter
—— 
:
—— 
$str
—— 1
)
——1 2
;
——2 3
migrationBuilder
”” 
.
”” 
CreateIndex
”” (
(
””( )
name
‘‘ 
:
‘‘ 
$str
‘‘ 2
,
‘‘2 3
table
’’ 
:
’’ 
$str
’’ )
,
’’) *
column
÷÷ 
:
÷÷ 
$str
÷÷  
)
÷÷  !
;
÷÷! "
migrationBuilder
ÿÿ 
.
ÿÿ 
CreateIndex
ÿÿ (
(
ÿÿ( )
name
ŸŸ 
:
ŸŸ 
$str
ŸŸ %
,
ŸŸ% &
table
⁄⁄ 
:
⁄⁄ 
$str
⁄⁄ $
,
⁄⁄$ %
column
€€ 
:
€€ 
$str
€€ (
,
€€( )
unique
‹‹ 
:
‹‹ 
true
‹‹ 
,
‹‹ 
filter
›› 
:
›› 
$str
›› 6
)
››6 7
;
››7 8
migrationBuilder
ﬂﬂ 
.
ﬂﬂ 
CreateIndex
ﬂﬂ (
(
ﬂﬂ( )
name
‡‡ 
:
‡‡ 
$str
‡‡ 2
,
‡‡2 3
table
·· 
:
·· 
$str
·· )
,
··) *
column
‚‚ 
:
‚‚ 
$str
‚‚  
)
‚‚  !
;
‚‚! "
migrationBuilder
‰‰ 
.
‰‰ 
CreateIndex
‰‰ (
(
‰‰( )
name
ÂÂ 
:
ÂÂ 
$str
ÂÂ 2
,
ÂÂ2 3
table
ÊÊ 
:
ÊÊ 
$str
ÊÊ )
,
ÊÊ) *
column
ÁÁ 
:
ÁÁ 
$str
ÁÁ  
)
ÁÁ  !
;
ÁÁ! "
migrationBuilder
ÈÈ 
.
ÈÈ 
CreateIndex
ÈÈ (
(
ÈÈ( )
name
ÍÍ 
:
ÍÍ 
$str
ÍÍ 1
,
ÍÍ1 2
table
ÎÎ 
:
ÎÎ 
$str
ÎÎ (
,
ÎÎ( )
column
ÏÏ 
:
ÏÏ 
$str
ÏÏ  
)
ÏÏ  !
;
ÏÏ! "
migrationBuilder
ÓÓ 
.
ÓÓ 
CreateIndex
ÓÓ (
(
ÓÓ( )
name
ÔÔ 
:
ÔÔ 
$str
ÔÔ "
,
ÔÔ" #
table
 
:
 
$str
 $
,
$ %
column
ÒÒ 
:
ÒÒ 
$str
ÒÒ )
)
ÒÒ) *
;
ÒÒ* +
migrationBuilder
ÛÛ 
.
ÛÛ 
CreateIndex
ÛÛ (
(
ÛÛ( )
name
ÙÙ 
:
ÙÙ 
$str
ÙÙ %
,
ÙÙ% &
table
ıı 
:
ıı 
$str
ıı $
,
ıı$ %
column
ˆˆ 
:
ˆˆ 
$str
ˆˆ ,
,
ˆˆ, -
unique
˜˜ 
:
˜˜ 
true
˜˜ 
,
˜˜ 
filter
¯¯ 
:
¯¯ 
$str
¯¯ :
)
¯¯: ;
;
¯¯; <
migrationBuilder
˙˙ 
.
˙˙ 
CreateIndex
˙˙ (
(
˙˙( )
name
˚˚ 
:
˚˚ 
$str
˚˚ 2
,
˚˚2 3
table
¸¸ 
:
¸¸ 
$str
¸¸ '
,
¸¸' (
column
˝˝ 
:
˝˝ 
$str
˝˝ "
)
˝˝" #
;
˝˝# $
migrationBuilder
ˇˇ 
.
ˇˇ 
CreateIndex
ˇˇ (
(
ˇˇ( )
name
ÄÄ 
:
ÄÄ 
$str
ÄÄ -
,
ÄÄ- .
table
ÅÅ 
:
ÅÅ 
$str
ÅÅ %
,
ÅÅ% &
columns
ÇÇ 
:
ÇÇ 
new
ÇÇ 
[
ÇÇ 
]
ÇÇ 
{
ÇÇ  
$str
ÇÇ! +
,
ÇÇ+ ,
$str
ÇÇ- 8
}
ÇÇ9 :
)
ÇÇ: ;
;
ÇÇ; <
migrationBuilder
ÑÑ 
.
ÑÑ 
CreateIndex
ÑÑ (
(
ÑÑ( )
name
ÖÖ 
:
ÖÖ 
$str
ÖÖ 0
,
ÖÖ0 1
table
ÜÜ 
:
ÜÜ 
$str
ÜÜ  
,
ÜÜ  !
column
áá 
:
áá 
$str
áá (
)
áá( )
;
áá) *
migrationBuilder
ââ 
.
ââ 
CreateIndex
ââ (
(
ââ( )
name
ää 
:
ää 
$str
ää :
,
ää: ;
table
ãã 
:
ãã 
$str
ãã  
,
ãã  !
columns
åå 
:
åå 
new
åå 
[
åå 
]
åå 
{
åå  
$str
åå! 1
,
åå1 2
$str
åå3 =
}
åå> ?
)
åå? @
;
åå@ A
migrationBuilder
éé 
.
éé 
CreateIndex
éé (
(
éé( )
name
èè 
:
èè 
$str
èè )
,
èè) *
table
êê 
:
êê 
$str
êê  
,
êê  !
column
ëë 
:
ëë 
$str
ëë  
,
ëë  !
unique
íí 
:
íí 
true
íí 
)
íí 
;
íí 
migrationBuilder
îî 
.
îî 
CreateIndex
îî (
(
îî( )
name
ïï 
:
ïï 
$str
ïï 6
,
ïï6 7
table
ññ 
:
ññ 
$str
ññ &
,
ññ& '
column
óó 
:
óó 
$str
óó '
,
óó' (
unique
òò 
:
òò 
true
òò 
)
òò 
;
òò 
migrationBuilder
öö 
.
öö 
CreateIndex
öö (
(
öö( )
name
õõ 
:
õõ 
$str
õõ 1
,
õõ1 2
table
úú 
:
úú 
$str
úú &
,
úú& '
column
ùù 
:
ùù 
$str
ùù "
)
ùù" #
;
ùù# $
migrationBuilder
üü 
.
üü 
CreateIndex
üü (
(
üü( )
name
†† 
:
†† 
$str
†† :
,
††: ;
table
°° 
:
°° 
$str
°° &
,
°°& '
columns
¢¢ 
:
¢¢ 
new
¢¢ 
[
¢¢ 
]
¢¢ 
{
¢¢  
$str
¢¢! ,
,
¢¢, -
$str
¢¢. 9
}
¢¢: ;
)
¢¢; <
;
¢¢< =
migrationBuilder
§§ 
.
§§ 
CreateIndex
§§ (
(
§§( )
name
•• 
:
•• 
$str
•• *
,
••* +
table
¶¶ 
:
¶¶ 
$str
¶¶ !
,
¶¶! "
column
ßß 
:
ßß 
$str
ßß  
,
ßß  !
unique
®® 
:
®® 
true
®® 
,
®® 
filter
©© 
:
©© 
$str
©© .
)
©©. /
;
©©/ 0
}
™™ 	
	protected
≠≠ 
override
≠≠ 
void
≠≠ 
Down
≠≠  $
(
≠≠$ %
MigrationBuilder
≠≠% 5
migrationBuilder
≠≠6 F
)
≠≠F G
{
ÆÆ 	
migrationBuilder
ØØ 
.
ØØ 
	DropTable
ØØ &
(
ØØ& '
name
∞∞ 
:
∞∞ 
$str
∞∞ (
)
∞∞( )
;
∞∞) *
migrationBuilder
≤≤ 
.
≤≤ 
	DropTable
≤≤ &
(
≤≤& '
name
≥≥ 
:
≥≥ 
$str
≥≥ (
)
≥≥( )
;
≥≥) *
migrationBuilder
µµ 
.
µµ 
	DropTable
µµ &
(
µµ& '
name
∂∂ 
:
∂∂ 
$str
∂∂ (
)
∂∂( )
;
∂∂) *
migrationBuilder
∏∏ 
.
∏∏ 
	DropTable
∏∏ &
(
∏∏& '
name
ππ 
:
ππ 
$str
ππ '
)
ππ' (
;
ππ( )
migrationBuilder
ªª 
.
ªª 
	DropTable
ªª &
(
ªª& '
name
ºº 
:
ºº 
$str
ºº (
)
ºº( )
;
ºº) *
migrationBuilder
ææ 
.
ææ 
	DropTable
ææ &
(
ææ& '
name
øø 
:
øø 
$str
øø &
)
øø& '
;
øø' (
migrationBuilder
¡¡ 
.
¡¡ 
	DropTable
¡¡ &
(
¡¡& '
name
¬¬ 
:
¬¬ 
$str
¬¬ $
)
¬¬$ %
;
¬¬% &
migrationBuilder
ƒƒ 
.
ƒƒ 
	DropTable
ƒƒ &
(
ƒƒ& '
name
≈≈ 
:
≈≈ 
$str
≈≈ %
)
≈≈% &
;
≈≈& '
migrationBuilder
«« 
.
«« 
	DropTable
«« &
(
««& '
name
»» 
:
»» 
$str
»» #
)
»»# $
;
»»$ %
migrationBuilder
   
.
   
	DropTable
   &
(
  & '
name
ÀÀ 
:
ÀÀ 
$str
ÀÀ $
)
ÀÀ$ %
;
ÀÀ% &
migrationBuilder
ÕÕ 
.
ÕÕ 
	DropTable
ÕÕ &
(
ÕÕ& '
name
ŒŒ 
:
ŒŒ 
$str
ŒŒ 
)
ŒŒ  
;
ŒŒ  !
migrationBuilder
–– 
.
–– 
	DropTable
–– &
(
––& '
name
—— 
:
—— 
$str
——  
)
——  !
;
——! "
migrationBuilder
”” 
.
”” 
	DropTable
”” &
(
””& '
name
‘‘ 
:
‘‘ 
$str
‘‘ #
)
‘‘# $
;
‘‘$ %
}
’’ 	
}
÷÷ 
}◊◊ 
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
private		 
readonly		 
ILogger		  
<		  !"
GlobalExceptionHandler		! 7
>		7 8
_logger		9 @
;		@ A
public "
GlobalExceptionHandler %
(% &
ILogger& -
<- ."
GlobalExceptionHandler. D
>D E
loggerF L
)L M
{ 	
_logger 
= 
logger 
; 
} 	
public 
async 
	ValueTask 
< 
bool #
># $
TryHandleAsync% 3
(3 4
HttpContext 
httpContext #
,# $
	Exception 
	exception 
,  
CancellationToken 
cancellationToken /
)/ 0
{ 	
_logger 
. 
LogError 
( 
	exception 
, 
$str 7
,7 8
	exception 
. 
Message !
)! "
;" #
var 
( 

statusCode 
, 
message $
)$ %
=& '
	exception( 1
switch2 8
{ $
PatientNotFoundException (
=>) +
(, -
StatusCodes- 8
.8 9
Status404NotFound9 J
,J K
	exceptionL U
.U V
MessageV ]
)] ^
,^ _#
DoctorNotFoundException '
=>( *
(+ ,
StatusCodes, 7
.7 8
Status404NotFound8 I
,I J
	exceptionK T
.T U
MessageU \
)\ ]
,] ^(
AppointmentNotFoundException ,
=>- /
(0 1
StatusCodes1 <
.< =
Status404NotFound= N
,N O
	exceptionP Y
.Y Z
MessageZ a
)a b
,b c)
HealthRecordNotFoundException -
=>. 0
(1 2
StatusCodes2 =
.= >
Status404NotFound> O
,O P
	exceptionQ Z
.Z [
Message[ b
)b c
,c d
_   
=>   
(   
StatusCodes    
.    !(
Status500InternalServerError  ! =
,  = >
$str  ? V
)  V W
}!! 
;!! 
var## 
response## 
=## 
new## 
ErrorResponse## ,
{$$ 

StatusCode%% 
=%% 

statusCode%% '
,%%' (
Message&& 
=&& 
message&& !
,&&! "
	TimeStamp'' 
='' 
DateTime'' $
.''$ %
UtcNow''% +
,''+ ,
Path(( 
=(( 
httpContext(( "
.((" #
Request((# *
.((* +
Path((+ /
})) 
;)) 
httpContext++ 
.++ 
Response++  
.++  !

StatusCode++! +
=++, -

statusCode++. 8
;++8 9
await-- 
httpContext-- 
.-- 
Response-- &
.--& '
WriteAsJsonAsync--' 7
(--7 8
response--8 @
,--@ A
cancellationToken--B S
)--S T
;--T U
return// 
true// 
;// 
}00 	
}11 
}22 ò
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
CreatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
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
< 
Patient 
, 
PatientListDto -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
Patient 
, 
PatientListDto -
>- .
(. /
)/ 0
;0 1
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
< 
Doctor 
, 
DoctorListDto +
>+ ,
(, -
)- .
;. /
	CreateMap 
<  
CreateAppointmentDto *
,* +
Appointment, 7
>7 8
(8 9
)9 :
;: ;
	CreateMap 
<  
UpdateAppointmentDto *
,* +
Appointment, 7
>7 8
(8 9
)9 :
;: ;
	CreateMap 
< 
Appointment !
,! "
AppointmentListDto# 5
>5 6
(6 7
)7 8
;8 9
	CreateMap"" 
<"" !
CreateHealthRecordDto"" +
,""+ ,
HealthRecord""- 9
>""9 :
("": ;
)""; <
;""< =
	CreateMap## 
<## 
UpdateDoctorDto## %
,##% &
HealthRecord##' 3
>##3 4
(##4 5
)##5 6
;##6 7
	CreateMap$$ 
<$$ 
HealthRecord$$ "
,$$" #
HealthRecordListDto$$$ 7
>$$7 8
($$8 9
)$$9 :
;$$: ;
	CreateMap%% 
<%% 
HealthRecord%% "
,%%" #
HealthRecordListDto%%$ 7
>%%7 8
(%%8 9
)%%9 :
.%%: ;
	ForMember%%; D
(%%D E
dest%%E I
=>%%J L
dest%%M Q
.%%Q R

DoctorName%%R \
,%%\ ]
opt&& 
=>&& 
opt&& 
.&& 
MapFrom&& !
(&&! "
src&&" %
=>&&& (
src&&) ,
.&&, -
Doctor&&- 3
.&&3 4
FullName&&4 <
)&&< =
)&&= >
;&&> ?
}** 	
},, 
}-- ¶
fC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Exceptions\UserNotFoundException.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Exceptions #
{ 
public 

class !
UserNotFoundException &
:' (
	Exception) 2
{ 
public !
UserNotFoundException $
($ %
)% &
: 
base 
( 
$str #
)# $
{ 	
}		 	
}

 
public 

class '
InvalidCredentialsException ,
:- .
	Exception/ 8
{ 
public '
InvalidCredentialsException *
(* +
)+ ,
: 
base 
( 
$str .
). /
{ 	
} 	
} 
} ˝
iC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Exceptions\PatientNotFoundException.cs
	namespace 	

HealthCare
 
. 
Api 
. 

Exceptions #
{ 
public 

class $
PatientNotFoundException )
:* +
	Exception, 5
{ 
public $
PatientNotFoundException '
(' (
string( .
message/ 6
)6 7
: 
base 
( 
message 
) 
{ 	
}		 	
}

 
} Ò
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
 ˙
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
public #
DoctorNotFoundException &
(& '
string' -
message. 5
)5 6
: 
base 
( 
message 
) 
{  !
}" #
} 
} ›
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
 ◊

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
roleManagerI T
)T U
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
} ÌW
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
< 
Patient 
> 
Patients &
=>' )
Set* -
<- .
Patient. 5
>5 6
(6 7
)7 8
;8 9
public 
DbSet 
< 
Doctor 
> 
Doctors $
=>% '
Set( +
<+ ,
Doctor, 2
>2 3
(3 4
)4 5
;5 6
public 
DbSet 
< 
Appointment  
>  !
Appointments" .
=>/ 1
Set2 5
<5 6
Appointment6 A
>A B
(B C
)C D
;D E
public 
DbSet 
< 
HealthRecord !
>! "
HealthRecords# 0
=>1 3
Set4 7
<7 8
HealthRecord8 D
>D E
(E F
)F G
;G H
public 
DbSet 
< 
DoctorLeaves !
>! "
DoctorLeaves# /
=>0 2
Set3 6
<6 7
DoctorLeaves7 C
>C D
(D E
)E F
;F G
public 
DbSet 
< 
AvailableSlots #
># $
AvailableSlots% 3
=>4 6
Set7 :
<: ;
AvailableSlots; I
>I J
(J K
)K L
;L M
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
modelBuilder= I
)I J
{ 	
base 
. 
OnModelCreating  
(  !
modelBuilder! -
)- .
;. /
modelBuilder 
. 
Entity 
<  
Appointment  +
>+ ,
(, -
)- .
. 
HasIndex 
( 
a 
=> 
new 
{  
a! "
." #
DoctorId# +
,+ ,
a- .
.. /
ScheduledDate/ <
,< =
a> ?
.? @
TimeSlot@ H
}I J
)J K
. 
IsUnique 
( 
) 
. 
	HasFilter 
( 
$str 4
)4 5
. 
HasDatabaseName  
(  !
$str! C
)C D
;D E
modelBuilder!! 
.!! 
Entity!! 
<!!  
Appointment!!  +
>!!+ ,
(!!, -
)!!- .
."" 
HasIndex"" 
("" 
a"" 
=>"" 
new"" "
{""# $
a""% &
.""& '
DoctorId""' /
,""/ 0
a""1 2
.""2 3
ScheduledDate""3 @
}""A B
)""B C
.## 
HasDatabaseName##  
(##  !
$str##! >
)##> ?
;##? @
modelBuilder%% 
.%% 
Entity%% 
<%%  
Appointment%%  +
>%%+ ,
(%%, -
)%%- .
.&& 
HasIndex&& 
(&& 
a&& 
=>&& 
new&& "
{&&# $
a&&% &
.&&& '
	PatientId&&' 0
,&&0 1
a&&2 3
.&&3 4
ScheduledDate&&4 A
}&&B C
)&&C D
.'' 
HasDatabaseName''  
(''  !
$str''! ?
)''? @
;''@ A
modelBuilder)) 
.)) 
Entity)) 
<))  
HealthRecord))  ,
>)), -
())- .
))). /
.** 
HasIndex** 
(** 
hr** 
=>**  
new**! $
{**% &
hr**' )
.**) *
	PatientId*** 3
,**3 4
hr**5 7
.**7 8
	VisitDate**8 A
}**B C
)**C D
.++ 
HasDatabaseName++ !
(++! "
$str++" F
)++F G
;++G H
modelBuilder.. 
... 
Entity.. 
<..  
Doctor..  &
>..& '
(..' (
)..( )
.// 
HasIndex// 
(// 
d// 
=>// 
new// "
{//# $
d//% &
.//& '
Specialisation//' 5
,//5 6
d//7 8
.//8 9
IsActive//9 A
}//B C
)//C D
.00 
HasDatabaseName00  
(00  !
$str00! E
)00E F
;00F G
modelBuilder22 
.22 
Entity22 
<22  
DoctorLeaves22  ,
>22, -
(22- .
)22. /
.33 
HasIndex33 
(33 
l33 
=>33 
new33 "
{33# $
l33% &
.33& '
DoctorId33' /
,33/ 0
l331 2
.332 3
	LeaveDate333 <
}33= >
)33> ?
.44 
HasDatabaseName44  
(44  !
$str44! 8
)448 9
;449 :
modelBuilder77 
.77 
Entity77 
<77  
Patient77  '
>77' (
(77( )
)77) *
.88 
HasOne88 
(88 
p88 
=>88 
p88 
.88 
User88 #
)88# $
.99 
WithOne99 
(99 
u99 
=>99 
u99 
.99  
Patient99  '
)99' (
.:: 
HasForeignKey:: 
<:: 
Patient:: &
>::& '
(::' (
p::( )
=>::* ,
p::- .
.::. /
UserId::/ 5
)::5 6
.;; 
OnDelete;; 
(;; 
DeleteBehavior;; (
.;;( )
Cascade;;) 0
);;0 1
;;;1 2
modelBuilder>> 
.>> 
Entity>> 
<>>  
Doctor>>  &
>>>& '
(>>' (
)>>( )
.?? 
HasOne?? 
(?? 
d?? 
=>?? 
d?? 
.?? 
User?? "
)??" #
.@@ 
WithOne@@ 
(@@ 
)@@ 
.AA 
HasForeignKeyAA 
<AA 
DoctorAA $
>AA$ %
(AA% &
dAA& '
=>AA( *
dAA+ ,
.AA, -
UserIdAA- 3
)AA3 4
.BB 
OnDeleteBB 
(BB 
DeleteBehaviorBB '
.BB' (
CascadeBB( /
)BB/ 0
;BB0 1
modelBuilderDD 
.DD 
EntityDD 
<DD  
AppointmentDD  +
>DD+ ,
(DD, -
)DD- .
.EE 
HasOneEE 
(EE 
aEE 
=>EE 
aEE 
.EE 
PatientEE &
)EE& '
.FF 
WithManyFF 
(FF 
pFF 
=>FF 
pFF  
.FF  !
AppointmentsFF! -
)FF- .
.GG 
HasForeignKeyGG 
(GG 
aGG  
=>GG! #
aGG$ %
.GG% &
	PatientIdGG& /
)GG/ 0
.HH 
OnDeleteHH 
(HH 
DeleteBehaviorHH (
.HH( )
RestrictHH) 1
)HH1 2
;HH2 3
modelBuilderJJ 
.JJ 
EntityJJ 
<JJ  
AppointmentJJ  +
>JJ+ ,
(JJ, -
)JJ- .
.KK 
HasOneKK 
(KK 
aKK 
=>KK 
aKK 
.KK 
DoctorKK %
)KK% &
.LL 
WithManyLL 
(LL 
dLL 
=>LL 
dLL  
.LL  !
AppointmentsLL! -
)LL- .
.MM 
HasForeignKeyMM 
(MM 
aMM  
=>MM! #
aMM$ %
.MM% &
DoctorIdMM& .
)MM. /
.NN 
OnDeleteNN 
(NN 
DeleteBehaviorNN (
.NN( )
RestrictNN) 1
)NN1 2
;NN2 3
modelBuilderPP 
.PP 
EntityPP 
<PP  
HealthRecordPP  ,
>PP, -
(PP- .
)PP. /
.QQ 
HasOneQQ 
(QQ 
hrQQ 
=>QQ 
hrQQ  
.QQ  !
AppointmentQQ! ,
)QQ, -
.RR 
WithOneRR 
(RR 
aRR 
=>RR 
aRR 
.RR  
HealthRecordRR  ,
)RR, -
.SS 
HasForeignKeySS 
<SS 
HealthRecordSS +
>SS+ ,
(SS, -
hrSS- /
=>SS0 2
hrSS3 5
.SS5 6
AppointmentIdSS6 C
)SSC D
.TT 
OnDeleteTT 
(TT 
DeleteBehaviorTT (
.TT( )
RestrictTT) 1
)TT1 2
;TT2 3
modelBuilderVV 
.VV 
EntityVV 
<VV  
HealthRecordVV  ,
>VV, -
(VV- .
)VV. /
.WW 
HasOneWW 
(WW 
hrWW 
=>WW 
hrWW  
.WW  !
PatientWW! (
)WW( )
.XX 
WithManyXX 
(XX 
pXX 
=>XX 
pXX  
.XX  !
HealthRecordsXX! .
)XX. /
.YY 
HasForeignKeyYY 
(YY 
hrYY !
=>YY" $
hrYY% '
.YY' (
	PatientIdYY( 1
)YY1 2
.ZZ 
OnDeleteZZ 
(ZZ 
DeleteBehaviorZZ (
.ZZ( )
RestrictZZ) 1
)ZZ1 2
;ZZ2 3
modelBuilder\\ 
.\\ 
Entity\\ 
<\\  
HealthRecord\\  ,
>\\, -
(\\- .
)\\. /
.]] 
HasOne]] 
(]] 
hr]] 
=>]] 
hr]]  
.]]  !
Doctor]]! '
)]]' (
.^^ 
WithMany^^ 
(^^ 
d^^ 
=>^^ 
d^^  
.^^  !
HealthRecords^^! .
)^^. /
.__ 
HasForeignKey__ 
(__ 
hr__ !
=>__" $
hr__% '
.__' (
DoctorId__( 0
)__0 1
.`` 
OnDelete`` 
(`` 
DeleteBehavior`` (
.``( )
Restrict``) 1
)``1 2
;``2 3
}aa 	
}dd 
}ee ˚
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
,| }
IConfiguration	~ å
configuration
ç ö
)
ö õ
{		 	
string

 

adminEmail

 
=

 
configuration

  -
[

- .
$str

. C
]

C D
!

D E
;

E F
string 
adminPassword  
=! "
configuration# 0
[0 1
$str1 I
]I J
!J K
;K L
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
new"" %
InvalidOperationException"" 7
(""7 8
$str""8 Q
+""R S
string""T Z
.""Z [
Join""[ _
(""_ `
$str""` d
,""d e
result""f l
.""l m
Errors""m s
.""s t
Select""t z
(""z {
e""{ |
=>""} 
e
""Ä Å
.
""Å Ç
Description
""Ç ç
)
""ç é
)
""é è
)## 
;## 
}%% 
}&& 
}'' 	
})) 
}** Ö)
cC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\PatientController.cs
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
 
)

 
]

 
[ 
ApiController 
] 
public 

class 
PatientController "
:# $
ControllerBase% 3
{ 
private 
readonly 
IPatientService (
_service) 1
;1 2
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
public 
PatientController  
(  !
IPatientService! 0
service1 8
,8 9 
IHealthRecordService9 M
healthRecordServiceN a
)a b
{ 	
_service 
= 
service 
;  
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ d
)d e
]e f
public 
async 
Task 
< 
IActionResult '
>' (
GetMyProfile) 5
(5 6
)6 7
{ 	
var 
	patientId 
= "
GetPatientIdFromClaims 2
(2 3
)3 4
;4 5
var 
patient 
= 
await 
_service  (
.( )
GetByIdAsync) 5
(5 6
	patientId6 ?
)? @
;@ A
return!! 
Ok!! 
(!! 
patient!! 
)!! 
;!! 
}"" 	
[$$ 	
HttpPut$$	 
($$ 
$str$$ 
)$$ 
]$$ 
[%% 	
	Authorize%%	 
(%% !
AuthenticationSchemes%% (
=%%) *
JwtBearerDefaults%%+ <
.%%< = 
AuthenticationScheme%%= Q
,%%Q R
Roles%%S X
=%%Y Z
$str%%[ d
)%%d e
]%%e f
public&& 
async&& 
Task&& 
<&& 
IActionResult&& '
>&&' (
Update&&) /
(&&/ 0
UpdatePatientDto&&1 A
dto&&B E
)&&E F
{'' 	
if(( 
((( 
!(( 

ModelState(( 
.(( 
IsValid(( #
)((# $
{)) 
return** 
ValidationProblem** (
(**( )

ModelState**) 3
)**3 4
;**4 5
}++ 
var.. 
	patientId.. 
=.. "
GetPatientIdFromClaims.. 2
(..2 3
)..3 4
;..4 5
await// 
_service// 
.// 
UpdateAsync// &
(//& '
	patientId//' 0
,//0 1
dto//2 5
)//5 6
;//6 7
return00 
Ok00 
(00 
new00 
{00 
message00 #
=00$ %
$str00& L
}00M N
)00N O
;00O P
}11 	
[33 	
HttpGet33	 
(33 
$str33 
)33 
]33 
[44 	
	Authorize44	 
(44 !
AuthenticationSchemes44 (
=44) *
JwtBearerDefaults44+ <
.44< = 
AuthenticationScheme44= Q
,44Q R
Roles44S X
=44Y Z
$str44[ d
)44d e
]44e f
public55 
async55 
Task55 
<55 
IActionResult55 '
>55' (
GetMyRecords55) 5
(555 6
)556 7
{66 	
var77 
	patientId77 
=77 "
GetPatientIdFromClaims77 2
(772 3
)773 4
;774 5
var99 
result99 
=99 
await99  
_healthRecordService99 3
.:: $
GetHealthRecordByPatient:: )
(::) *
	patientId::* 3
)::3 4
;::4 5
return<< 
Ok<< 
(<< 
result<< 
)<< 
;<< 
}== 	
private?? 
int?? "
GetPatientIdFromClaims?? *
(??* +
)??+ ,
{@@ 	
varAA 
claimAA 
=AA 
UserAA 
.AA 
	FindFirstAA &
(AA& '
$strAA' 2
)AA2 3
??BB 
throwBB 
newBB %
InvalidOperationExceptionBB 6
(BB6 7
$strBB7 \
)BB\ ]
;BB] ^
returnDD 
intDD 
.DD 
ParseDD 
(DD 
claimDD "
.DD" #
ValueDD# (
)DD( )
;DD) *
}EE 	
}HH 
}II Î7
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
,Q R
RolesS X
=Y Z
$str[ c
)c d
]d e
public 
async 
Task 
< 
IActionResult '
>' (
Add) ,
(, -
[- .
FromBody. 6
]6 7!
CreateHealthRecordDto8 M
dtoN Q
)Q R
{ 	
if 
( 
! 

ModelState 
. 
IsValid #
)# $
return 

BadRequest !
(! "

ModelState" ,
), -
;- .
var 
doctorId 
= !
GetDoctorIdFromClaims 0
(0 1
)1 2
;2 3
await    
_healthRecordService   &
.  & '
AddAsync  ' /
(  / 0
dto  0 3
,  3 4
doctorId  5 =
)  = >
;  > ?
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
[GG 	
HttpGetGG	 
(GG 
$strGG 
)GG 
]GG 
[HH 	
	AuthorizeHH	 
(HH !
AuthenticationSchemesHH (
=HH) *
JwtBearerDefaultsHH+ <
.HH< = 
AuthenticationSchemeHH= Q
)HHQ R
]HHR S
[II 	
	AuthorizeII	 
(II 
RolesII 
=II 
$strII $
)II$ %
]II% &
publicJJ 
asyncJJ 
TaskJJ 
<JJ 
IActionResultJJ '
>JJ' ($
GetHealthRecordByPatientJJ) A
(JJA B
)JJB C
{KK 	
varLL 
	patientIdLL 
=LL "
GetPatientIdFromClaimsLL 2
(LL2 3
)LL3 4
;LL4 5
varMM 
resultMM 
=MM 
awaitMM  
_healthRecordServiceMM 3
.MM3 4$
GetHealthRecordByPatientMM4 L
(MML M
	patientIdMMM V
)MMV W
;MMW X
returnNN 
OkNN 
(NN 
resultNN 
)NN 
;NN 
}OO 	
privateQQ 
intQQ "
GetPatientIdFromClaimsQQ *
(QQ* +
)QQ+ ,
{RR 	
varSS 
claimSS 
=SS 
UserSS 
.SS 
	FindFirstSS &
(SS& '
$strSS' 2
)SS2 3
??TT 
throwTT 
newTT %
InvalidOperationExceptionTT 6
(TT6 7
$strTT7 \
)TT\ ]
;TT] ^
returnVV 
intVV 
.VV 
ParseVV 
(VV 
claimVV "
.VV" #
ValueVV# (
)VV( )
;VV) *
}WW 	
privateYY 
intYY !
GetDoctorIdFromClaimsYY )
(YY) *
)YY* +
{ZZ 	
var[[ 
claim[[ 
=[[ 
User[[ 
.[[ 
	FindFirst[[ &
([[& '
$str[[' 1
)[[1 2
??\\ 
throw\\ 
new\\ %
InvalidOperationException\\ 6
(\\6 7
$str\\7 [
)\\[ \
;\\\ ]
return^^ 
int^^ 
.^^ 
Parse^^ 
(^^ 
claim^^ "
.^^" #
Value^^# (
)^^( )
;^^) *
}__ 	
}bb 
}cc í;
bC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\DoctorController.cs
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
$str 
) 
] 
[ 
ApiController 
] 
public 

class 
DoctorController !
:" #
ControllerBase$ 2
{ 
private 
readonly 
IDoctorService '
_service( 0
;0 1
public 
DoctorController 
(  
IDoctorService  .
service/ 6
)6 7
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ c
)c d
]d e
public 
async 
Task 
< 
IActionResult '
>' (
GetMyProfile) 5
(5 6
)6 7
{ 	
var 
doctorId 
= !
GetDoctorIdFromClaims 0
(0 1
)1 2
;2 3
var 
result 
= 
await 
_service '
.' (
GetMyProfileAsync( 9
(9 :
doctorId: B
)B C
;C D
return 
Ok 
( 
result 
) 
; 
} 	
[!! 	
HttpPut!!	 
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
,""Q R
Roles""S X
=""Y Z
$str""[ c
)""c d
]""d e
public$$ 
async$$ 
Task$$ 
<$$ 
IActionResult$$ '
>$$' (
Update$$) /
($$/ 0
int$$0 3
id$$4 6
,$$6 7
[$$8 9
FromBody$$9 A
]$$A B
UpdateDoctorDto$$C R
dto$$S V
)$$V W
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
;''- .
await)) 
_service)) 
.)) 
UpdateAsync)) &
())& '
id))' )
,))) *
dto))+ .
))). /
;))/ 0
return++ 
Ok++ 
(++ 
new++ 
{++ 
message++ #
=++$ %
$str++& K
}++L M
)++M N
;++N O
},, 	
[// 	
HttpPost//	 
(// 
$str// 
)// 
]// 
[00 	
	Authorize00	 
(00 !
AuthenticationSchemes00 (
=00) *
JwtBearerDefaults00+ <
.00< = 
AuthenticationScheme00= Q
)00Q R
]00R S
[11 	
	Authorize11	 
(11 
Roles11 
=11 
$str11 #
)11# $
]11$ %
public22 
async22 
Task22 
<22 
IActionResult22 '
>22' (
AddDoctorLeaves22) 8
(228 9
[229 :
FromBody22: B
]22B C
List22D H
<22H I
CreateLeaveDto22I W
>22W X
leaves22Y _
)22_ `
{33 	
if44 
(44 
!44 

ModelState44 
.44 
IsValid44 #
)44# $
return55 

BadRequest55 !
(55! "

ModelState55" ,
)55, -
;55- .
var77 
doctorId77 
=77 !
GetDoctorIdFromClaims77 0
(770 1
)771 2
;772 3
await88 
_service88 
.88 
CreateLeave88 &
(88& '
doctorId88' /
,88/ 0
leaves881 7
)887 8
;888 9
return99 
Ok99 
(99 
new99 
{99 
message99 #
=99$ %
$str99& A
}99B C
)99C D
;99D E
}:: 	
[== 	
HttpGet==	 
(== 
$str== 
)== 
]== 
[>> 	
AllowAnonymous>>	 
]>> 
public?? 
async?? 
Task?? 
<?? 
IActionResult?? '
>??' (
AvailableDoctors??) 9
(??9 :
[??: ;
	FromQuery??; D
]??D E
string??F L
specialisation??M [
,??[ \
[??\ ]
	FromQuery??] f
]??f g
DateOnly??h p
date??q u
)??u v
{@@ 	
ifAA 
(AA 
stringAA 
.AA 
IsNullOrWhiteSpaceAA )
(AA) *
specialisationAA* 8
)AA8 9
)AA9 :
returnBB 

BadRequestBB !
(BB! "
$strBB" >
)BB> ?
;BB? @
varDD 
resultDD 
=DD 
awaitDD 
_serviceDD '
.DD' (
AvailableDoctorsDD( 8
(DD8 9
specialisationDD9 G
,DDG H
dateDDI M
)DDM N
;DDN O
returnFF 
OkFF 
(FF 
resultFF 
)FF 
;FF 
}GG 	
[II 	
HttpGetII	 
(II 
$strII 
)II 
]II 
[JJ 	
	AuthorizeJJ	 
(JJ !
AuthenticationSchemesJJ (
=JJ) *
JwtBearerDefaultsJJ+ <
.JJ< = 
AuthenticationSchemeJJ= Q
,JJQ R
RolesJJS X
=JJY Z
$strJJ[ c
)JJc d
]JJd e
publicKK 
asyncKK 
TaskKK 
<KK 
IActionResultKK '
>KK' (
GetMyLeavesKK) 4
(KK4 5
)KK5 6
{LL 	
varMM 
doctorIdMM 
=MM !
GetDoctorIdFromClaimsMM 0
(MM0 1
)MM1 2
;MM2 3
varOO 
leavesOO 
=OO 
awaitOO 
_serviceOO '
.OO' ($
GetLeavesByDoctorIdAsyncOO( @
(OO@ A
doctorIdOOA I
)OOI J
;OOJ K
returnQQ 
OkQQ 
(QQ 
leavesQQ 
)QQ 
;QQ 
}RR 	
privateTT 
intTT !
GetDoctorIdFromClaimsTT )
(TT) *
)TT* +
{UU 	
varVV 
claimVV 
=VV 
UserVV 
.VV 
	FindFirstVV &
(VV& '
$strVV' 1
)VV1 2
??WW 
throwWW 
newWW %
InvalidOperationExceptionWW 6
(WW6 7
$strWW7 [
)WW[ \
;WW\ ]
returnYY 
intYY 
.YY 
ParseYY 
(YY 
claimYY "
.YY" #
ValueYY# (
)YY( )
;YY) *
}ZZ 	
}^^ 
}__ ä6
`C:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AuthController.cs
	namespace 	

HealthCare
 
. 
Api 
. 
Controllers $
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
AuthController 
:  !
ControllerBase" 0
{ 
private 
readonly 
IAuthService %
_authService& 2
;2 3
public 
AuthController 
( 
IAuthService *
authService+ 6
)6 7
{ 	
_authService 
= 
authService &
;& '
} 	
[ 	
HttpPost	 
( 
$str $
)$ %
]% &
[ 	
AllowAnonymous	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
RegisterPatient) 8
(8 9
[9 :
FromBody: B
]B C
CreatePatientDtoD T
dtoU X
)X Y
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
;- .
try   
{!! 
await"" 
_authService"" "
.""" # 
RegisterPatientAsync""# 7
(""7 8
dto""8 ;
)""; <
;""< =
return## 
Ok## 
(## 
new## 
{## 
message##  '
=##( )
$str##* K
}##L M
)##M N
;##N O
}$$ 
catch%% 
(%% 
	Exception%% 
ex%% 
)%%  
{&& 
return'' 

BadRequest'' !
(''! "
new''" %
{''& '
message''( /
=''0 1
ex''2 4
.''4 5
Message''5 <
}''= >
)''> ?
;''? @
}(( 
})) 	
[,, 	
HttpPost,,	 
(,, 
$str,, #
),,# $
],,$ %
[-- 	
	Authorize--	 
(-- !
AuthenticationSchemes-- (
=--) *
JwtBearerDefaults--+ <
.--< = 
AuthenticationScheme--= Q
,--Q R
Roles--S X
=--Y Z
$str--[ b
)--b c
]--c d
public.. 
async.. 
Task.. 
<.. 
IActionResult.. '
>..' (
RegisterDoctor..) 7
(..7 8
[..8 9
FromBody..9 A
]..A B
DoctorRegisterDto..C T
dto..U X
)..X Y
{// 	
if00 
(00 
!00 

ModelState00 
.00 
IsValid00 #
)00# $
return11 

BadRequest11 !
(11! "

ModelState11" ,
)11, -
;11- .
try33 
{44 
await55 
_authService55 "
.55" #
RegisterDoctorAsync55# 6
(556 7
dto557 :
)55: ;
;55; <
return66 
Ok66 
(66 
new66 
{66 
message66  '
=66( )
$str66* G
}66H I
)66I J
;66J K
}77 
catch88 
(88 
	Exception88 
ex88 
)88  
{99 
return:: 

BadRequest:: !
(::! "
ex::" $
.::$ %
Message::% ,
)::, -
;::- .
};; 
}<< 	
[@@ 	
AllowAnonymous@@	 
]@@ 
[AA 	
HttpPostAA	 
(AA 
$strAA 
)AA 
]AA 
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
LoginBB) .
(BB. /
[BB/ 0
FromBodyBB0 8
]BB8 9
LoginDtoBB: B
dtoBBC F
)BBF G
{CC 	
varDD 
responseDD 
=DD 
awaitDD  
_authServiceDD! -
.DD- .

LoginAsyncDD. 8
(DD8 9
dtoDD9 <
)DD< =
;DD= >
ifFF 
(FF 
responseFF 
==FF 
nullFF  
)FF  !
{GG 
returnHH 
UnauthorizedHH #
(HH# $
$strHH$ ?
)HH? @
;HH@ A
}II 
returnJJ 
OkJJ 
(JJ 
responseJJ 
)JJ 
;JJ  
}KK 	
[OO 	
HttpGetOO	 
(OO 
$strOO 
)OO 
]OO  
[PP 	
AllowAnonymousPP	 
]PP 
publicQQ 
asyncQQ 
TaskQQ 
<QQ 
IActionResultQQ '
>QQ' (

CheckEmailQQ) 3
(QQ3 4
stringQQ4 :
emailQQ; @
)QQ@ A
{RR 	
varSS 
existsSS 
=SS 
awaitSS 
_authServiceSS +
.SS+ ,
EmailExistsAsyncSS, <
(SS< =
emailSS= B
)SSB C
;SSC D
returnUU 
OkUU 
(UU 
existsUU 
)UU 
;UU 
}VV 	
[YY 	
HttpPostYY	 
(YY 
$strYY #
)YY# $
]YY$ %
[ZZ 	
	AuthorizeZZ	 
(ZZ !
AuthenticationSchemesZZ (
=ZZ) *
JwtBearerDefaultsZZ+ <
.ZZ< = 
AuthenticationSchemeZZ= Q
)ZZQ R
]ZZR S
public[[ 
async[[ 
Task[[ 
<[[ 
IActionResult[[ '
>[[' (
ChangePassword[[) 7
([[7 8
ChangePasswordDto[[8 I
dto[[J M
)[[M N
{\\ 	
if]] 
(]] 
!]] 

ModelState]] 
.]] 
IsValid]] #
)]]# $
return^^ 

BadRequest^^ !
(^^! "

ModelState^^" ,
)^^, -
;^^- .
var`` 
userId`` 
=`` 
User`` 
.`` 
	FindFirst`` '
(``' (

ClaimTypes``( 2
.``2 3
NameIdentifier``3 A
)``A B
?``B C
.``C D
Value``D I
;``I J
awaitbb 
_authServicebb 
.bb 
ChangePasswordAsyncbb 2
(bb2 3
userIdbb3 9
!bb9 :
,bb: ;
dtobb< ?
)bb? @
;bb@ A
returndd 
Okdd 
(dd 
$strdd 5
)dd5 6
;dd6 7
}ee 	
}ff 
}gg Êa
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
,Q R
RolesS X
=Y Z
$str[ d
)d e
]e f
public 
async 
Task 
< 
IActionResult '
>' (
Create) /
(/ 0
[0 1
FromBody1 9
]9 : 
CreateAppointmentDto; O
dtoP S
)S T
{ 	
if 
( 
! 

ModelState 
. 
IsValid #
)# $
return 

BadRequest !
(! "

ModelState" ,
), -
;- .
var 
	patientId 
= "
GetPatientIdFromClaims 2
(2 3
)3 4
;4 5
await!! 
_service!! 
.!! 
AddAsync!! #
(!!# $
dto!!$ '
,!!' (
	patientId!!) 2
)!!2 3
;!!3 4
return## 

StatusCode## 
(## 
$num## !
,##! "
new### &
{$$ 
message%% 
=%% 
$str%% <
}&& 
)&& 
;&& 
}'' 	
[** 	
HttpPut**	 
(** 
$str** 
)** 
]** 
[++ 	
	Authorize++	 
(++ !
AuthenticationSchemes++ (
=++) *
JwtBearerDefaults+++ <
.++< = 
AuthenticationScheme++= Q
,++Q R
Roles++S X
=++Y Z
$str++[ c
)++c d
]++d e
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
,88Q R
Roles88S X
=88Y Z
$str88[ c
)88c d
]88d e
public99 
async99 
Task99 
<99 
IActionResult99 '
>99' (
UpdateStatus99) 5
(995 6
int996 9
id99: <
,99< =
[99> ?
FromBody99? G
]99G H 
UpdateAppointmentDto99I ]
dto99^ a
)99a b
{:: 	
await;; 
_service;; 
.;; 
UpdateStatusAsync;; ,
(;;, -
id;;- /
,;;/ 0
dto;;1 4
);;4 5
;;;5 6
return<< 
	NoContent<< 
(<< 
)<< 
;<< 
}== 	
[@@ 	
HttpGet@@	 
(@@ 
$str@@ "
)@@" #
]@@# $
[AA 	
	AuthorizeAA	 
(AA !
AuthenticationSchemesAA (
=AA) *
JwtBearerDefaultsAA+ <
.AA< = 
AuthenticationSchemeAA= Q
,AAQ R
RolesAAS X
=AAY Z
$strAA[ d
)AAd e
]AAe f
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
GetAvailableSlotsBB) :
(BB: ;
[BB< =
	FromQueryBB= F
]BBF G
intBBH K
doctorIdBBL T
,BBT U
[BBU V
	FromQueryBBV _
]BB_ `
DateOnlyBBa i
dateBBj n
)BBn o
{CC 	
varDD 
slotsDD 
=DD 
awaitDD 
_serviceDD &
.DD& '
AvailableTimeSlotsDD' 9
(DD9 :
dateDD: >
,DD> ?
doctorIdDD@ H
)DDH I
;DDI J
returnEE 
OkEE 
(EE 
slotsEE 
)EE 
;EE 
}FF 	
[II 	
HttpGetII	 
(II 
$strII %
)II% &
]II& '
[JJ 	
	AuthorizeJJ	 
(JJ !
AuthenticationSchemesJJ (
=JJ) *
JwtBearerDefaultsJJ+ <
.JJ< = 
AuthenticationSchemeJJ= Q
,JJQ R
RolesJJS X
=JJY Z
$strJJ[ d
)JJd e
]JJe f
publicKK 
asyncKK 
TaskKK 
<KK 
IActionResultKK '
>KK' (
CheckAvailabilityKK) :
(KK: ;
intKK; >
doctorIdKK? G
,KKG H
DateOnlyKKI Q
dateKKR V
,KKV W
stringKKX ^
timeSlotKK_ g
)KKg h
{LL 	
varMM 
resultMM 
=MM 
awaitMM 
_serviceMM '
.MM' (
IsAvailableMM( 3
(MM3 4
dateMM4 8
,MM8 9
doctorIdMM: B
,MMB C
timeSlotMMD L
)MML M
;MMM N
returnNN 
OkNN 
(NN 
newNN 
{NN 
	availableNN %
=NN& '
resultNN( .
}NN/ 0
)NN0 1
;NN1 2
}OO 	
[QQ 	
HttpGetQQ	 
(QQ 
$strQQ "
)QQ" #
]QQ# $
[RR 	
	AuthorizeRR	 
(RR !
AuthenticationSchemesRR (
=RR) *
JwtBearerDefaultsRR+ <
.RR< = 
AuthenticationSchemeRR= Q
,RRQ R
RolesRRS X
=RRY Z
$strRR[ c
)RRc d
]RRd e
publicSS 
asyncSS 
TaskSS 
<SS 
IActionResultSS '
>SS' (
GetDoctorScheduleSS) :
(SS: ;
DateOnlySS< D
dateSSE I
)SSI J
{TT 	
varUU 
doctorIdUU 
=UU !
GetDoctorIdFromClaimsUU 0
(UU0 1
)UU1 2
;UU2 3
varVV 
resultVV 
=VV 
awaitVV 
_serviceVV '
.VV' (
GetDoctorScheduleVV( 9
(VV9 :
dateVV: >
,VV> ?
doctorIdVV@ H
)VVH I
;VVI J
returnWW 
OkWW 
(WW 
resultWW 
)WW 
;WW 
}XX 	
[[[ 	
HttpGet[[	 
([[ 
$str[[ 
)[[ 
][[  
[\\ 	
	Authorize\\	 
(\\ !
AuthenticationSchemes\\ (
=\\) *
JwtBearerDefaults\\+ <
.\\< = 
AuthenticationScheme\\= Q
,\\Q R
Roles\\S X
=\\Y Z
$str\\[ c
)\\c d
]\\d e
public]] 
async]] 
Task]] 
<]] 
IActionResult]] '
>]]' (
GetMySchedule]]) 6
(]]6 7
[]]7 8
	FromQuery]]8 A
]]]A B
DateOnly]]C K
date]]L P
)]]P Q
{^^ 	
var__ 
	patientId__ 
=__ "
GetPatientIdFromClaims__ 2
(__2 3
)__3 4
;__4 5
var`` 
result`` 
=`` 
await`` 
_service`` '
.``' (
GetPatientSchedule``( :
(``: ;
date``; ?
,``? @
	patientId``A J
)``J K
;``K L
returnaa 
Okaa 
(aa 
resultaa 
)aa 
;aa 
}bb 	
[dd 	
HttpGetdd	 
(dd 
$strdd 
)dd 
]dd 
[ee 	
	Authorizeee	 
(ee !
AuthenticationSchemesee (
=ee) *
JwtBearerDefaultsee+ <
.ee< = 
AuthenticationSchemeee= Q
,eeQ R
RoleseeS X
=eeY Z
$stree[ d
)eed e
]eee f
publicff 
asyncff 
Taskff 
<ff 
IActionResultff '
>ff' (
GetMyAppointmentsff) :
(ff: ;
)ff; <
{gg 	
varhh 
	patientIdhh 
=hh "
GetPatientIdFromClaimshh 2
(hh2 3
)hh3 4
;hh4 5
varii 
resultii 
=ii 
awaitii 
_serviceii '
.ii' (#
GetAppointmentByPatientii( ?
(ii? @
	patientIdii@ I
)iiI J
;iiJ K
returnjj 
Okjj 
(jj 
resultjj 
)jj 
;jj 
}kk 	
[nn 	
HttpGetnn	 
(nn 
$strnn 
)nn 
]nn 
[oo 	
	Authorizeoo	 
(oo !
AuthenticationSchemesoo (
=oo) *
JwtBearerDefaultsoo+ <
.oo< = 
AuthenticationSchemeoo= Q
)ooQ R
]ooR S
[pp 	
	Authorizepp	 
(pp 
Rolespp 
=pp 
$strpp #
)pp# $
]pp$ %
publicqq 
asyncqq 
Taskqq 
<qq 
IActionResultqq '
>qq' (!
GetDoctorAppointmentsqq) >
(qq> ?
)qq? @
{rr 	
varss 
doctorIdss 
=ss !
GetDoctorIdFromClaimsss 0
(ss0 1
)ss1 2
;ss2 3
vartt 
resulttt 
=tt 
awaittt 
_servicett '
.tt' ("
GetAppointmentByDoctortt( >
(tt> ?
doctorIdtt? G
)ttG H
;ttH I
returnuu 
Okuu 
(uu 
resultuu 
)uu 
;uu 
}vv 	
privatexx 
intxx "
GetPatientIdFromClaimsxx *
(xx* +
)xx+ ,
{yy 	
varzz 
claimzz 
=zz 
Userzz 
.zz 
	FindFirstzz &
(zz& '
$strzz' 2
)zz2 3
??{{ 
throw{{ 
new{{ %
InvalidOperationException{{ 6
({{6 7
$str{{7 \
){{\ ]
;{{] ^
return}} 
int}} 
.}} 
Parse}} 
(}} 
claim}} "
.}}" #
Value}}# (
)}}( )
;}}) *
}~~ 	
private
ÄÄ 
int
ÄÄ #
GetDoctorIdFromClaims
ÄÄ )
(
ÄÄ) *
)
ÄÄ* +
{
ÅÅ 	
var
ÇÇ 
claim
ÇÇ 
=
ÇÇ 
User
ÇÇ 
.
ÇÇ 
	FindFirst
ÇÇ &
(
ÇÇ& '
$str
ÇÇ' 1
)
ÇÇ1 2
??
ÉÉ 
throw
ÉÉ 
new
ÉÉ '
InvalidOperationException
ÉÉ 6
(
ÉÉ6 7
$str
ÉÉ7 [
)
ÉÉ[ \
;
ÉÉ\ ]
return
ÖÖ 
int
ÖÖ 
.
ÖÖ 
Parse
ÖÖ 
(
ÖÖ 
claim
ÖÖ "
.
ÖÖ" #
Value
ÖÖ# (
)
ÖÖ( )
;
ÖÖ) *
}
ÜÜ 	
}
àà 
}ââ §O
hC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AdminPatientController.cs
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
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
)  
]  !
public 

class "
AdminPatientController '
:( )
ControllerBase* 8
{ 
private 
readonly 
IPatientService (
_patientService) 8
;8 9
private 
readonly 
IAuthService %
_authService& 2
;2 3
public "
AdminPatientController %
(% &
IPatientService& 5
patientService6 D
,D E
IAuthServiceF R
authServiceS ^
)^ _
{ 	
_patientService 
= 
patientService ,
;, -
_authService 
= 
authService &
;& '
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ b
)b c
]c d
public 
async 
Task 
< 
IActionResult '
>' (
GetPatientById) 7
(7 8
int8 ;
id< >
)> ?
{ 	
if   
(   
id   
<=   
$num   
)   
{!! 
return"" 

BadRequest"" !
(""! "
$str""" 7
)""7 8
;""8 9
}## 
var%% 
result%% 
=%% 
await%% 
_patientService%% .
.%%. /
GetByIdAsync%%/ ;
(%%; <
id%%< >
)%%> ?
;%%? @
return&& 
Ok&& 
(&& 
result&& 
)&& 
;&& 
}'' 	
[** 	
HttpGet**	 
(** 
)** 
]** 
[++ 	
	Authorize++	 
(++ !
AuthenticationSchemes++ (
=++) *
JwtBearerDefaults+++ <
.++< = 
AuthenticationScheme++= Q
,++Q R
Roles++S X
=++Y Z
$str++[ b
)++b c
]++c d
public,, 
async,, 
Task,, 
<,, 
IActionResult,, '
>,,' (
GetAllPatient,,) 6
(,,6 7
[,,7 8
	FromQuery,,8 A
],,A B
PatientFilter,,C P
filter,,Q W
),,W X
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
;//- .
var11 
result11 
=11 
await11 
_patientService11 .
.11. /
GetAllAsync11/ :
(11: ;
filter11; A
)11A B
;11B C
return22 
Ok22 
(22 
result22 
)22 
;22 
}33 	
[66 	
HttpPut66	 
(66 
$str66 
)66 
]66 
[77 	
	Authorize77	 
(77 !
AuthenticationSchemes77 (
=77) *
JwtBearerDefaults77+ <
.77< = 
AuthenticationScheme77= Q
,77Q R
Roles77S X
=77Y Z
$str77[ b
)77b c
]77c d
public88 
async88 
Task88 
<88 
IActionResult88 '
>88' (
UpdatePatient88) 6
(886 7
int887 :
id88; =
,88= >
[88? @
FromBody88@ H
]88H I
UpdatePatientDto88J Z
dto88[ ^
)88^ _
{99 	
if:: 
(:: 
!:: 

ModelState:: 
.:: 
IsValid:: #
)::# $
return;; 

BadRequest;; !
(;;! "

ModelState;;" ,
);;, -
;;;- .
if== 
(== 
id== 
<=== 
$num== 
)== 
return>> 

BadRequest>> !
(>>! "
$str>>" 7
)>>7 8
;>>8 9
await@@ 
_patientService@@ !
.@@! "
UpdateAsync@@" -
(@@- .
id@@. 0
,@@0 1
dto@@2 5
)@@5 6
;@@6 7
returnAA 
OkAA 
(AA 
)AA 
;AA 
}BB 	
[EE 	
	HttpPatchEE	 
(EE 
$strEE  
)EE  !
]EE! "
[FF 	
	AuthorizeFF	 
(FF !
AuthenticationSchemesFF (
=FF) *
JwtBearerDefaultsFF+ <
.FF< = 
AuthenticationSchemeFF= Q
,FFQ R
RolesFFS X
=FFY Z
$strFF[ b
)FFb c
]FFc d
publicGG 
asyncGG 
TaskGG 
<GG 
IActionResultGG '
>GG' (
UpdatePatientStatusGG) <
(GG< =
intGG= @
idGGA C
,GGC D
[GGE F
FromBodyGGF N
]GGN O
boolGGP T
isActiveGGU ]
)GG] ^
{HH 	
ifJJ 
(JJ 
!JJ 

ModelStateJJ 
.JJ 
IsValidJJ #
)JJ# $
{KK 
returnLL 
ValidationProblemLL (
(LL( )

ModelStateLL) 3
)LL3 4
;LL4 5
}MM 
awaitOO 
_patientServiceOO !
.OO! "
UpdateStatusAsyncOO" 3
(OO3 4
idOO4 6
,OO6 7
isActiveOO8 @
)OO@ A
;OOA B
returnPP 
OkPP 
(PP 
)PP 
;PP 
}QQ 	
[TT 	

HttpDeleteTT	 
(TT 
$strTT 
)TT 
]TT 
[UU 	
	AuthorizeUU	 
(UU !
AuthenticationSchemesUU (
=UU) *
JwtBearerDefaultsUU+ <
.UU< = 
AuthenticationSchemeUU= Q
,UUQ R
RolesUUS X
=UUY Z
$strUU[ b
)UUb c
]UUc d
publicVV 
asyncVV 
TaskVV 
<VV 
IActionResultVV '
>VV' (
DeletePatientVV) 6
(VV6 7
intVV7 :
idVV; =
)VV= >
{WW 	
ifYY 
(YY 
idYY 
<=YY 
$numYY 
)YY 
{ZZ 
return[[ 

BadRequest[[ !
([[! "
$str[[" 7
)[[7 8
;[[8 9
}\\ 
await^^ 
_patientService^^ !
.^^! "
DeleteAsync^^" -
(^^- .
id^^. 0
)^^0 1
;^^1 2
return__ 
Ok__ 
(__ 
)__ 
;__ 
}`` 	
[cc 	
HttpPostcc	 
(cc 
$strcc 
)cc 
]cc 
[dd 	
	Authorizedd	 
(dd !
AuthenticationSchemesdd (
=dd) *
JwtBearerDefaultsdd+ <
.dd< = 
AuthenticationSchemedd= Q
,ddQ R
RolesddS X
=ddY Z
$strdd[ b
)ddb c
]ddc d
publicee 
asyncee 
Taskee 
<ee 
IActionResultee '
>ee' (
RegisterPatientee) 8
(ee8 9
[ee9 :
FromBodyee: B
]eeB C
CreatePatientDtoeeD T
dtoeeU X
)eeX Y
{ff 	
ifhh 
(hh 
!hh 

ModelStatehh 
.hh 
IsValidhh #
)hh# $
{ii 
returnjj 
ValidationProblemjj (
(jj( )

ModelStatejj) 3
)jj3 4
;jj4 5
}kk 
awaitmm 
_authServicemm 
.mm  
RegisterPatientAsyncmm 3
(mm3 4
dtomm4 7
)mm7 8
;mm8 9
returnnn 
Oknn 
(nn 
newnn 
{nn 
messagenn #
=nn$ %
$strnn& ?
}nn@ A
)nnA B
;nnB C
}oo 	
[rr 	
HttpGetrr	 
(rr 
$strrr 
)rr 
]rr 
[ss 	
	Authorizess	 
(ss !
AuthenticationSchemesss (
=ss) *
JwtBearerDefaultsss+ <
.ss< = 
AuthenticationSchemess= Q
,ssQ R
RolesssS X
=ssY Z
$strss[ b
)ssb c
]ssc d
publictt 
asynctt 
Tasktt 
<tt 
IActionResulttt '
>tt' (
SearchByNamett) 5
(tt5 6
stringtt6 <
namett= A
)ttA B
{uu 	
ifvv 
(vv 
stringvv 
.vv 
IsNullOrWhiteSpacevv )
(vv) *
namevv* .
)vv. /
)vv/ 0
returnww 

BadRequestww !
(ww! "
$strww" 4
)ww4 5
;ww5 6
varyy 
resultyy 
=yy 
awaityy 
_patientServiceyy .
.yy. /
SearchByNameAsyncyy/ @
(yy@ A
nameyyA E
)yyE F
;yyF G
if{{ 
({{ 
!{{ 
result{{ 
.{{ 
Any{{ 
({{ 
){{ 
){{ 
return|| 
NotFound|| 
(||  
$"||  "
$str||" ?
{||? @
name||@ D
}||D E
$str||E F
"||F G
)||G H
;||H I
return~~ 
Ok~~ 
(~~ 
result~~ 
)~~ 
;~~ 
} 	
}
ÄÄ 
}ÇÇ §
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
$str		 
)		 
]		  
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
$str 
) 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ b
)b c
]c d
public 
async 
Task 
< 
IActionResult '
>' (
DeleteHealthRecord) ;
(; <
int< ?
id@ B
)B C
{ 	
await  
_healthRecordService &
.& '
DeleteAsync' 2
(2 3
id3 5
)5 6
;6 7
return 
Ok 
( 
) 
; 
} 	
} 
} Æ.
gC:\Users\310521\source\repos\UST-Live-01\HealthCare\HealthCare.Api\Controllers\AdminDoctorController.cs
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
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class !
AdminDoctorController &
:' (
ControllerBase) 7
{ 
private 
readonly 
IDoctorService '
_doctorService( 6
;6 7
public !
AdminDoctorController $
($ %
IDoctorService% 3
doctorService4 A
)A B
{ 	
_doctorService 
= 
doctorService *
;* +
} 	
[ 	
HttpGet	 
( 
$str 
)  
]  !
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ b
)b c
]c d
public 
async 
Task 
< 
IActionResult '
>' (
GetDoctorById) 6
(6 7
int7 :
id; =
)= >
{ 	
var   
result   
=   
await   
_doctorService   -
.  - .
GetByIdAsync  . :
(  : ;
id  ; =
)  = >
;  > ?
if!! 
(!! 
result!! 
==!! 
null!! 
)!! 
throw"" 
new"" #
DoctorNotFoundException"" 1
(""1 2
$str""2 D
)""D E
;""E F
return$$ 
Ok$$ 
($$ 
result$$ 
)$$ 
;$$ 
}%% 	
[&& 	
HttpGet&&	 
(&& 
$str&& 
)&& 
]&& 
['' 	
	Authorize''	 
('' !
AuthenticationSchemes'' (
='') *
JwtBearerDefaults''+ <
.''< = 
AuthenticationScheme''= Q
,''Q R
Roles''S X
=''Y Z
$str''[ b
)''b c
]''c d
public(( 
async(( 
Task(( 
<(( 
IActionResult(( '
>((' (
GetAllDoctor(() 5
(((5 6
[((6 7
	FromQuery((7 @
]((@ A
DoctorFilter((B N
filter((O U
)((U V
{)) 	
var** 
result** 
=** 
await** 
_doctorService** -
.**- .
GetAllAsync**. 9
(**9 :
filter**: @
)**@ A
;**A B
return++ 
Ok++ 
(++ 
result++ 
)++ 
;++ 
},, 	
[// 	
HttpPut//	 
(// 
$str// 
)//  
]//  !
[00 	
	Authorize00	 
(00 !
AuthenticationSchemes00 (
=00) *
JwtBearerDefaults00+ <
.00< = 
AuthenticationScheme00= Q
,00Q R
Roles00S X
=00Y Z
$str00[ b
)00b c
]00c d
public11 
async11 
Task11 
<11 
IActionResult11 '
>11' (
UpdateDoctor11) 5
(115 6
int116 9
id11: <
,11< =
[11> ?
FromBody11? G
]11G H
UpdateDoctorDto11I X
dto11Y \
)11\ ]
{22 	
await44 
_doctorService44  
.44  !
UpdateAsync44! ,
(44, -
id44- /
,44/ 0
dto441 4
)444 5
;445 6
return55 
Ok55 
(55 
)55 
;55 
}66 	
[88 	
	HttpPatch88	 
(88 
$str88 (
)88( )
]88) *
[99 	
	Authorize99	 
(99 !
AuthenticationSchemes99 (
=99) *
JwtBearerDefaults99+ <
.99< = 
AuthenticationScheme99= Q
,99Q R
Roles99S X
=99Y Z
$str99[ b
)99b c
]99c d
public:: 
async:: 
Task:: 
<:: 
IActionResult:: '
>::' (
UpdateDoctorStatus::) ;
(::; <
int::< ?
id::@ B
,::B C
[::D E
FromBody::E M
]::M N
bool::O S
isActive::T \
)::\ ]
{;; 	
await<< 
_doctorService<<  
.<<  !
UpdateStatusAsync<<! 2
(<<2 3
id<<3 5
,<<5 6
isActive<<7 ?
)<<? @
;<<@ A
return== 
Ok== 
(== 
)== 
;== 
}>> 	
[@@ 	

HttpDelete@@	 
(@@ 
$str@@ "
)@@" #
]@@# $
[AA 	
	AuthorizeAA	 
(AA !
AuthenticationSchemesAA (
=AA) *
JwtBearerDefaultsAA+ <
.AA< = 
AuthenticationSchemeAA= Q
,AAQ R
RolesAAS X
=AAY Z
$strAA[ b
)AAb c
]AAc d
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
DeleteDoctorBB) 5
(BB5 6
intBB6 9
idBB: <
)BB< =
{CC 	
awaitDD 
_doctorServiceDD  
.DD  !
DeleteAsyncDD! ,
(DD, -
idDD- /
)DD/ 0
;DD0 1
returnEE 
OkEE 
(EE 
)EE 
;EE 
}FF 	
}GG 
}HH ∂!
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
$str 
)  
]  !
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ b
)b c
]c d
public 
async 
Task 
< 
IActionResult '
>' (
GetAllAppointment) :
(: ;
[; <
	FromQuery< E
]E F
AppointmentFilterG X
filterY _
)_ `
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
;- .
var 
result 
= 
await 
_appointmentService 2
.2 3
GetAllAsync3 >
(> ?
filter? E
)E F
;F G
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
$str!! &
)!!& '
]!!' (
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
>$$' (
GetDailyReport$$) 7
($$7 8
[$$8 9
	FromQuery$$9 B
]$$B C
DateOnly$$D L
	startDate$$M V
,$$V W
[$$X Y
	FromQuery$$Y b
]$$b c
DateOnly$$d l
endDate$$m t
)$$t u
{%% 	
var&& 
result&& 
=&& 
await&& 
_appointmentService&& 2
.'' 
GetDailyReport'' 
(''  
	startDate''  )
,'') *
endDate''+ 2
)''2 3
;''3 4
return)) 
Ok)) 
()) 
result)) 
))) 
;)) 
}** 	
[-- 	
HttpGet--	 
(-- 
$str-- 
)-- 
]-- 
[.. 	
	Authorize..	 
(.. !
AuthenticationSchemes.. (
=..) *
JwtBearerDefaults..+ <
...< = 
AuthenticationScheme..= Q
,..Q R
Roles..S X
=..Y Z
$str..[ b
)..b c
]..c d
public// 
async// 
Task// 
<// 
IActionResult// '
>//' (
GetDashboard//) 5
(//5 6
)//6 7
{00 	
var11 
result11 
=11 
await11 
_appointmentService11 2
.112 3
GetSummaryAsync113 B
(11B C
)11C D
;11D E
return22 
Ok22 
(22 
result22 
)22 
;22 
}33 	
}55 
}66 