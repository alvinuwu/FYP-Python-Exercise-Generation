DROP TABLE IF EXISTS Topics
DROP TABLE IF EXISTS ExercisePaper
DROP TABLE IF EXISTS AppUser;
DROP TABLE IF EXISTS OEQuestions;
DROP TABLE IF EXISTS SubmitPaper;
DROP TABLE IF EXISTS OEQuestion_Templates;
DROP TABLE IF EXISTS OEQuestionsPaper;
DROP TABLE IF EXISTS OExPaperList;

CREATE TABLE AppUser (
    Id       VARCHAR(50)    NOT NULL,
    Name     VARCHAR (50)   NOT NULL,
    Password VARBINARY (50) NOT NULL,
    Role     VARCHAR(50)    NOT NULL,
    Class    VARCHAR(50)    NULL,
    Email    VARCHAR(100)   NOT NULL,
    PRIMARY KEY CLUSTERED (Id ASC)
);
GO

INSERT INTO AppUser (Id, Name, Password, Role, Class, Email) VALUES 
('deslee', 'Desmond Lee', HASHBYTES('SHA1','password'),'Admin', 'W65G', 'testing@gmail.com'),
('ernlee', 'Ernest Lee', HASHBYTES('SHA1','password'),'Admin', 'W65G', 'testing1@gmail.com'),
('eriqa', 'Eriqayana', HASHBYTES('SHA1','password'),'Admin', 'W65G', 'testing2@gmail.com'),
('alvyu', 'Alvin Yu', HASHBYTES('SHA1','password'),'Student', 'W65G', 'alvindevisiras@gmail.com'),
('stud', 'Student 01', HASHBYTES('SHA1','password'),'Student', 'W65G', '18001767@myrp.edu.sg'),
('lect', 'Lecturer 01', HASHBYTES('SHA1','password'),'Lecturer', 'W65G', 'testing5@gmail.com'),
('stud1', 'Student 02', HASHBYTES('SHA1','password'),'Student', 'W65H', 'testing6@gmail.com'),
('lect1', 'Lecturer 02', HASHBYTES('SHA1','password'),'Lecturer', 'W65H', 'testing7@gmail.com')

GO

CREATE TABLE Topics (
   Id      INT   PRIMARY KEY,
   Name    VARCHAR(30) NOT NULL
);

INSERT INTO Topics (Id, Name) VALUES 
(1, 'Array'),
(2, 'For Loop'),
(3, 'While Loop'),
(4, 'Functions'),
(5, 'Dictionaries'),
(6, 'Lists')


CREATE TABLE ExercisePaper (
    Id        INT IDENTITY (1, 1) PRIMARY KEY,
    Name      VARCHAR(100),
    Topics    VARCHAR(100) NOT NULL,
    TotalQns  INT NOT NULL,
    ExercisePaperId INT NOT NULL,
    Time INT NULL,
    Difficulty INT
);

CREATE TABLE OEQuestions (
    Id            INT IDENTITY (1, 1) PRIMARY KEY,
    Topic         VARCHAR(50) NOT NULL,
    Question      VARCHAR(500) NOT NULL,
    Figure        VARCHAR(500),
    Answer        VARCHAR(500) NOT NULL,
    UseCount      INT,
    Difficulty    INT,
    Marks         INT
);


CREATE TABLE OExPaperList (
    Id            	INT IDENTITY (1, 1) PRIMARY KEY,
    ExercisePaperId     INT NOT NULL,
    OEQuestionId  	INT NOT NULL
    
);

CREATE TABLE SubmitPaper (
    Id            	INT IDENTITY (1, 1) PRIMARY KEY,
    ExercisePaperId     INT NOT NULL,
    Answers       	VARCHAR(8000) NOT NULL,
    Attempts            INT NOT NULL,
    SubmittedBy         VARCHAR(100) NOT NULL,
    Grade               VARCHAR(10) NULL
    
);

CREATE TABLE OEQuestion_Templates (
    Id            INT IDENTITY (1, 1) PRIMARY KEY,
    TopicId       INT NOT NULL,
    Figure        VARCHAR(1000),
    Question      VARCHAR(1000) NOT NULL,
    Answer        VARCHAR(1000) NOT NULL,
    FigureVar     VARCHAR(1000),
    QuestionVar   VARCHAR(1000),
    AnswerVar     VARCHAR(1000)
);

