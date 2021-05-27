CREATE TABLE [dbo].[Departments]
(
	[DepartmentId] INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
	[Name] NVARCHAR(50) NOT NULL,
    [MinAmount] MONEY NOT NULL,
	[Delay] INT NOT NULL,
	[MinTerm] INT NOT NULL,
	[InterestRate] INT NOT NULL,
	[AccountType] TINYINT NOT NULL,
	[IsBasic] BIT NOT NULL
)

CREATE TABLE [dbo].[Clients]
(
	[ClientId] INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL,
	[SecondName] NVARCHAR(50) NOT NULL,
	[Surname] NVARCHAR(50) NOT NULL,	
	[Amount] MONEY NOT NULL,
	[BadHistory] BIT NOT NULL
)

CREATE TABLE [dbo].[Accounts]
(
	[AccountId] INT IDENTITY(1,1) NOT NULL,
	[DepartmentId] INT NOT NULL, 
	[OwnerId] INT NOT NULL,
	[OwnerName] NVARCHAR(50) NOT NULL,
    [Amount] MONEY NOT NULL,
	[CreationDate] DATE NOT NULL,
	[CurrentDate] DATE NOT NULL,
	[MinTerm] INT NOT NULL,
	[InterestRate] INT NOT NULL,
	[AccountType] TINYINT NOT NULL,
	[BadHistory] BIT NOT NULL,
	FOREIGN KEY (DepartmentId) REFERENCES Departments (DepartmentId),
	FOREIGN KEY (OwnerId) REFERENCES Clients (ClientId)
)

INSERT INTO Clients ( [FirstName], [SecondName], [Surname], [Amount], [BadHistory]) VALUES (N'Вадим', N'Натанович', N'Кулибяка', 27450, 1)
INSERT INTO Clients ( [FirstName], [SecondName], [Surname], [Amount], [BadHistory]) VALUES (N'Владимир', N'Юльевич', N'Пыпырин', 38850, 0)
INSERT INTO Clients ( [FirstName], [SecondName], [Surname], [Amount], [BadHistory]) VALUES (N'Алексей', N'Александрович', N'Прокопов', 40450, 0)
INSERT INTO Clients ( [FirstName], [SecondName], [Surname], [Amount], [BadHistory]) VALUES (N'Олег', N'Викторович', N'Никишин', 120250, 0)
INSERT INTO Clients ( [FirstName], [SecondName], [Surname], [Amount], [BadHistory]) VALUES (N'Анна', N'Сергеевна', N'Крупская', 117300, 0)
INSERT INTO Clients ( [FirstName], [SecondName], [Surname], [Amount], [BadHistory]) VALUES (N'Станислав', N'Валерьевич', N'Коняев', 301200, 0)
INSERT INTO Clients ( [FirstName], [SecondName], [Surname], [Amount], [BadHistory]) VALUES (N'Валентина', N'Витальевна', N'Чизмар', 178500, 0)

INSERT INTO Departments ( [Name], [MinAmount], [MinTerm], [InterestRate], [Delay], [IsBasic], [AccountType]) VALUES (N'Не выбрано', 0, 0, 0, 0, 1, 0)
INSERT INTO Departments ( [Name], [MinAmount], [MinTerm], [InterestRate], [Delay], [IsBasic], [AccountType]) VALUES (N'Отдел по работе с физическими лицами', 50000, 6, 15, 0, 0, 1)
INSERT INTO Departments ( [Name], [MinAmount], [MinTerm], [InterestRate], [Delay], [IsBasic], [AccountType]) VALUES (N'Отдел по работе с юридическими лицами', 30000, 12, 15, 0, 0, 2)
INSERT INTO Departments ( [Name], [MinAmount], [MinTerm], [InterestRate], [Delay], [IsBasic], [AccountType]) VALUES (N'Отдел по работе с привелигированными клиентами', 100000, 18, 20, 0, 0, 3)

INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (1, 1, N'Вадим Натанович Кулибяка', 15, 50000, 6, '2020-09-05', 1, 1, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (1, 1, N'Вадим Натанович Кулибяка', 15, 78540, 12, '2020-11-04', 1, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (1, 2, N'Владимир Юльевич Пыпырин', 15, 58900, 10, '2020-06-12', 1, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (1, 2, N'Владимир Юльевич Пыпырин', 15, 34040, 10, '2020-10-23', 1, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (2, 3, N'Алексей Александрович Прокопов', 15, 35000, 10, '2019-01-24', 2, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (2, 3, N'Алексей Александрович Прокопов', 15, 55000, 10, '2021-01-13', 2, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (2, 4, N'Олег Викторович Никишин', 10, 124000, 12, '2020-08-07', 2, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (2, 4, N'Олег Викторович Никишин', 10, 37000, 12, '2020-02-14', 2, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (2, 5, N'Анна Сергеевна Крупская', 10, 48159, 12, '2020-07-15', 2, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (3, 6, N'Станислав Валерьевич Коняев', 20, 3012250, 18, '2018-10-12', 3, 0, '2021-05-24')
INSERT INTO Accounts ( [DepartmentId], [OwnerId], [OwnerName], [InterestRate], [Amount], [MinTerm], [CreationDate], [AccountType], [BadHistory], [CurrentDate]) VALUES (3, 7, N'Валентина Витальевна Чизмар', 20, 178500, 18, '2019-11-21', 3, 0, '2021-05-24')


                   
                    
                    