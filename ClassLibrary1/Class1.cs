using ClassLibrary11;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;

public class T
{


    //ТРИГГЕРЫ для записи в базу
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "RollbackTrigger",  Event = "FOR INSERT")]
    public static void RollbackTrigger()
    {
        Writer.ReCryptoAll();
    }



    //ФУНКЦИИ ДЛЯ РАСШИФРОВКИ 
    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "FillRow2")]
    public static IEnumerable GetTable2(string table)
    {
        return Reader.GetTable(table);
    }


    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "FillRow3")]
    public static IEnumerable GetTable3(string table)
    {
        return Reader.GetTable(table);
    }


    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "FillRow4")]
    public static IEnumerable GetTable4(string table)
    {
        return Reader.GetTable(table);
    }


    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "FillRow5")]
    public static IEnumerable GetTable5(string table)
    {
        return Reader.GetTable(table);
    }


    public static void FillRow2(object table, out SqlString c1, out SqlString c2)
    {
        Row tableResult = (Row)table;

        c1 = tableResult.cell0;
        c2 = tableResult.cell1;
    }

    public static void FillRow3(object table, out SqlString c1, out SqlString c2, out SqlString c3)
    {
        Row tableResult = (Row)table;

        c1 = tableResult.cell0;
        c2 = tableResult.cell1;
        c3 = tableResult.cell2;
    }

    public static void FillRow4(object table, out SqlString c1, out SqlString c2, out SqlString c3, out SqlString c4)
    {
        Row tableResult = (Row)table;

        c1 = tableResult.cell0;
        c2 = tableResult.cell1;
        c3 = tableResult.cell2;
        c4 = tableResult.cell3;
    }

    public static void FillRow5(object table, out SqlString c1, out SqlString c2, out SqlString c3, out SqlString c4, out SqlString c5)
    {
        Row tableResult = (Row)table;

        c1 = tableResult.cell0;
        c2 = tableResult.cell1;
        c3 = tableResult.cell2;
        c4 = tableResult.cell3;
        c5 = tableResult.cell4;
       /* c6 = tableResult.cell5;
        c7 = tableResult.cell6;
        c8 = tableResult.cell7;
        c9 = tableResult.cell8;
        c10 = tableResult.cell9;*/
    }

}

public class Row
{
    public SqlString cell0 = "";
    public SqlString cell1 = "";
    public SqlString cell2 = "";
    public SqlString cell3 = "";
    public SqlString cell4 = "";
    public SqlString cell5 = "";
    public SqlString cell6 = "";
    public SqlString cell7 = "";
    public SqlString cell8 = "";
    public SqlString cell9 = "";
    public SqlString cell10 = "";


    public Row(List<SqlString> fieldsList)
    {

        this.cell0 = fieldsList[0];
        this.cell1 = fieldsList[1];
        this.cell2 = fieldsList[2];
        this.cell3 = fieldsList[3];
        this.cell4 = fieldsList[4];
        this.cell5 = fieldsList[5];
     /*  this.cell6 = fieldsList[6];
        this.cell7 = fieldsList[7];
        this.cell8 = fieldsList[8];
        this.cell9 = fieldsList[9];*/

    }
}