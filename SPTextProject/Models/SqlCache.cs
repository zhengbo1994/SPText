using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.Models
{
    public static class SqlCache
    {
        public readonly static string GetSequenceNumberSql = @"DECLARE @QQ VARCHAR(2),@MO VARCHAR(2),@DA VARCHAR(2),@LSH VARCHAR(5);
EXEC MDDB.dbo.MDJD_LSH @QQ = @QQ OUTPUT, @MO = @MO OUTPUT, @DA = @DA OUTPUT, @LSH = @LSH OUTPUT;
SELECT @qq + RIGHT('00' + CAST(@MO as varchar(2)), 2) + RIGHT('00' + CAST(@DA as varchar(2)), 2) + CASE WHEN LEN(@LSH) > 4 THEN @LSH ELSE '-' + RIGHT('0000' + CAST(@LSH AS VARCHAR(4)), 4) END + 'C' 流水號";

        public readonly static string GetOmaFileBYBarcodeSql = "SELECT oma_file FROM [order].[dbo].[hkows_order] WHERE job_no={0}";
    }
}
