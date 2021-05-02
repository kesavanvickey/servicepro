CREATE DATABASE ServicePro;
GO

USE ServicePro;
GO

--Product Database Starts
--Creating Schema for Handling Master Tables
CREATE SCHEMA "Master";
GO

--MasterControl Table for handling Activation Process
CREATE TABLE [Master].[MasterControl](
[ActivationMaster_Key] VARCHAR(500) NOT NULL,
[InstalledBy] VARCHAR(100) NOT NULL,
[CompanyName] VARCHAR(200) NOT NULL,
[ModifierLineNo] INT NOT NULL IDENTITY(1,1),
[IsActive] BIT NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_DateTime] DATETIME NULL,
CONSTRAINT PK_MasterControl_ActivationMasterKey PRIMARY KEY ([ActivationMaster_Key]),
CONSTRAINT UNIQUE_MasterControl_ModifierLineNo UNIQUE ([ModifierLineNo])
);
GO

--CompanyMaster Table handles Company Details and Admin Login Info
CREATE TABLE [Master].[CompanyMaster](
[CompanyMaster_ID] INT NOT NULL IDENTITY(1,1),
[CompanyName] VARCHAR(200) NOT NULL,
[CompanyType] VARCHAR(50) NULL,
[TinNo] VARCHAR(50) NULL,
[UserName] VARCHAR(20) NOT NULL,
[Password] VARCHAR(MAX) NOT NULL,
[Recovery_Mobile] BIGINT NOT NULL,
[Recovery_Email] VARCHAR(100) NOT NULL,
[Recovery_Question] VARCHAR(200) NOT NULL,
[Recovery_Answer] VARCHAR(200) NOT NULL,
[ActivationMaster_Key] VARCHAR(500) NOT NULL,
[CompanyLogo] VARCHAR(MAX) NULL,
[ReportSignature] VARCHAR(MAX) NULL,
[ReportBottom] VARCHAR(MAX) NULL,
[IsActive] BIT NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_DateTime] DATETIME NULL,
CONSTRAINT PK_CompanyMaster_CompanyMasterID PRIMARY KEY ([CompanyMaster_ID]),
CONSTRAINT UNIQUE_CompanyMaster_ActivationMasterKey UNIQUE ([ActivationMaster_Key]),
CONSTRAINT FK_CompanyMaster_ActivationMasterKey_MasterControl FOREIGN KEY ([ActivationMaster_Key]) REFERENCES Master.MasterControl(ActivationMaster_Key)
);
GO

--TypeMaster for handling all types required in DB
CREATE TABLE [Master].[TypeMaster](
[TypeMaster_ID] INT NOT NULL IDENTITY(1,1),
[TypeMasterName] VARCHAR(100) NOT NULL,
[ShortName] VARCHAR(100) NULL,
[Parent_ID] INT NULL,
[Description] VARCHAR(200) NULL,
[CompanyMaster_ID] INT NOT NULL,
[IsActive] BIT NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_DateTime] DATETIME NULL,
CONSTRAINT PK_TypeMaster_TypeMasterID PRIMARY KEY ([TypeMaster_ID]),
CONSTRAINT UNIQUE_TypeMaster_TypeMasterName UNIQUE ([TypeMasterName]),
CONSTRAINT FK_TypeMaster_ParentID_TypeMaster FOREIGN KEY (Parent_ID) REFERENCES Master.TypeMaster([TypeMaster_ID]),
CONSTRAINT FK_TypeMaster_CompanyMasterID_CompanyMaster FOREIGN KEY ([CompanyMaster_ID]) REFERENCES Master.CompanyMaster([CompanyMaster_ID])
);
GO

