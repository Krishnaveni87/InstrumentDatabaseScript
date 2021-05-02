USE [InstrumentDatabase]
GO

/****** Object:  StoredProcedure [dbo].[InsertEquipmentType]    Script Date: 03-05-2021 01:33:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[InsertEquipmentType] 
(  
   @Area INTEGER,  
   @Eq_Type VARCHAR(100),  
   @Tag VARCHAR(100),  
   @P_ID varchar(200),  
   @Eq VARCHAR(200),  
   @Area2 VARCHAR(200),
   @Equipment_Name VARCHAR(200),
   @Requestor VARCHAR(200),
   @Project_Name VARCHAR(200)
)  
AS  
BEGIN   
insert into EquipmentTag(Area,Eq_Type,Tag,P_ID,Eq,Area2,Equipment_Name,Requestor,Project_Name) values( @Area, @Eq_Type, @Tag, @P_ID, @Eq,@Area2,@Equipment_Name,@Requestor,@Project_Name)  
End
GO

