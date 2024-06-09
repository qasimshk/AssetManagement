# Getting Started
1.	Set Ms SQL Server database connection string in appSettings.json
2.	Run EFCore command ( Update-Database ) to create database
3.  Use API throught Swagger
4.  Add asset and sources using swagger interface

# Introduction
Since it is a small API, I used a simple 3-tier architecture divided into three strategic layers: Presentation, Business Logic, and Data Access. 

For validation, I used fluent validation, which is an open-source library commonly used to write validation rules for models. The validation rules are flexible and easy to be used in unit tests, which can be seen in my project. It even allows rules to check the record in the database, by injection entity framework core context class in the constructor.

Both database entity's properties are private and can only be created or updated through static methods provided within the entity. Each entity has its own set of rules and relationships, mentioned in the entity configuration class. Conversion between entities and models is handled by mapping classes which are placed in a separate folder. 

I won't use the CQRS pattern for this kind of API as there is no need to raise any events based on the CRUD process.