SET IDENTITY_INSERT [dbo].[OEQuestion_Templates] ON
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (2001, 6, N'list1 = [3, 5, 25, 1, 3]', N'what is min(list1)?', N'1', N' list1 = [2445,133,12454,123][/]names = [''Amir'', ''Bear'', ''Charlton'', ''Daman''][nline]', N'Look at the following code, what is max(list1)?[/]Study the code, what is names[2]?', N'12454[/]Charlton')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (3001, 6, N'aLsit = [100, 200, 300, 400, 500][nline][nline]Expected Output:[nline][500, 400, 300, 200, 100]', N'Given a Python list you should be able to display Python list in the following order', N'aList = aList.reverse()[nline]print(aList)', N'aLsit = [500, 400, 300, 200, 100][nline][nline]Expected Output:[nline][100, 200, 300, 400, 500][/]aLsit = [100, 200, 300, 400, 500][nline][nline]Expected Output:[nline][500, 400, 300, 200, 100]', N'Study the following code and display the list shown in the expected output[/]Return the expected output', N'aList = aList.sort()[nline]print(aList)[/]aList = aList.reverse()[nline]print(aList)')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (4001, 4, N'def demo ()[nline]....', N'Create a function that can accept two arguments name and age and print its value', N'def demo(name, age):[nline]    print(name, age)', N'def demo ()[nline]....', N'Use the following code and create a function that accepts two arguments , "firstName" and "lastName". Print its value', N'def demo(firstName, lastName):[nline]    print(firstName, lastName)')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (5001, 5, N'keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]Expected output:[nline]{''Ten'': 10, ''Twenty'': 20, ''Thirty'': 30}', N'Below are the two lists convert it into the dictionary', N'keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]sampleDict = dict(zip(keys, values))[nline]print(sampleDict)', N'keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]Expected output:[nline]{''Ten'': 10, ''Twenty'': 20, ''Thirty'': 30}[/]keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]Expected output:[nline]{''Ten'': 10, ''Twenty'': 20, ''Thirty'': 30}[/]keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]Expected output:[nline]{''Ten'': 10, ''Twenty'': 20, ''Thirty'': 30}', N'Convert the two lists into dictionary[/]Study the two lists and convert the lists into dictionary[/]Analyze the code and convert them into dictionary', N'keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]sampleDict = dict(zip(keys, values))[nline]print(sampleDict)[/]keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]sampleDict = dict(zip(keys, values))[nline]print(sampleDict)[/]keys = [''Ten'', ''Twenty'', ''Thirty''][nline]values = [10, 20, 30][nline][nline]sampleDict = dict(zip(keys, values))[nline]print(sampleDict)')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (6001, 5, N'sampleDict = { [nline]   "class":{ [nline]      "student":{ [nline]         "name":"Mike",[nline]         "marks":{ [nline]            "physics":70,[nline]            "history":80[nline]         }[nline]      }[nline]   }[nline]}[nline][nline]Expected Output:[nline]80', N'Access the value of key ‘history’', N'print(sampleDict[''class''][''student''][''marks''][''history''])', N'sampleDict = { [nline]   "class":{ [nline]      "student":{ [nline]         "name":"Mike",[nline]         "marks":{ [nline]            "physics":70,[nline]            "history":80[nline]         }[nline]      }[nline]   }[nline]}', N'Access the value of key ‘history’ and the expected output should be 80', N'print(sampleDict[''class''][''student''][''marks''][''history''])')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (6002, 5, N'employees = [''Kelly'', ''Emma'', ''John''][nline]defaults = {"designation": ''Application Developer'', "salary": 8000}', N'Initialize dictionary with default values', N'employees = [''Kelly'', ''Emma'', ''John''][nline]defaults = {"designation": ''Application Developer'', "salary": 8000}[nline][nline]resDict = dict.fromkeys(employees, defaults)[nline]print(resDict)[nline][nline]# Individual data[nline]print(resDict["Kelly"])', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (6003, 5, N'sampleDict = {[nline]  "name": "Kelly",[nline]  "age":25,[nline]  "salary": 8000,[nline]  "city": "New york"[nline]  [nline]}[nline][nline]Keys to extract:[nline]keys = ["name", "salary"][nline][nline]Expected output:[nline]{''name'': ''Kelly'', ''salary'': 8000}', N'Create a new dictionary by extracting the following keys from a given dictionary', N'sampleDict = { [nline]  "name": "Kelly",[nline]  "age":25, [nline]  "salary": 8000, [nline]  "city": "New york" }[nline][nline]keys = ["name", "salary"][nline][nline]newDict = {k: sampleDict[k] for k in keys}[nline]print(newDict)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7001, 1, N'Given:[nline]sampleDict = {[nline]  "name": "Kelly",[nline]  "age":25,[nline]  "salary": 8000,[nline]  "city": "New york"[nline]  [nline]}[nline]keysToRemove = ["name", "salary"][nline][nline]Expected output:[nline]{''city'': ''New york'', ''age'': 25}', N'Delete set of keys from Python Dictionary', N'sampleDict = {[nline]  "name": "Kelly",[nline]  "age":25,[nline]  "salary": 8000,[nline]  "city": "New york"[nline]}[nline]keysToRemove = ["name", "salary"][nline][nline]sampleDict = {k: sampleDict[k] for k in sampleDict.keys() - keysToRemove}[nline]print(sampleDict)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7002, 5, N'sampleDict = {[nline]  "name": "Kelly",[nline]  "age":25,[nline]  "salary": 8000,[nline]  "city": "New york"[nline]}[nline][nline]Expected output:[nline]{[nline]  "name": "Kelly",[nline]  "age":25,[nline]  "salary": 8000,[nline]  "location": "New york"[nline]}', N'Rename key city to location in the following dictionary', N'sampleDict = {[nline]  "name": "Kelly",[nline]  "age":25,[nline]  "salary": 8000,[nline]  "city": "New york"[nline]}[nline][nline]sampleDict[''location''] = sampleDict.pop(''city'')[nline]print(sampleDict)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7003, 5, N'sampleDict = {[nline]  ''Physics'': 82,[nline]  ''Math'': 65,[nline]  ''history'': 75[nline]}[nline][nline]Expected output:[nline]Math', N'Get the key corresponding to the minimum value from the following dictionary', N'sampleDict = {[nline]  ''Physics'': 82,[nline]  ''Math'': 65,[nline]  ''history'': 75[nline]}[nline]print(min(sampleDict, key=sampleDict.get))', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7005, 4, N'func1(20, 40, 60)[nline][nline]Expected Output:[nline]20[nline]40[nline]60', N'Write a function func1() such that it can accept a variable length of  argument and print all arguments value', N'def func1(*args):[nline]    for i in args:[nline]        print(i)[nline][nline]func1(20, 40, 60)', N'func1(80, 100)[nline][nline]Expected Output:[nline]80[nline]100', N'Write a function func1() such that it can accept a variable length of  argument and print all arguments value', N'def func1(*args):[nline]    for i in args:[nline]        print(i)[nline][nline]func1(80, 100)')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7006, 4, N'def calculation(a, b):[nline]    # Your Code[nline][nline]res = calculation(40, 10)[nline]print(res)[nline][nline]A res should produce result 50, 30', N'Write a function calculation() such that it can accept two variables and calculate the addition and subtraction of it. And also it must return both addition and subtraction in a single return call', N'def calculation(a, b):[nline]    return a+b, a-b[nline][nline]res = calculation(40, 10)[nline]print(res)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7007, 4, N'Expected Output:[nline]showEmployee("Ben", 9000)[nline]showEmployee("Ben")[nline][nline]Should Produce:[nline][nline]Employee Ben salary is: 9000[nline]Employee Ben salary is: 9000', N'Create a function showEmployee() in such a way that it should accept employee name, and it’s salary and display both, and if the salary is missing in function call it should show it as 9000', N'def showEmployee(name, salary=9000):[nline]    print("Employee", name, "salary is:", salary)[nline][nline]showEmployee("Ben", 9000)[nline]showEmployee("Ben")', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7008, 4, N'- Create an outer function that will accept two parameters a and b[nline]- Create an inner function inside an outer function that will calculate the addition of a and b[nline]- At last, an outer function will add 5 into addition and return it', N'Create an inner function to calculate the addition in the following way', N'def outerFun(a, b):[nline]    square = a**2[nline]    def innerFun(a,b):[nline]        return a+b[nline]    add = innerFun(a, b)[nline]    return add+5[nline][nline]result = outerFun(5, 10)[nline]print(result)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7009, 4, N'Expected Output:[nline]55', N' Write a recursive function to calculate the sum of numbers from 0 to 10', N'def calculateSum(num):[nline]    if num:[nline]        return num + calculateSum(num-1)[nline]    else:[nline]        return 0[nline][nline]res = calculateSum(10)[nline]print(res)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7010, 6, N'list1 = ["M", "na", "i", "Ke"] [nline]list2 = ["y", "me", "s", "lly"][nline][nline]Expected output:[nline][''My'', ''name'', ''is'', ''Kelly'']', N'Concatenate two lists index-wise', N'list1 = ["M", "na", "i", "Ke"] [nline]list2 = ["y", "me", "s", "lly"][nline]list3 = [i + j for i, j in zip(list1, list2)][nline]print(list3)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7011, 1, N'aList = [1, 2, 3, 4, 5, 6, 7][nline][nline]Expected output:[nline][1, 4, 9, 16, 25, 36, 49]', N'Given a Python list. Turn every item of a list into its square', N'aList = [1, 2, 3, 4, 5, 6, 7][nline]aList =  [x * x for x in aList][nline]print(aList)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7012, 6, N'aList = [1, 2, 3, 4, 5, 6, 7][nline][nline]Expected output:[nline][1, 4, 9, 16, 25, 36, 49]', N'Given a Python list. Turn every item of a list into its square', N'aList = [1, 2, 3, 4, 5, 6, 7][nline]aList =  [x * x for x in aList][nline]print(aList)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7013, 6, N'list1 = [10, 20, 30, 40][nline]list2 = [100, 200, 300, 400][nline][nline]Expected output:[nline]10 400[nline]20 300[nline]30 200[nline]40 100', N'Given a two Python list. Iterate both lists simultaneously such that list1 should display item in original order and list2 in reverse order', N'list1 = [10, 20, 30, 40][nline]list2 = [100, 200, 300, 400][nline][nline]for x, y in zip(list1, list2[::-1]):[nline]    print(x, y)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7014, 6, N'list1 = ["Mike", "", "Emma", "Kelly", "", "Brad"][nline][nline]Expected output:[nline]["Mike", "Emma", "Kelly", "Brad"]', N' Remove empty strings from the list of strings', N'list1 = ["Mike", "", "Emma", "Kelly", "", "Brad"][nline]resList = list(filter(None, list1))[nline]print(resList)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7015, 6, N'list1 = [10, 20, [300, 400, [5000, 6000], 500], 30, 40][nline][nline]Expected output:[nline][10, 20, [300, 400, [5000, 6000, 7000], 500], 30, 40]', N'Add item 7000 after 6000 in the following Python List', N'list1 = [10, 20, [300, 400, [5000, 6000], 500], 30, 40][nline]list1[2][2].append(7000)[nline]print(list1)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7016, 3, N'Expected output:[nline]0[nline]1[nline]2[nline]3[nline]4[nline]5[nline]6[nline]7[nline]8[nline]9[nline]10', N'Print First 10 natural numbers using while loop', N'i = 0[nline]while i <= 10:[nline]    print(i)[nline]    i += 1', N'Expected output:[nline]0[nline]1[nline]2[nline]3[nline]4[nline]5', N'Print First 5 natural numbers using while loop', N'i = 0[nline]while i <= 5:[nline]    print(i)[nline]    i += 1')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7017, 2, N'Expected Output:[nline]1 [nline]1 2 [nline]1 2 3 [nline]1 2 3 4 [nline]1 2 3 4 5', N'Print the following pattern using for loop', N'print("Second Number Pattern ")[nline]lastNumber = 6[nline]for row in range(1, lastNumber):[nline]    for column in range(1, row + 1):[nline]        print(column, end='' '')[nline]    print("")', N'Expected Output:[nline]1 [nline]1 2 [nline]1 2 3', N'Study and print expected output using for loop', N'print("Second Number Pattern ")[nline]lastNumber = 3[nline]for row in range(1, lastNumber):[nline]    for column in range(1, row + 1):[nline]        print(column, end='' '')[nline]    print("")')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7018, 2, N'For example num = 2 so the output should be[nline][nline]2[nline]4[nline]6[nline]8[nline]10[nline]12[nline]14[nline]16[nline]18[nline]20', N'Print multiplication table of even number', N'n = 2[nline]for i in range(1, 11, 1):[nline]    product = n*i[nline]    print(product)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7019, 3, N'-', N'Given a number count the total number of digits in a number[nline]For example, the number is 75869, so the output should be 5.', N'num = 75869[nline]count = 0[nline]while num != 0:[nline]    num //= 10[nline]    count+= 1[nline]print("Total digits are: ", count)', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7020, 1, N'list1 = [10, 20, 30, 40, 50][nline][nline]Expected output:[nline]50[nline]40[nline]30[nline]20[nline]10', N'Reverse the following list using for loop', N'list1 = [10, 20, 30, 40, 50][nline]start = len(list1) - 1[nline]stop = -1[nline]step = -1[nline]for i in range(start, stop, step) :[nline]    print(list1[i])', N'', N'', N'')
INSERT INTO [dbo].[OEQuestion_Templates] ([Id], [TopicId], [Figure], [Question], [Answer], [FigureVar], [QuestionVar], [AnswerVar]) VALUES (7021, 2, N'So the Expected output should be:[nline]0[nline]1[nline]2[nline]3[nline]4[nline]Done!', N'Display a message “Done” after successful execution using for loop[nline]For example, the following loop will execute without any error.[nline][nline]for i in range(5):[nline]    print(i)', N'for i in range(5):[nline]    print(i)[nline]else:[nline]    print("Done!")', N'', N'', N'')
SET IDENTITY_INSERT [dbo].[OEQuestion_Templates] OFF

CREATE TABLE OEQuestionsPaper (
    Id            INT IDENTITY (1, 1) PRIMARY KEY,
    Topic         VARCHAR(50) NOT NULL,
    Question      VARCHAR(500) NOT NULL,
    Figure        VARCHAR(500),
    Answer        VARCHAR(500) NOT NULL
);
