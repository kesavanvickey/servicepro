USE ServicePro

insert into Master.TypeMaster (TypeMasterName,ShortName,Description,IsActive,CompanyMaster_ID)
values
('EmployeeCodeTemplate','EmployeeCode','EmployeeCodeMask','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('EmployeeType','EmployeeType','List of EmployeeType','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('Gender','Gender','List of Gender','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('RollType','RollType','List of RollType','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('CustomerCodeTemplate','CustomerCode','CustomerCodeMask','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('ServiceCodeTemplate','ServiceCode','ServiceCodeMask','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('StatusType','StatusType','List of StatusType','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('PaymentCodeTemplate','PaymentCode','PaymentCodeMask','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('WorkCodeTemplate','WorkCode','WorkCodeMask','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('AddressType','AddressType','List of AddressTypes','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('City','City','List of City','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('State','State','List of State','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('Country','Country','List of Country','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('IDProofType','IDProofType','List of IDProofTypes','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('StorageType','StorageType','List of StorageTypes','1',(select CompanyMaster_ID from Master.CompanyMaster)),
('PaymentType','PaymentType','List of PaymentTypes','1',(select CompanyMaster_ID from Master.CompanyMaster))



insert into Master.TypeDetailMaster (TypeName,Description,IsActive,CompanyMaster_ID,TypeMaster_ID)
values
('EMP','EmployeeCode','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'EmployeeCodeTemplate')),
('Permanent','Employee Types','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'EmployeeType')),
('Temporary','Employee Types','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'EmployeeType')),
('Male','Gender','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'Gender')),
('Female','Gender','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'Gender')),
('Others','Gender','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'Gender')),
('Admin','Roll Types','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'RollType')),
('Employee','Roll Types','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'RollType')),
('CUS','CustomerCode','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'CustomerCodeTemplate')),
('SERVICE','ServiceCode','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'ServiceCodeTemplate')),
('New','New Status','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'StatusType')),
('WorkStarted','WorkStarted Status','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'StatusType')),
('Completed','Completed Status','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'StatusType')),
('PAY','PaymentCode','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'PaymentCodeTemplate')),
('WORK','WorkCode','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'WorkCodeTemplate')),
('Billing','Billing','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'AddressType')),
('Shipping','Shipping','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'AddressType')),
('Chennai','Chennai City','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'City')),
('Madurai','Madurai City','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'City')),
('TamilNadu','TamilNadu State','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'State')),
('Pondichery','Pondichery State','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'State')),
('India','India','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'Country')),
('Aadhaar','Aadhaar Card','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'IDProofType')),
('Pan','Pan Card','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'IDProofType')),
('ProPic','ProPic','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'StorageType')),
('Report','Report','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'StorageType')),
('Document','Document','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'StorageType')),
('Cash','Cash','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'PaymentType')),
('DebitCard','DebitCard','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'PaymentType')),
('CreditCard','CreditCard','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'PaymentType')),
('Paytm','Paytm','1',(select CompanyMaster_ID from Master.CompanyMaster),(select TypeMaster_ID from Master.TypeMaster where TypeMasterName = 'PaymentType'))