--TypeDetailMaster for handling details about TypeMaster
CREATE TABLE [Master].[TypeDetailMaster](
[TypeDetailMaster_ID] INT NOT NULL IDENTITY(1,1),
[TypeMaster_ID] INT NOT NULL,
[TypeName] VARCHAR(100) NOT NULL,
[Description] VARCHAR(200) NULL,
[CompanyMaster_ID] INT NOT NULL,
[IsActive] BIT NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_DateTime] DATETIME NULL,
CONSTRAINT PK_TypeDetailMaster_TypeDetailMasterID PRIMARY KEY ([TypeDetailMaster_ID]),
CONSTRAINT FK_TypeDetailMaster_TypeMasterID_TypeMaster FOREIGN KEY ([TypeMaster_ID]) REFERENCES Master.TypeMaster([TypeMaster_ID]),
CONSTRAINT FK_TypeDetailMaster_CompanyMasterID_CompanyMaster FOREIGN KEY ([CompanyMaster_ID]) REFERENCES Master.CompanyMaster([CompanyMaster_ID])
);
GO

--AdditionalColumnMaster for handling AddCol details
CREATE TABLE [Master].[AdditionalColumnMaster](
[AdditionalColumnMaster_ID] INT NOT NULL IDENTITY(1,1),
[TableName] INT NOT NULL,
[AddtionalColumnName] INT NOT NULL,
[DisplayName] VARCHAR(50) NOT NULL,
[DataType] INT NOT NULL,
[CompanyMaster_ID] INT NOT NULL,
[IsActive] BIT NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_DateTime] DATETIME NULL,
CONSTRAINT PK_AdditionalColumnMaster_AdditionalColumnMasterID PRIMARY KEY ([AdditionalColumnMaster_ID]),
CONSTRAINT FK_AdditionalColumnMaster_TableName_TypeDetailMaster FOREIGN KEY ([TableName]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_AdditionalColumnMaster_AddtionalColumnName_TypeDetailMaster FOREIGN KEY ([AddtionalColumnName]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_AdditionalColumnMaster_DataType_TypeDetailMaster FOREIGN KEY ([DataType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_AdditionalColumnMaster_CompanyMasterID_CompanyMaster FOREIGN KEY ([CompanyMaster_ID]) REFERENCES Master.CompanyMaster([CompanyMaster_ID])
);
GO

--EmployeeMaster for handling Employee details
CREATE TABLE [Master].[EmployeeMaster](
[EmployeeMaster_ID] INT NOT NULL IDENTITY(1,1),
[EmployeeCode] INT NOT NULL,
[EmployeeCodeTemplate] INT NOT NULL,
[EmployeeName] VARCHAR(50) NOT NULL,
[DOB] DATETIME NOT NULL,
[Gender] VARCHAR(20) NOT NULL,
[EmployeeType] INT NOT NULL,
[JointDate] DATETIME NOT NULL,
[CompanyMaster_ID] INT NOT NULL,
[IsActive] BIT NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT UNIQUE_EmployeeMaster_EmployeeCode UNIQUE([EmployeeCode]),
CONSTRAINT PK_EmployeeMaster_EmployeeMasterID PRIMARY KEY ([EmployeeMaster_ID]),
CONSTRAINT FK_EmployeeMaster_EmployeeType_TypeDetailMaster FOREIGN KEY ([EmployeeType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_EmployeeMaster_EmployeeCodeTemplate_TypeDetailMaster FOREIGN KEY ([EmployeeCodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_EmployeeMaster_CompanyMasterID_TypeDetailMaster FOREIGN KEY ([CompanyMaster_ID]) REFERENCES Master.CompanyMaster([CompanyMaster_ID])
);
GO

--ServicePro Common Table are begins
--Creating ServicePro Schema to maintain product stability
CREATE SCHEMA "ServicePro";
GO

--UserMaster to maintain Employee Login Details
CREATE TABLE [ServicePro].[UserMaster](
[UserMaster_ID] INT NOT NULL IDENTITY(1,1),
[EmployeeMaster_ID] INT NOT NULL,
[UserName] VARCHAR(20) NOT NULL,
[Password] VARCHAR(MAX) NOT NULL,
[RollType] INT NOT NULL,
[IsActive] BIT NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_UserMaster_UserMasterID PRIMARY KEY ([UserMaster_ID]),
CONSTRAINT FK_UserMaster_EmployeeMasterID_EmployeeMaster FOREIGN KEY ([EmployeeMaster_ID]) REFERENCES Master.EmployeeMaster([EmployeeMaster_ID]),
CONSTRAINT FK_UserMaster_RollType_TypeDetailMaster FOREIGN KEY ([RollType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT UNIQUE_UserMaster_UserName UNIQUE ([UserName]),
CONSTRAINT UNIQUE_UserMaster_EmployeeMasterID UNIQUE ([EmployeeMaster_ID])
);
GO

--TimeSheet to maintain Employee in and out Time
CREATE TABLE [ServicePro].[TimeSheet](
[TimeSheet_ID] INT NOT NULL IDENTITY(1,1),
[EmployeeMaster_ID] INT NOT NULL,
[TimeSheet_Date] DATETIME NOT NULL,
[CheckInDateTime] DATETIME NOT NULL,
[CheckOutDateTime] DATETIME NULL,
[IsActive] BIT NOT NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_TimeSheet_TimeSheetID PRIMARY KEY ([TimeSheet_ID]),
CONSTRAINT FK_TimeSheet_EmployeeMasterID_EmployeeMaster FOREIGN KEY ([EmployeeMaster_ID]) REFERENCES Master.EmployeeMaster([EmployeeMaster_ID])
);
GO

--CustomerMaster to main Customer Details
CREATE TABLE [ServicePro].[CustomerMaster](
[CustomerMaster_ID] INT NOT NULL IDENTITY(1,1),
[CustomerCodeTemplate] INT NOT NULL,
[CustomerCode] INT NOT NULL,
[CustomerName] VARCHAR(50) NOT NULL,
[DOB] DATETIME NULL,
[Gender] VARCHAR(20) NOT NULL,
[EmployeeMaster_ID] INT NOT NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_CustomerMaster_CustomerMasterID PRIMARY KEY ([CustomerMaster_ID]),
CONSTRAINT FK_CustomerMaster_CustomerCodeTemplate_TypeDetailMaster FOREIGN KEY ([CustomerCodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_CustomerMaster_EmployeeMasterID_EmployeeMaster FOREIGN KEY ([EmployeeMaster_ID]) REFERENCES Master.EmployeeMaster([EmployeeMaster_ID]),
CONSTRAINT UNIQUE_CustomerMaster_CustomerCode UNIQUE ([CustomerCode])
);
GO

--IDProofMaster to maintain ID proof details such as PAN,AADHAR..,
CREATE TABLE [ServicePro].[IDProofMaster](
[IDProofMaster_ID] INT NOT NULL IDENTITY(1,1),
[CodeTemplate] INT NULL,
[Ref_ID] INT NOT NULL,
[IDProofType] INT NOT NULL,
[IDProofData] VARCHAR(100),
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_IDProofMaster_IDProofMasterID PRIMARY KEY ([IDProofMaster_ID]),
CONSTRAINT FK_IDProofMaster_CodeTemplate_TypeDetailMaster FOREIGN KEY ([CodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_IDProofMaster_IDProofType_TypeDetailMaster FOREIGN KEY ([IDProofType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID])
);
GO

--AddressMaster to main all address details for Company,Employee and Customers
CREATE TABLE [ServicePro].[AddressMaster](
[AddressMaster_ID] INT NOT NULL IDENTITY(1,1),
[CodeTemplate] INT NULL,
[Ref_ID] INT NOT NULL,
[AddressType] INT NOT NULL,
[Address1] VARCHAR(100) NULL,
[Address2] VARCHAR(100) NULL,
[Address3] VARCHAR(100) NULL,
[City] INT NULL,
[State] INT NULL,
[Country] INT NULL,
[Pincode] BIGINT NULL,
[ContactNo1] BIGINT NOT NULL,
[ContactNo2] BIGINT NULL,
[Email] VARCHAR(100) NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_AddressMaster_AddressMasterID PRIMARY KEY ([AddressMaster_ID]),
CONSTRAINT FK_AddressMaster_CodeTemplate_TypeDetailMaster FOREIGN KEY ([CodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_AddressMaster_AddressType_TypeDetailMaster FOREIGN KEY ([AddressType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_AddressMaster_City_TypeDetailMaster FOREIGN KEY ([City]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_AddressMastter_State_TypeDetailMaster FOREIGN KEY ([State]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_AddressMaster_Country_TypeDetailMaster FOREIGN KEY ([Country]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT UNIQUE_AddressMaster_ContactNo1 UNIQUE ([ContactNo1])
);
GO

--StorageMaster to maintain all PDF,DOC,Image and XL.., 
CREATE TABLE [ServicePro].[StorageMaster](
[StorageMaster_ID] INT NOT NULL IDENTITY(1,1),
[CodeTemplate] INT NULL,
[Ref_ID] INT NOT NULL,
[FileName] VARCHAR(100) NULL,
[StorageType] INT NOT NULL,
[Extension] VARCHAR(20) NULL,
[ContentType] VARCHAR(100) NULL,
[StorageMaster_Data] VARBINARY(MAX) NOT NULL,
[FileSize] BIGINT NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_StorageMaster_StorageMasterID PRIMARY KEY ([StorageMaster_ID]),
CONSTRAINT FK_StorageMaster_CodeTemplate_TypeDetailMaster FOREIGN KEY ([CodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_StorageMaster_StorageType_TypeDetailMaster FOREIGN KEY ([StorageType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID])
);
GO

--ServiceItemMaster to store all service item list
CREATE TABLE [ServicePro].[ServiceItemMaster](
[ServiceItemMaster_ID] INT NOT NULL IDENTITY(1,1),
[ServiceCodeTemplate] INT NOT NULL,
[CustomerMaster_ID] INT NOT NULL,
[EmployeeMaster_ID] INT NOT NULL,
[Brand] VARCHAR(50) NULL,
[Model] VARCHAR(20) NULL,
[ItemOrderDate] DATETIME NOT NULL,
[ItemExpectedDeliverDate] DATETIME NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_ServiceItemMaster_ServiceItemMasterID PRIMARY KEY ([ServiceItemMaster_ID]),
CONSTRAINT FK_ServiceItemMaster_ServiceCodeTemplate_TypeDetailMaster FOREIGN KEY ([ServiceCodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_ServiceItemMaster_CustomerMasterID_CustomerMaster FOREIGN KEY ([CustomerMaster_ID]) REFERENCES ServicePro.CustomerMaster([CustomerMaster_ID]),
CONSTRAINT FK_ServiceItemMaster_EmployeeMasterID_EmployeeMaster FOREIGN KEY ([EmployeeMaster_ID]) REFERENCES Master.EmployeeMaster([EmployeeMaster_ID])
);
GO

--ServiceItemDetail to store service about the item
CREATE TABLE [ServicePro].[ServiceItemDetail](
[ServiceItemDetail_ID] INT NOT NULL IDENTITY(1,1),
[ServiceItemMaster_ID] INT NOT NULL,
[Comments] VARCHAR(200) NOT NULL,
[StatusType] INT NOT NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_ServiceItemDetail_ServiceItemDetailID PRIMARY KEY ([ServiceItemDetail_ID]),
CONSTRAINT FK_ServiceItemDetail_ServiceItemMasterID_ServiceItemMaster FOREIGN KEY ([ServiceItemMaster_ID]) REFERENCES ServicePro.ServiceItemMaster([ServiceItemMaster_ID]),
CONSTRAINT FK_ServiceItemDetail_ServiceItemMasterID_StatusType FOREIGN KEY ([StatusType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID])
);
GO

----StatusMaster used to handle Starus of the service
--CREATE TABLE [ServicePro].[StatusMaster](
--[StatusMaster_ID] INT NOT NULL IDENTITY(1,1),
--[ServiceItemMaster_ID] INT NOT NULL,
--[StatusType] INT NOT NULL,
--[ItemOrderDate] DATETIME NOT NULL,
--[ItemExpectedDeliverDate] DATETIME NULL,
--[IsActive] BIT NOT NULL,
--[Created_UserID] VARCHAR(20) NOT NULL,
--[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
--[Modified_UserID] VARCHAR(20) NULL,
--[Modified_DateTime] DATETIME NULL,
--[AddCol_1] VARCHAR(100) NULL,
--[AddCol_2] VARCHAR(100) NULL,
--[AddCol_3] VARCHAR(100) NULL,
--[AddCol_4] VARCHAR(100) NULL,
--[AddCol_5] VARCHAR(100) NULL,
--CONSTRAINT PK_StatusMaster_StatusMasterID PRIMARY KEY ([StatusMaster_ID]),
--CONSTRAINT UNIQUE_StatusMaster_ServiceItemMasterID UNIQUE ([ServiceItemMaster_ID]),
--CONSTRAINT FK_StatusMaster_ServiceItemMasterID_ServiceItemMaster FOREIGN KEY ([ServiceItemMaster_ID]) REFERENCES ServicePro.ServiceItemMaster([ServiceItemMaster_ID]),
--CONSTRAINT FK_StatusMaster_StatusType_TypeDetailMaster FOREIGN KEY ([StatusType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID])
--);
--GO

--WorkOrderMaster used to handle all work list
CREATE TABLE [ServicePro].[WorkOrderMaster](
[WorkOrderMaster_ID] INT NOT NULL IDENTITY(1,1),
[WorkCodeTemplate] INT NOT NULL,
[ServiceItemDetail_ID] INT NOT NULL,
[EmployeeMaster_ID] INT NOT NULL,
[ServiceStartDate] DATETIME NOT NULL,
[ServiceEndDate] DATETIME NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_WorkOrderMaster_WorkOrderMasterID PRIMARY KEY ([WorkOrderMaster_ID]),
CONSTRAINT FK_WorkOrderMaster_WorkCodeTemplate_TypeDetailMaster FOREIGN KEY ([WorkCodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_WorkOrderMaster_ServiceItemDetailID_ServiceItemDetail FOREIGN KEY ([ServiceItemDetail_ID]) REFERENCES ServicePro.ServiceItemDetail([ServiceItemDetail_ID]),
CONSTRAINT FK_WorkOrderMaster_EmployeeMasterID_EmployeeMaster FOREIGN KEY ([EmployeeMaster_ID]) REFERENCES Master.EmployeeMaster([EmployeeMaster_ID])
);
GO

--PaymentReceivable used to record payable amount to all item detail
CREATE TABLE [ServicePro].[PaymentReceivable](
[PaymentTotal_ID] INT NOT NULL IDENTITY(1,1),
[PaymentCodeTemplate] INT NOT NULL,
[ServiceItemDetail_ID] INT NOT NULL,
[Amount] NUMERIC(18,2) NOT NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_PaymentReceivable_PaymentTotalID PRIMARY KEY ([PaymentTotal_ID]),
CONSTRAINT FK_PaymentReceivable_PaymentCodeTemplate_TypeDetailMaster FOREIGN KEY ([PaymentCodeTemplate]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_PaymentReceivable_ServiceItemDetailID_ServiceItemDetail FOREIGN KEY ([ServiceItemDetail_ID]) REFERENCES ServicePro.ServiceItemDetail([ServiceItemDetail_ID])
);
GO

--PaymentReceived to handle all received payment info
CREATE TABLE [ServicePro].[PaymentReceived](
[PaymentReceived_ID] INT NOT NULL IDENTITY(1,1),
[ServiceItemMaster_ID] INT NOT NULL,
[Amount] NUMERIC(18,2) NOT NULL,
[PaymentType] INT NOT NULL,
[PaymentReferenceNo] VARCHAR(50) NULL,
[PaymentReceivedBy] INT NOT NULL,
[ReceivedDateTime] DATETIME NOT NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_PaymentReceived_PaymentReceivedID PRIMARY KEY ([PaymentReceived_ID]),
CONSTRAINT FK_PaymentReceived_ServiceItemMaster_ID_ServiceItemMaster FOREIGN KEY ([ServiceItemMaster_ID]) REFERENCES ServicePro.ServiceItemMaster([ServiceItemMaster_ID]),
CONSTRAINT FK_PaymentReceived_PaymentType_TypeDetailMaster FOREIGN KEY ([PaymentType]) REFERENCES Master.TypeDetailMaster([TypeDetailMaster_ID]),
CONSTRAINT FK_PaymentReceived_PaymentReceivedBy_EmployeeMaster FOREIGN KEY ([PaymentReceivedBy]) REFERENCES Master.EmployeeMaster([EmployeeMaster_ID])
);
GO

--ItemReceivedHandler is used to maintain all received service item 
CREATE TABLE [ServicePro].[ItemReceivedHandler](
[ItemReceivedHandler_ID] INT NOT NULL IDENTITY(1,1),
[ServiceItemMaster_ID] INT NOT NULL,
[EmployeeMaster_ID] INT NOT NULL,
[CustomerMaster_ID] INT NULL,
[Comments] VARCHAR(200) NULL,
[ReceivedDateTime] DATETIME NOT NULL,
[IsActive] BIT NOT NULL,
[Created_UserID] VARCHAR(20) NOT NULL,
[Created_DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
[Modified_UserID] VARCHAR(20) NULL,
[Modified_DateTime] DATETIME NULL,
[AddCol_1] VARCHAR(100) NULL,
[AddCol_2] VARCHAR(100) NULL,
[AddCol_3] VARCHAR(100) NULL,
[AddCol_4] VARCHAR(100) NULL,
[AddCol_5] VARCHAR(100) NULL,
CONSTRAINT PK_ItemReceivedHandler_ItemReceivedHandlerID PRIMARY KEY ([ItemReceivedHandler_ID]),
CONSTRAINT FK_ItemReceivedHandler_ServiceItemDetailID_ServiceItemDetail FOREIGN KEY ([ServiceItemMaster_ID] ) REFERENCES ServicePro.ServiceItemDetail([ServiceItemDetail_ID]),
CONSTRAINT FK_ItemReceivedHandler_EmployeeMasterID_EmployeeMaster FOREIGN KEY ([EmployeeMaster_ID]) REFERENCES Master.EmployeeMaster([EmployeeMaster_ID]),
CONSTRAINT FK_ItemReceivedHandler_CustomerMasterID_CustomerMaster FOREIGN KEY ([CustomerMaster_ID]) REFERENCES ServicePro.CustomerMaster([CustomerMaster_ID])
);
GO

--Invoice Table is used to store all invoices
CREATE TABLE [ServicePro].[Invoice](
[InvoiceID] INT NOT NULL IDENTITY(1,1),
[CompanyName] VARCHAR(100) NULL,
[CompanyAddress] VARCHAR(500) NULL,
[CompanyContactNo] VARCHAR(100) NULL,
[CustomerId] VARCHAR(100) NULL,
[ServiceItemId] VARCHAR(100) NULL,
[PrintDateTime] VARCHAR(100) NULL,
[CustomerName] VARCHAR(100) NULL,
[ItemName] VARCHAR(100) NULL,
[ItemReceivedDateTime] VARCHAR(100) NULL,
[ItemDeliverDateTime] VARCHAR(100) NULL,
[ItemDetailTotalAmount] VARCHAR(100) NULL,
[PaidAmount] VARCHAR(100) NULL,
[Balance] VARCHAR(100) NULL,
[DeliveredDateTime] VARCHAR(100) NULL,
Created_UserId VARCHAR(100) NOT NULL,
Created_DateTime DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT PK_Invoice_InvoiceId PRIMARY KEY ([InvoiceId])
);

CREATE TABLE ServicePro.InvoiceDetail(
InvoiceDetail_ID INT IDENTITY(1,1),
InvoiceID INT NOT NULL,
Type VARCHAR(50) NOT NULL,
Comments VARCHAR(500) NULL,
StatusType VARCHAR(100) NULL,
Amount VARCHAR(100) NULL,
PaymentType VARCHAR(100) NULL,
RefNo VARCHAR(100) NULL,
ReceivedDateTime VARCHAR(100) NULL,
CONSTRAINT PK_Invoice_InvoiceDetailID PRIMARY KEY ([InvoiceDetail_ID]),
CONSTRAINT FK_InvoiceDetail_InvoiceID_Invoice FOREIGN KEY ([InvoiceID]) REFERENCES ServicePro.Invoice([InvoiceID])
);
