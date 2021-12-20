Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web.UI.WebControls
Imports System.Threading
Imports System.ComponentModel

Public Class clsIV00101
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ITEMNMBR As String = ""
    Dim _ITEMDESC As String = ""
    Dim _NOTEINDX As Double = 0.0
    Dim _ITMSHNAM As String = ""
    Dim _ITEMTYPE As Integer = 0
    Dim _ITMGEDSC As String = ""
    Dim _STNDCOST As Double = 0.0
    Dim _CURRCOST As Double = 0.0
    Dim _ITEMSHWT As Integer = 0
    Dim _DECPLQTY As Integer = 0
    Dim _DECPLCUR As Integer = 0
    Dim _ITMTSHID As String = ""
    Dim _TAXOPTNS As Integer = 0
    Dim _IVIVINDX As Integer = 0
    Dim _IVIVOFIX As Integer = 0
    Dim _IVCOGSIX As Integer = 0
    Dim _IVSLSIDX As Integer = 0
    Dim _IVSLDSIX As Integer = 0
    Dim _IVSLRNIX As Integer = 0
    Dim _IVINUSIX As Integer = 0
    Dim _IVINSVIX As Integer = 0
    Dim _IVDMGIDX As Integer = 0
    Dim _IVVARIDX As Integer = 0
    Dim _DPSHPIDX As Integer = 0
    Dim _PURPVIDX As Integer = 0
    Dim _UPPVIDX As Integer = 0
    Dim _IVRETIDX As Integer = 0
    Dim _ASMVRIDX As Integer = 0

    Dim _ITMCLSCD As String = ""
    Dim _ITMTRKOP As Integer = 0
    Dim _LOTTYPE As String = ""
    Dim _KPERHIST As Integer = 0
    Dim _KPTRXHST As Integer = 0
    Dim _KPCALHST As Integer = 0
    Dim _KPDSTHST As Integer = 0
    Dim _ALWBKORD As Integer = 0
    Dim _VCTNMTHD As Integer = 0
    Dim _UOMSCHDL As String = ""
    Dim _ALTITEM1 As String = ""
    Dim _ALTITEM2 As String = ""
    Dim _USCATVLS_1 As String = ""
    Dim _USCATVLS_2 As String = ""
    Dim _USCATVLS_3 As String = ""
    Dim _USCATVLS_4 As String = ""
    Dim _USCATVLS_5 As String = ""
    Dim _USCATVLS_6 As String = ""
    Dim _MSTRCDTY As Integer = 0
    Dim _MODIFDT As String = ""
    Dim _CREATDDT As String = ""
    Dim _WRNTYDYS As Integer = 0
    Dim _PRCLEVEL As String = ""
    Dim _LOCNCODE As String = ""
    Dim _PINFLIDX As Integer = 0
    Dim _PURMCIDX As Integer = 0
    Dim _IVINFIDX As Integer = 0
    Dim _INVMCIDX As Integer = 0
    Dim _CGSINFLX As Integer = 0
    Dim _CGSMCIDX As Integer = 0
    Dim _ITEMCODE As String = ""
    Dim _TCC As String = ""
    Dim _PriceGroup As String = ""
    Dim _PRICMTHD As Integer = 0
    Dim _PRCHSUOM As String = ""
    Dim _SELNGUOM As String = ""
    Dim _KTACCTSR As Integer = 0
    Dim _LASTGENSN As String = ""
    Dim _ABCCODE As Integer = 0
    Dim _Revalue_Inventory As Integer = 0
    Dim _Tolerance_Percentage As Integer = 0
    Dim _Purchase_Item_Tax_Schedu As String = ""
    Dim _Purchase_Tax_Options As Integer = 0
    Dim _ITMPLNNNGTYP As Integer = 0
    Dim _STTSTCLVLPRCNTG As Integer = 0
    Dim _CNTRYORGN As String = ""
    Dim _INACTIVE As Integer = 0
    Dim _MINSHELF1 As Integer = 0
    Dim _MINSHELF2 As Integer = 0
    Dim _INCLUDEINDP As Integer = 0
    Dim _LOTEXPWARN As Integer = 0
    Dim _LOTEXPWARNDAYS As Integer = 0
    Dim _DEX_ROW_TS As String = ""
    Dim _DEX_ROW_ID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_IV00101 where DEX_ROW_ID = '" & _ID & "'", cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_IV00101 where DEX_ROW_ID = '" & _ID & "'"
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_IV00101")
            If ds.Tables("t_IV00101").Rows.Count > 0 Then
                dr = ds.Tables("t_IV00101").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ITEMNMBR") Is System.DBNull.Value) Then _ITEMNMBR = dr("ITEMNMBR")
        If Not (dr("ITEMDESC") Is System.DBNull.Value) Then _ITEMDESC = dr("ITEMDESC")
        If Not (dr("NOTEINDX") Is System.DBNull.Value) Then _NOTEINDX = dr("NOTEINDX")
        If Not (dr("ITMSHNAM") Is System.DBNull.Value) Then _ITMSHNAM = dr("ITMSHNAM")
        If Not (dr("ITEMTYPE") Is System.DBNull.Value) Then _ITEMTYPE = dr("ITEMTYPE")
        If Not (dr("ITMGEDSC") Is System.DBNull.Value) Then _ITMGEDSC = dr("ITMGEDSC")
        If Not (dr("STNDCOST") Is System.DBNull.Value) Then _STNDCOST = dr("STNDCOST")
        If Not (dr("CURRCOST") Is System.DBNull.Value) Then _CURRCOST = dr("CURRCOST")
        If Not (dr("ITEMSHWT") Is System.DBNull.Value) Then _ITEMSHWT = dr("ITEMSHWT")
        If Not (dr("DECPLQTY") Is System.DBNull.Value) Then _DECPLQTY = dr("DECPLQTY")
        If Not (dr("DECPLCUR") Is System.DBNull.Value) Then _DECPLCUR = dr("DECPLCUR")
        If Not (dr("ITMTSHID") Is System.DBNull.Value) Then _ITMTSHID = dr("ITMTSHID")
        If Not (dr("TAXOPTNS") Is System.DBNull.Value) Then _TAXOPTNS = dr("TAXOPTNS")
        If Not (dr("IVIVINDX") Is System.DBNull.Value) Then _IVIVINDX = dr("IVIVINDX")
        If Not (dr("IVIVOFIX") Is System.DBNull.Value) Then _IVIVOFIX = dr("IVIVOFIX")
        If Not (dr("IVCOGSIX") Is System.DBNull.Value) Then _IVCOGSIX = dr("IVCOGSIX")
        If Not (dr("IVSLSIDX") Is System.DBNull.Value) Then _IVSLSIDX = dr("IVSLSIDX")
        If Not (dr("IVSLDSIX") Is System.DBNull.Value) Then _IVSLDSIX = dr("IVSLDSIX")
        If Not (dr("IVSLRNIX") Is System.DBNull.Value) Then _IVSLRNIX = dr("IVSLRNIX")
        If Not (dr("IVINUSIX") Is System.DBNull.Value) Then _IVINUSIX = dr("IVINUSIX")
        If Not (dr("IVINSVIX") Is System.DBNull.Value) Then _IVINSVIX = dr("IVINSVIX")
        If Not (dr("IVDMGIDX") Is System.DBNull.Value) Then _IVDMGIDX = dr("IVDMGIDX")
        If Not (dr("IVVARIDX") Is System.DBNull.Value) Then _IVVARIDX = dr("IVVARIDX")
        If Not (dr("DPSHPIDX") Is System.DBNull.Value) Then _DPSHPIDX = dr("DPSHPIDX")
        If Not (dr("PURPVIDX") Is System.DBNull.Value) Then _PURPVIDX = dr("PURPVIDX")
        If Not (dr("UPPVIDX") Is System.DBNull.Value) Then _UPPVIDX = dr("UPPVIDX")
        If Not (dr("IVRETIDX") Is System.DBNull.Value) Then _IVRETIDX = dr("IVRETIDX")
        If Not (dr("ASMVRIDX") Is System.DBNull.Value) Then _ASMVRIDX = dr("ASMVRIDX")
        If Not (dr("ITMCLSCD") Is System.DBNull.Value) Then _ITMCLSCD = dr("ITMCLSCD")
        If Not (dr("ITMTRKOP") Is System.DBNull.Value) Then _ITMTRKOP = dr("ITMTRKOP")
        If Not (dr("LOTTYPE") Is System.DBNull.Value) Then _LOTTYPE = dr("LOTTYPE")
        If Not (dr("KPERHIST") Is System.DBNull.Value) Then _KPERHIST = dr("KPERHIST")
        If Not (dr("KPTRXHST") Is System.DBNull.Value) Then _KPTRXHST = dr("KPTRXHST")
        If Not (dr("KPCALHST") Is System.DBNull.Value) Then _KPCALHST = dr("KPCALHST")
        If Not (dr("KPDSTHST") Is System.DBNull.Value) Then _KPDSTHST = dr("KPDSTHST")
        If Not (dr("ALWBKORD") Is System.DBNull.Value) Then _ALWBKORD = dr("ALWBKORD")
        If Not (dr("VCTNMTHD") Is System.DBNull.Value) Then _VCTNMTHD = dr("VCTNMTHD")
        If Not (dr("UOMSCHDL") Is System.DBNull.Value) Then _UOMSCHDL = dr("UOMSCHDL")
        If Not (dr("ALTITEM1") Is System.DBNull.Value) Then _ALTITEM1 = dr("ALTITEM1")
        If Not (dr("ALTITEM2") Is System.DBNull.Value) Then _ALTITEM2 = dr("ALTITEM2")
        If Not (dr("USCATVLS_1") Is System.DBNull.Value) Then _USCATVLS_1 = dr("USCATVLS_1")
        If Not (dr("USCATVLS_2") Is System.DBNull.Value) Then _USCATVLS_2 = dr("USCATVLS_2")
        If Not (dr("USCATVLS_3") Is System.DBNull.Value) Then _USCATVLS_3 = dr("USCATVLS_3")
        If Not (dr("USCATVLS_4") Is System.DBNull.Value) Then _USCATVLS_4 = dr("USCATVLS_4")
        If Not (dr("USCATVLS_5") Is System.DBNull.Value) Then _USCATVLS_5 = dr("USCATVLS_5")
        If Not (dr("USCATVLS_6") Is System.DBNull.Value) Then _USCATVLS_6 = dr("USCATVLS_6")
        If Not (dr("MSTRCDTY") Is System.DBNull.Value) Then _MSTRCDTY = dr("MSTRCDTY")
        If Not (dr("MODIFDT") Is System.DBNull.Value) Then _MODIFDT = dr("MODIFDT")
        If Not (dr("CREATDDT") Is System.DBNull.Value) Then _CREATDDT = dr("CREATDDT")
        If Not (dr("WRNTYDYS") Is System.DBNull.Value) Then _WRNTYDYS = dr("WRNTYDYS")
        If Not (dr("PRCLEVEL") Is System.DBNull.Value) Then _PRCLEVEL = dr("PRCLEVEL")
        If Not (dr("LOCNCODE") Is System.DBNull.Value) Then _LOCNCODE = dr("LOCNCODE")
        If Not (dr("PINFLIDX") Is System.DBNull.Value) Then _PINFLIDX = dr("PINFLIDX")
        If Not (dr("PURMCIDX") Is System.DBNull.Value) Then _PURMCIDX = dr("PURMCIDX")
        If Not (dr("IVINFIDX") Is System.DBNull.Value) Then _IVINFIDX = dr("IVINFIDX")
        If Not (dr("INVMCIDX") Is System.DBNull.Value) Then _INVMCIDX = dr("INVMCIDX")
        If Not (dr("CGSINFLX") Is System.DBNull.Value) Then _CGSINFLX = dr("CGSINFLX")
        If Not (dr("CGSMCIDX") Is System.DBNull.Value) Then _CGSMCIDX = dr("CGSMCIDX")
        If Not (dr("ITEMCODE") Is System.DBNull.Value) Then _ITEMCODE = dr("ITEMCODE")
        If Not (dr("TCC") Is System.DBNull.Value) Then _TCC = dr("TCC")
        If Not (dr("PriceGroup") Is System.DBNull.Value) Then _PriceGroup = dr("PriceGroup")
        If Not (dr("PRICMTHD") Is System.DBNull.Value) Then _PRICMTHD = dr("PRICMTHD")
        If Not (dr("PRCHSUOM") Is System.DBNull.Value) Then _PRCHSUOM = dr("PRCHSUOM")
        If Not (dr("SELNGUOM") Is System.DBNull.Value) Then _SELNGUOM = dr("SELNGUOM")
        If Not (dr("KTACCTSR") Is System.DBNull.Value) Then _KTACCTSR = dr("KTACCTSR")
        If Not (dr("LASTGENSN") Is System.DBNull.Value) Then _LASTGENSN = dr("LASTGENSN")
        If Not (dr("ABCCODE") Is System.DBNull.Value) Then _ABCCODE = dr("ABCCODE")
        If Not (dr("Revalue_Inventory") Is System.DBNull.Value) Then _Revalue_Inventory = dr("Revalue_Inventory")
        If Not (dr("Tolerance_Percentage") Is System.DBNull.Value) Then _Tolerance_Percentage = dr("Tolerance_Percentage")
        If Not (dr("Purchase_Item_Tax_Schedu") Is System.DBNull.Value) Then _Purchase_Item_Tax_Schedu = dr("Purchase_Item_Tax_Schedu")
        If Not (dr("Purchase_Tax_Options") Is System.DBNull.Value) Then _Purchase_Tax_Options = dr("Purchase_Tax_Options")
        If Not (dr("ITMPLNNNGTYP") Is System.DBNull.Value) Then _ITMPLNNNGTYP = dr("ITMPLNNNGTYP")
        If Not (dr("STTSTCLVLPRCNTG") Is System.DBNull.Value) Then _STTSTCLVLPRCNTG = dr("STTSTCLVLPRCNTG")
        If Not (dr("CNTRYORGN") Is System.DBNull.Value) Then _CNTRYORGN = dr("CNTRYORGN")
        If Not (dr("INACTIVE") Is System.DBNull.Value) Then _INACTIVE = dr("INACTIVE")
        If Not (dr("MINSHELF1") Is System.DBNull.Value) Then _MINSHELF1 = dr("MINSHELF1")
        If Not (dr("MINSHELF2") Is System.DBNull.Value) Then _MINSHELF2 = dr("MINSHELF2")
        If Not (dr("INCLUDEINDP") Is System.DBNull.Value) Then _INCLUDEINDP = dr("INCLUDEINDP")
        If Not (dr("LOTEXPWARN") Is System.DBNull.Value) Then _LOTEXPWARN = dr("LOTEXPWARN")
        If Not (dr("LOTEXPWARNDAYS") Is System.DBNull.Value) Then _LOTEXPWARNDAYS = dr("LOTEXPWARNDAYS")
        If Not (dr("DEX_ROW_TS") Is System.DBNull.Value) Then _DEX_ROW_TS = dr("DEX_ROW_TS")
        'If Not (dr("DEX_ROW_ID") Is System.DBNull.Value) Then _DEX_ROW_ID = dr("DEX_ROW_ID")
    End Sub

    Public Function Save() As Boolean
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_IV00101 where DEX_ROW_ID = '" & _ID & "'"
        da = New SqlDataAdapter(cm)
        Dim sqlCMBuild As New SqlCommandBuilder(da)
        ds = New DataSet
        da.Fill(ds, "t_IV00101")
        If ds.Tables("t_IV00101").Rows.Count > 0 Then
            dr = ds.Tables("t_IV00101").Rows(0)
        Else
            da.InsertCommand = New SqlCommand("dbo.sp_IV00101Insert", cn)
            da.InsertCommand.CommandType = CommandType.StoredProcedure
            da.InsertCommand.Parameters.Add("@ITEMNMBR", SqlDbType.Char, 0, "ITEMNMBR")
            da.InsertCommand.Parameters.Add("@ITEMDESC", SqlDbType.Char, 0, "ITEMDESC")
            da.InsertCommand.Parameters.Add("@NOTEINDX", SqlDbType.Decimal, 0, "NOTEINDX")
            da.InsertCommand.Parameters.Add("@ITMSHNAM", SqlDbType.Char, 0, "ITMSHNAM")
            da.InsertCommand.Parameters.Add("@ITEMTYPE", SqlDbType.SmallInt, 0, "ITEMTYPE")
            da.InsertCommand.Parameters.Add("@ITMGEDSC", SqlDbType.Char, 0, "ITMGEDSC")
            da.InsertCommand.Parameters.Add("@STNDCOST", SqlDbType.Decimal, 0, "STNDCOST")
            da.InsertCommand.Parameters.Add("@CURRCOST", SqlDbType.Decimal, 0, "CURRCOST")
            da.InsertCommand.Parameters.Add("@ITEMSHWT", SqlDbType.Int, 0, "ITEMSHWT")
            da.InsertCommand.Parameters.Add("@DECPLQTY", SqlDbType.SmallInt, 0, "DECPLQTY")
            da.InsertCommand.Parameters.Add("@DECPLCUR", SqlDbType.SmallInt, 0, "DECPLCUR")
            da.InsertCommand.Parameters.Add("@ITMTSHID", SqlDbType.Char, 0, "ITMTSHID")
            da.InsertCommand.Parameters.Add("@TAXOPTNS", SqlDbType.SmallInt, 0, "TAXOPTNS")
            da.InsertCommand.Parameters.Add("@IVIVINDX", SqlDbType.Int, 0, "IVIVINDX")
            da.InsertCommand.Parameters.Add("@IVIVOFIX", SqlDbType.Int, 0, "IVIVOFIX")
            da.InsertCommand.Parameters.Add("@IVCOGSIX", SqlDbType.Int, 0, "IVCOGSIX")
            da.InsertCommand.Parameters.Add("@IVSLSIDX", SqlDbType.Int, 0, "IVSLSIDX")
            da.InsertCommand.Parameters.Add("@IVSLDSIX", SqlDbType.Int, 0, "IVSLDSIX")
            da.InsertCommand.Parameters.Add("@IVSLRNIX", SqlDbType.Int, 0, "IVSLRNIX")
            da.InsertCommand.Parameters.Add("@IVINUSIX", SqlDbType.Int, 0, "IVINUSIX")
            da.InsertCommand.Parameters.Add("@IVINSVIX", SqlDbType.Int, 0, "IVINSVIX")
            da.InsertCommand.Parameters.Add("@IVDMGIDX", SqlDbType.Int, 0, "IVDMGIDX")
            da.InsertCommand.Parameters.Add("@IVVARIDX", SqlDbType.Int, 0, "IVVARIDX")
            da.InsertCommand.Parameters.Add("@DPSHPIDX", SqlDbType.Int, 0, "DPSHPIDX")
            da.InsertCommand.Parameters.Add("@PURPVIDX", SqlDbType.Int, 0, "PURPVIDX")
            da.InsertCommand.Parameters.Add("@UPPVIDX", SqlDbType.Int, 0, "UPPVIDX")
            da.InsertCommand.Parameters.Add("@IVRETIDX", SqlDbType.Int, 0, "IVRETIDX")
            da.InsertCommand.Parameters.Add("@ASMVRIDX", SqlDbType.Int, 0, "ASMVRIDX")
            da.InsertCommand.Parameters.Add("@ITMCLSCD", SqlDbType.Char, 0, "ITMCLSCD")
            da.InsertCommand.Parameters.Add("@ITMTRKOP", SqlDbType.SmallInt, 0, "ITMTRKOP")
            da.InsertCommand.Parameters.Add("@LOTTYPE", SqlDbType.Char, 0, "LOTTYPE")
            da.InsertCommand.Parameters.Add("@KPERHIST", SqlDbType.TinyInt, 0, "KPERHIST")
            da.InsertCommand.Parameters.Add("@KPTRXHST", SqlDbType.TinyInt, 0, "KPTRXHST")
            da.InsertCommand.Parameters.Add("@KPCALHST", SqlDbType.TinyInt, 0, "KPCALHST")
            da.InsertCommand.Parameters.Add("@KPDSTHST", SqlDbType.TinyInt, 0, "KPDSTHST")
            da.InsertCommand.Parameters.Add("@ALWBKORD", SqlDbType.TinyInt, 0, "ALWBKORD")
            da.InsertCommand.Parameters.Add("@VCTNMTHD", SqlDbType.SmallInt, 0, "VCTNMTHD")
            da.InsertCommand.Parameters.Add("@UOMSCHDL", SqlDbType.Char, 0, "UOMSCHDL")
            da.InsertCommand.Parameters.Add("@ALTITEM1", SqlDbType.Char, 0, "ALTITEM1")
            da.InsertCommand.Parameters.Add("@ALTITEM2", SqlDbType.Char, 0, "ALTITEM2")
            da.InsertCommand.Parameters.Add("@USCATVLS_1", SqlDbType.Char, 0, "USCATVLS_1")
            da.InsertCommand.Parameters.Add("@USCATVLS_2", SqlDbType.Char, 0, "USCATVLS_2")
            da.InsertCommand.Parameters.Add("@USCATVLS_3", SqlDbType.Char, 0, "USCATVLS_3")
            da.InsertCommand.Parameters.Add("@USCATVLS_4", SqlDbType.Char, 0, "USCATVLS_4")
            da.InsertCommand.Parameters.Add("@USCATVLS_5", SqlDbType.Char, 0, "USCATVLS_5")
            da.InsertCommand.Parameters.Add("@USCATVLS_6", SqlDbType.Char, 0, "USCATVLS_6")
            da.InsertCommand.Parameters.Add("@MSTRCDTY", SqlDbType.SmallInt, 0, "MSTRCDTY")
            da.InsertCommand.Parameters.Add("@MODIFDT", SqlDbType.DateTime, 0, "MODIFDT")
            da.InsertCommand.Parameters.Add("@CREATDDT", SqlDbType.DateTime, 0, "CREATDDT")
            da.InsertCommand.Parameters.Add("@WRNTYDYS", SqlDbType.SmallInt, 0, "WRNTYDYS")
            da.InsertCommand.Parameters.Add("@PRCLEVEL", SqlDbType.Char, 0, "PRCLEVEL")
            da.InsertCommand.Parameters.Add("@LOCNCODE", SqlDbType.Char, 0, "LOCNCODE")
            da.InsertCommand.Parameters.Add("@PINFLIDX", SqlDbType.Int, 0, "PINFLIDX")
            da.InsertCommand.Parameters.Add("@PURMCIDX", SqlDbType.Int, 0, "PURMCIDX")
            da.InsertCommand.Parameters.Add("@IVINFIDX", SqlDbType.Int, 0, "IVINFIDX")
            da.InsertCommand.Parameters.Add("@INVMCIDX", SqlDbType.Int, 0, "INVMCIDX")
            da.InsertCommand.Parameters.Add("@CGSINFLX", SqlDbType.Int, 0, "CGSINFLX")
            da.InsertCommand.Parameters.Add("@CGSMCIDX", SqlDbType.Int, 0, "CGSMCIDX")
            da.InsertCommand.Parameters.Add("@ITEMCODE", SqlDbType.Char, 0, "ITEMCODE")
            da.InsertCommand.Parameters.Add("@TCC", SqlDbType.Char, 0, "TCC")
            da.InsertCommand.Parameters.Add("@PriceGroup", SqlDbType.Char, 0, "PriceGroup")
            da.InsertCommand.Parameters.Add("@PRICMTHD", SqlDbType.SmallInt, 0, "PRICMTHD")
            da.InsertCommand.Parameters.Add("@PRCHSUOM", SqlDbType.Char, 0, "PRCHSUOM")
            da.InsertCommand.Parameters.Add("@SELNGUOM", SqlDbType.Char, 0, "SELNGUOM")
            da.InsertCommand.Parameters.Add("@KTACCTSR", SqlDbType.SmallInt, 0, "KTACCTSR")
            da.InsertCommand.Parameters.Add("@LASTGENSN", SqlDbType.Char, 0, "LASTGENSN")
            da.InsertCommand.Parameters.Add("@ABCCODE", SqlDbType.SmallInt, 0, "ABCCODE")
            da.InsertCommand.Parameters.Add("@Revalue_Inventory", SqlDbType.TinyInt, 0, "Revalue_Inventory")
            da.InsertCommand.Parameters.Add("@Tolerance_Percentage", SqlDbType.Int, 0, "Tolerance_Percentage")
            da.InsertCommand.Parameters.Add("@Purchase_Item_Tax_Schedu", SqlDbType.Char, 0, "Purchase_Item_Tax_Schedu")
            da.InsertCommand.Parameters.Add("@Purchase_Tax_Options", SqlDbType.SmallInt, 0, "Purchase_Tax_Options")
            da.InsertCommand.Parameters.Add("@ITMPLNNNGTYP", SqlDbType.SmallInt, 0, "ITMPLNNNGTYP")
            da.InsertCommand.Parameters.Add("@STTSTCLVLPRCNTG", SqlDbType.SmallInt, 0, "STTSTCLVLPRCNTG")
            da.InsertCommand.Parameters.Add("@CNTRYORGN", SqlDbType.Char, 0, "CNTRYORGN")
            da.InsertCommand.Parameters.Add("@INACTIVE", SqlDbType.TinyInt, 0, "INACTIVE")
            da.InsertCommand.Parameters.Add("@MINSHELF1", SqlDbType.SmallInt, 0, "MINSHELF1")
            da.InsertCommand.Parameters.Add("@MINSHELF2", SqlDbType.SmallInt, 0, "MINSHELF2")
            da.InsertCommand.Parameters.Add("@INCLUDEINDP", SqlDbType.TinyInt, 0, "INCLUDEINDP")
            da.InsertCommand.Parameters.Add("@LOTEXPWARN", SqlDbType.TinyInt, 0, "LOTEXPWARN")
            da.InsertCommand.Parameters.Add("@LOTEXPWARNDAYS", SqlDbType.SmallInt, 0, "LOTEXPWARNDAYS")
            da.InsertCommand.Parameters.Add("@DEX_ROW_TS", SqlDbType.DateTime, 0, "DEX_ROW_TS")

            Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@DEX_ROW_ID", SqlDbType.Int, 0, "DEX_ROW_ID")
            parameter.Direction = ParameterDirection.Output
            dr = ds.Tables("t_IV00101").NewRow
        End If
        Update_Field("ITEMNMBR", _ITEMNMBR, dr)
        Update_Field("ITEMDESC", _ITEMDESC, dr)
        Update_Field("NOTEINDX", _NOTEINDX, dr)
        Update_Field("ITMSHNAM", _ITMSHNAM, dr)
        Update_Field("ITEMTYPE", _ITEMTYPE, dr)
        Update_Field("ITMGEDSC", _ITMGEDSC, dr)
        Update_Field("STNDCOST", _STNDCOST, dr)
        Update_Field("CURRCOST", _CURRCOST, dr)
        Update_Field("ITEMSHWT", _ITEMSHWT, dr)
        Update_Field("DECPLQTY", _DECPLQTY, dr)
        Update_Field("DECPLCUR", _DECPLCUR, dr)
        Update_Field("ITMTSHID", _ITMTSHID, dr)
        Update_Field("TAXOPTNS", _TAXOPTNS, dr)
        Update_Field("IVIVINDX", _IVIVINDX, dr)
        Update_Field("IVIVOFIX", _IVIVOFIX, dr)
        Update_Field("IVCOGSIX", _IVCOGSIX, dr)
        Update_Field("IVSLSIDX", _IVSLSIDX, dr)
        Update_Field("IVSLDSIX", _IVSLDSIX, dr)
        Update_Field("IVSLRNIX", _IVSLRNIX, dr)
        Update_Field("IVINUSIX", _IVINUSIX, dr)
        Update_Field("IVINSVIX", _IVINSVIX, dr)
        Update_Field("IVDMGIDX", _IVDMGIDX, dr)
        Update_Field("IVVARIDX", _IVVARIDX, dr)
        Update_Field("DPSHPIDX", _DPSHPIDX, dr)
        Update_Field("PURPVIDX", _PURPVIDX, dr)
        Update_Field("UPPVIDX", _UPPVIDX, dr)
        Update_Field("IVRETIDX", _IVRETIDX, dr)
        Update_Field("ASMVRIDX", _ASMVRIDX, dr)
        Update_Field("ITMCLSCD", _ITMCLSCD, dr)
        Update_Field("ITMTRKOP", _ITMTRKOP, dr)
        Update_Field("LOTTYPE", _LOTTYPE, dr)
        Update_Field("KPERHIST", _KPERHIST, dr)
        Update_Field("KPTRXHST", _KPTRXHST, dr)
        Update_Field("KPCALHST", _KPCALHST, dr)
        Update_Field("KPDSTHST", _KPDSTHST, dr)
        Update_Field("ALWBKORD", _ALWBKORD, dr)
        Update_Field("VCTNMTHD", _VCTNMTHD, dr)
        Update_Field("UOMSCHDL", _UOMSCHDL, dr)
        Update_Field("ALTITEM1", _ALTITEM1, dr)
        Update_Field("ALTITEM2", _ALTITEM2, dr)
        Update_Field("USCATVLS_1", _USCATVLS_1, dr)
        Update_Field("USCATVLS_2", _USCATVLS_2, dr)
        Update_Field("USCATVLS_3", _USCATVLS_3, dr)
        Update_Field("USCATVLS_4", _USCATVLS_4, dr)
        Update_Field("USCATVLS_5", _USCATVLS_5, dr)
        Update_Field("USCATVLS_6", _USCATVLS_6, dr)
        Update_Field("MSTRCDTY", _MSTRCDTY, dr)
        Update_Field("MODIFDT", _MODIFDT, dr)
        Update_Field("CREATDDT", _CREATDDT, dr)
        Update_Field("WRNTYDYS", _WRNTYDYS, dr)
        Update_Field("PRCLEVEL", _PRCLEVEL, dr)
        Update_Field("LOCNCODE", _LOCNCODE, dr)
        Update_Field("PINFLIDX", _PINFLIDX, dr)
        Update_Field("PURMCIDX", _PURMCIDX, dr)
        Update_Field("IVINFIDX", _IVINFIDX, dr)
        Update_Field("INVMCIDX", _INVMCIDX, dr)
        Update_Field("CGSINFLX", _CGSINFLX, dr)
        Update_Field("CGSMCIDX", _CGSMCIDX, dr)
        Update_Field("ITEMCODE", _ITEMCODE, dr)
        Update_Field("TCC", _TCC, dr)
        Update_Field("PriceGroup", _PriceGroup, dr)
        Update_Field("PRICMTHD", _PRICMTHD, dr)
        Update_Field("PRCHSUOM", _PRCHSUOM, dr)
        Update_Field("SELNGUOM", _SELNGUOM, dr)
        Update_Field("KTACCTSR", _KTACCTSR, dr)
        Update_Field("LASTGENSN", _LASTGENSN, dr)
        Update_Field("ABCCODE", _ABCCODE, dr)
        Update_Field("Revalue_Inventory", _Revalue_Inventory, dr)
        Update_Field("Tolerance_Percentage", _Tolerance_Percentage, dr)
        Update_Field("Purchase_Item_Tax_Schedu", _Purchase_Item_Tax_Schedu, dr)
        Update_Field("Purchase_Tax_Options", _Purchase_Tax_Options, dr)
        Update_Field("ITMPLNNNGTYP", _ITMPLNNNGTYP, dr)
        Update_Field("STTSTCLVLPRCNTG", _STTSTCLVLPRCNTG, dr)
        Update_Field("CNTRYORGN", _CNTRYORGN, dr)
        Update_Field("INACTIVE", _INACTIVE, dr)
        Update_Field("MINSHELF1", _MINSHELF1, dr)
        Update_Field("MINSHELF2", _MINSHELF2, dr)
        Update_Field("INCLUDEINDP", _INCLUDEINDP, dr)
        Update_Field("LOTEXPWARN", _LOTEXPWARN, dr)
        Update_Field("LOTEXPWARNDAYS", _LOTEXPWARNDAYS, dr)
        Update_Field("DEX_ROW_TS", _DEX_ROW_TS, dr)
        'Update_Field("DEX_ROW_ID", _DEX_ROW_ID, dr)
        If ds.Tables("t_IV00101").Rows.Count < 1 Then ds.Tables("t_IV00101").Rows.Add(dr)
        da.Update(ds, "t_IV00101")
        _ID = ds.Tables("t_IV00101").Rows(0).Item("DEX_ROW_ID")
        Return True
        'Catch ex As Exception
        '_Err = ex.ToString
        'Return False
        'End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), RTrim(drow(sField).ToString), "") <> RTrim(sValue) Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "InventoryItemID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub


    Public Function List_Items(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If filter = "" Then
            ds.SelectCommand = "Select DEX_ROW_ID as ID, ItemNmbr, ItemDesc from t_IV00101 order by itemnmbr asc"
        Else
            ds.SelectCommand = "Select DEX_ROW_ID as ID, ItemNmbr, ItemDesc from t_IV00101 where ItemDesc like '" & filter & "%'"
        End If
        Return ds
    End Function

    Public Function List_Filters(ByVal filterlvl As Integer, Optional ByVal Filter1 As String = "", Optional ByVal Filter2 As String = "", Optional ByVal Filter3 As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Select Case filterlvl
            Case 1
                ds.SelectCommand = "Select Distinct(RTRIM(ITMCLSCD)) as Filter from t_IV00101 order by RTRIM(ITMCLSCD) asc"
            Case 2
                ds.SelectCommand = "Select Distinct(RTRIM(USCATVLS_1)) as Filter from t_IV00101 where RTRIM(ITMCLSCD) = '" & Trim(Filter1) & "' ORDER BY RTRIM(USCATVLS_1) ASC"
            Case 3
                ds.SelectCommand = "Select Distinct(RTRIM(USCATVLS_2)) as Filter from t_IV00101 where RTRIM(ITMCLSCD) = '" & Trim(Filter1) & "' and RTRIM(USCATVLS_1) = '" & Trim(Filter2) & "' ORDER BY RTRIM(USCATVLS_2) ASC"
            Case 4
                ds.SelectCommand = "Select Distinct(RTRIM(USCATVLS_3)) as Filter from t_IV00101 where RTRIM(ITMCLSCD) = '" & Trim(Filter1) & "' and RTRIM(USCATVLS_1) = '" & Trim(Filter2) & "' and RTRIM(USCATVLS_2) = '" & Trim(Filter3) & "' ORDER BY RTRIM(USCATVLS_3) ASC"
        End Select
        Return ds
    End Function

    Public Function Verify_Name(ByVal ID As Integer, ByVal item As String) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select DEX_ROW_ID from t_IV00101 where RTrim(ITEMNMBR) = '" & RTrim(item) & "' and DEX_ROW_ID <> " & ID
            dread = cm.ExecuteReader
            bValid = dread.HasRows
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function get_Parent_ID(ByVal ID As Integer) As Integer
        Dim pID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select iv.DEX_ROW_ID from t_IV00101 iv inner join t_IV00102 iv2 on RTrim(iv.ITEMNMBR) = RTrim(iv2.ITEMNMBR) where iv2.DEX_ROW_ID = " & ID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                pID = dread("DEX_ROW_ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return pID
    End Function

    Public Function Get_Item_ID(ByVal item As String) As Integer
        Dim itemID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select DEX_ROW_ID from t_IV00101 where RTrim(ITEMNMBR) = '" & RTrim(item) & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                itemID = dread("DEX_ROW_ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return itemID
    End Function

    Public Property ITEMNMBR() As String
        Get
            Return _ITEMNMBR
        End Get
        Set(ByVal value As String)
            _ITEMNMBR = value
        End Set
    End Property

    Public Property ITEMDESC() As String
        Get
            Return _ITEMDESC
        End Get
        Set(ByVal value As String)
            _ITEMDESC = value
        End Set
    End Property

    Public Property NOTEINDX() As Double
        Get
            Return _NOTEINDX
        End Get
        Set(ByVal value As Double)
            _NOTEINDX = value
        End Set
    End Property

    Public Property ITMSHNAM() As String
        Get
            Return _ITMSHNAM
        End Get
        Set(ByVal value As String)
            _ITMSHNAM = value
        End Set
    End Property

    Public Property ITEMTYPE() As Integer
        Get
            Return _ITEMTYPE
        End Get
        Set(ByVal value As Integer)
            _ITEMTYPE = value
        End Set
    End Property

    Public Property ITMGEDSC() As String
        Get
            Return _ITMGEDSC
        End Get
        Set(ByVal value As String)
            _ITMGEDSC = value
        End Set
    End Property

    Public Property STNDCOST() As Double
        Get
            Return _STNDCOST
        End Get
        Set(ByVal value As Double)
            _STNDCOST = value
        End Set
    End Property

    Public Property CURRCOST() As Double
        Get
            Return _CURRCOST
        End Get
        Set(ByVal value As Double)
            _CURRCOST = value
        End Set
    End Property

    Public Property ITEMSHWT() As Integer
        Get
            Return _ITEMSHWT
        End Get
        Set(ByVal value As Integer)
            _ITEMSHWT = value
        End Set
    End Property

    Public Property DECPLQTY() As Integer
        Get
            Return _DECPLQTY
        End Get
        Set(ByVal value As Integer)
            _DECPLQTY = value
        End Set
    End Property

    Public Property DECPLCUR() As Integer
        Get
            Return _DECPLCUR
        End Get
        Set(ByVal value As Integer)
            _DECPLCUR = value
        End Set
    End Property

    Public Property ITMTSHID() As String
        Get
            Return _ITMTSHID
        End Get
        Set(ByVal value As String)
            _ITMTSHID = value
        End Set
    End Property

    Public Property TAXOPTNS() As Integer
        Get
            Return _TAXOPTNS
        End Get
        Set(ByVal value As Integer)
            _TAXOPTNS = value
        End Set
    End Property

    Public Property IVIVINDX() As Integer
        Get
            Return _IVIVINDX
        End Get
        Set(ByVal value As Integer)
            _IVIVINDX = value
        End Set
    End Property

    Public Property IVIVOFIX() As Integer
        Get
            Return _IVIVOFIX
        End Get
        Set(ByVal value As Integer)
            _IVIVOFIX = value
        End Set
    End Property

    Public Property IVCOGSIX() As Integer
        Get
            Return _IVCOGSIX
        End Get
        Set(ByVal value As Integer)
            _IVCOGSIX = value
        End Set
    End Property

    Public Property IVSLSIDX() As Integer
        Get
            Return _IVSLSIDX
        End Get
        Set(ByVal value As Integer)
            _IVSLSIDX = value
        End Set
    End Property

    Public Property IVSLDSIX() As Integer
        Get
            Return _IVSLDSIX
        End Get
        Set(ByVal value As Integer)
            _IVSLDSIX = value
        End Set
    End Property

    Public Property IVSLRNIX() As Integer
        Get
            Return _IVSLRNIX
        End Get
        Set(ByVal value As Integer)
            _IVSLRNIX = value
        End Set
    End Property

    Public Property IVINUSIX() As Integer
        Get
            Return _IVINUSIX
        End Get
        Set(ByVal value As Integer)
            _IVINUSIX = value
        End Set
    End Property

    Public Property IVINSVIX() As Integer
        Get
            Return _IVINSVIX
        End Get
        Set(ByVal value As Integer)
            _IVINSVIX = value
        End Set
    End Property

    Public Property IVDMGIDX() As Integer
        Get
            Return _IVDMGIDX
        End Get
        Set(ByVal value As Integer)
            _IVDMGIDX = value
        End Set
    End Property

    Public Property IVVARIDX() As Integer
        Get
            Return _IVVARIDX
        End Get
        Set(ByVal value As Integer)
            _IVVARIDX = value
        End Set
    End Property

    Public Property DPSHPIDX() As Integer
        Get
            Return _DPSHPIDX
        End Get
        Set(ByVal value As Integer)
            _DPSHPIDX = value
        End Set
    End Property

    Public Property PURPVIDX() As Integer
        Get
            Return _PURPVIDX
        End Get
        Set(ByVal value As Integer)
            _PURPVIDX = value
        End Set
    End Property

    Public Property UPPVIDX() As Integer
        Get
            Return _UPPVIDX
        End Get
        Set(ByVal value As Integer)
            _UPPVIDX = value
        End Set
    End Property

    Public Property IVRETIDX() As Integer
        Get
            Return _IVRETIDX
        End Get
        Set(ByVal value As Integer)
            _IVRETIDX = value
        End Set
    End Property

    Public Property ASMVRIDX() As Integer
        Get
            Return _ASMVRIDX
        End Get
        Set(ByVal value As Integer)
            _ASMVRIDX = value
        End Set
    End Property

    Public Property ITMCLSCD() As String
        Get
            Return _ITMCLSCD
        End Get
        Set(ByVal value As String)
            _ITMCLSCD = value
        End Set
    End Property

    Public Property ITMTRKOP() As Integer
        Get
            Return _ITMTRKOP
        End Get
        Set(ByVal value As Integer)
            _ITMTRKOP = value
        End Set
    End Property

    Public Property LOTTYPE() As String
        Get
            Return _LOTTYPE
        End Get
        Set(ByVal value As String)
            _LOTTYPE = value
        End Set
    End Property

    Public Property KPERHIST() As Integer
        Get
            Return _KPERHIST
        End Get
        Set(ByVal value As Integer)
            _KPERHIST = value
        End Set
    End Property

    Public Property KPTRXHST() As Integer
        Get
            Return _KPTRXHST
        End Get
        Set(ByVal value As Integer)
            _KPTRXHST = value
        End Set
    End Property

    Public Property KPCALHST() As Integer
        Get
            Return _KPCALHST
        End Get
        Set(ByVal value As Integer)
            _KPCALHST = value
        End Set
    End Property

    Public Property KPDSTHST() As Integer
        Get
            Return _KPDSTHST
        End Get
        Set(ByVal value As Integer)
            _KPDSTHST = value
        End Set
    End Property

    Public Property ALWBKORD() As Integer
        Get
            Return _ALWBKORD
        End Get
        Set(ByVal value As Integer)
            _ALWBKORD = value
        End Set
    End Property

    Public Property VCTNMTHD() As Integer
        Get
            Return _VCTNMTHD
        End Get
        Set(ByVal value As Integer)
            _VCTNMTHD = value
        End Set
    End Property

    Public Property UOMSCHDL() As String
        Get
            Return _UOMSCHDL
        End Get
        Set(ByVal value As String)
            _UOMSCHDL = value
        End Set
    End Property

    Public Property ALTITEM1() As String
        Get
            Return _ALTITEM1
        End Get
        Set(ByVal value As String)
            _ALTITEM1 = value
        End Set
    End Property

    Public Property ALTITEM2() As String
        Get
            Return _ALTITEM2
        End Get
        Set(ByVal value As String)
            _ALTITEM2 = value
        End Set
    End Property

    Public Property USCATVLS_1() As String
        Get
            Return _USCATVLS_1
        End Get
        Set(ByVal value As String)
            _USCATVLS_1 = value
        End Set
    End Property

    Public Property USCATVLS_2() As String
        Get
            Return _USCATVLS_2
        End Get
        Set(ByVal value As String)
            _USCATVLS_2 = value
        End Set
    End Property

    Public Property USCATVLS_3() As String
        Get
            Return _USCATVLS_3
        End Get
        Set(ByVal value As String)
            _USCATVLS_3 = value
        End Set
    End Property

    Public Property USCATVLS_4() As String
        Get
            Return _USCATVLS_4
        End Get
        Set(ByVal value As String)
            _USCATVLS_4 = value
        End Set
    End Property

    Public Property USCATVLS_5() As String
        Get
            Return _USCATVLS_5
        End Get
        Set(ByVal value As String)
            _USCATVLS_5 = value
        End Set
    End Property

    Public Property USCATVLS_6() As String
        Get
            Return _USCATVLS_6
        End Get
        Set(ByVal value As String)
            _USCATVLS_6 = value
        End Set
    End Property

    Public Property MSTRCDTY() As Integer
        Get
            Return _MSTRCDTY
        End Get
        Set(ByVal value As Integer)
            _MSTRCDTY = value
        End Set
    End Property

    Public Property MODIFDT() As String
        Get
            Return _MODIFDT
        End Get
        Set(ByVal value As String)
            _MODIFDT = value
        End Set
    End Property

    Public Property CREATDDT() As String
        Get
            Return _CREATDDT
        End Get
        Set(ByVal value As String)
            _CREATDDT = value
        End Set
    End Property

    Public Property WRNTYDYS() As Integer
        Get
            Return _WRNTYDYS
        End Get
        Set(ByVal value As Integer)
            _WRNTYDYS = value
        End Set
    End Property

    Public Property PRCLEVEL() As String
        Get
            Return _PRCLEVEL
        End Get
        Set(ByVal value As String)
            _PRCLEVEL = value
        End Set
    End Property

    Public Property LOCNCODE() As String
        Get
            Return _LOCNCODE
        End Get
        Set(ByVal value As String)
            _LOCNCODE = value
        End Set
    End Property

    Public Property PINFLIDX() As Integer
        Get
            Return _PINFLIDX
        End Get
        Set(ByVal value As Integer)
            _PINFLIDX = value
        End Set
    End Property

    Public Property PURMCIDX() As Integer
        Get
            Return _PURMCIDX
        End Get
        Set(ByVal value As Integer)
            _PURMCIDX = value
        End Set
    End Property

    Public Property IVINFIDX() As Integer
        Get
            Return _IVINFIDX
        End Get
        Set(ByVal value As Integer)
            _IVINFIDX = value
        End Set
    End Property

    Public Property INVMCIDX() As Integer
        Get
            Return _INVMCIDX
        End Get
        Set(ByVal value As Integer)
            _INVMCIDX = value
        End Set
    End Property

    Public Property CGSINFLX() As Integer
        Get
            Return _CGSINFLX
        End Get
        Set(ByVal value As Integer)
            _CGSINFLX = value
        End Set
    End Property

    Public Property CGSMCIDX() As Integer
        Get
            Return _CGSMCIDX
        End Get
        Set(ByVal value As Integer)
            _CGSMCIDX = value
        End Set
    End Property

    Public Property ITEMCODE() As String
        Get
            Return _ITEMCODE
        End Get
        Set(ByVal value As String)
            _ITEMCODE = value
        End Set
    End Property

    Public Property TCC() As String
        Get
            Return _TCC
        End Get
        Set(ByVal value As String)
            _TCC = value
        End Set
    End Property

    Public Property PriceGroup() As String
        Get
            Return _PriceGroup
        End Get
        Set(ByVal value As String)
            _PriceGroup = value
        End Set
    End Property

    Public Property PRICMTHD() As Integer
        Get
            Return _PRICMTHD
        End Get
        Set(ByVal value As Integer)
            _PRICMTHD = value
        End Set
    End Property

    Public Property PRCHSUOM() As String
        Get
            Return _PRCHSUOM
        End Get
        Set(ByVal value As String)
            _PRCHSUOM = value
        End Set
    End Property

    Public Property SELNGUOM() As String
        Get
            Return _SELNGUOM
        End Get
        Set(ByVal value As String)
            _SELNGUOM = value
        End Set
    End Property

    Public Property KTACCTSR() As Integer
        Get
            Return _KTACCTSR
        End Get
        Set(ByVal value As Integer)
            _KTACCTSR = value
        End Set
    End Property

    Public Property LASTGENSN() As String
        Get
            Return _LASTGENSN
        End Get
        Set(ByVal value As String)
            _LASTGENSN = value
        End Set
    End Property

    Public Property ABCCODE() As Integer
        Get
            Return _ABCCODE
        End Get
        Set(ByVal value As Integer)
            _ABCCODE = value
        End Set
    End Property

    Public Property Revalue_Inventory() As Integer
        Get
            Return _Revalue_Inventory
        End Get
        Set(ByVal value As Integer)
            _Revalue_Inventory = value
        End Set
    End Property

    Public Property Tolerance_Percentage() As Integer
        Get
            Return _Tolerance_Percentage
        End Get
        Set(ByVal value As Integer)
            _Tolerance_Percentage = value
        End Set
    End Property

    Public Property Purchase_Item_Tax_Schedu() As String
        Get
            Return _Purchase_Item_Tax_Schedu
        End Get
        Set(ByVal value As String)
            _Purchase_Item_Tax_Schedu = value
        End Set
    End Property

    Public Property Purchase_Tax_Options() As Integer
        Get
            Return _Purchase_Tax_Options
        End Get
        Set(ByVal value As Integer)
            _Purchase_Tax_Options = value
        End Set
    End Property

    Public Property ITMPLNNNGTYP() As Integer
        Get
            Return _ITMPLNNNGTYP
        End Get
        Set(ByVal value As Integer)
            _ITMPLNNNGTYP = value
        End Set
    End Property

    Public Property STTSTCLVLPRCNTG() As Integer
        Get
            Return _STTSTCLVLPRCNTG
        End Get
        Set(ByVal value As Integer)
            _STTSTCLVLPRCNTG = value
        End Set
    End Property

    Public Property CNTRYORGN() As String
        Get
            Return _CNTRYORGN
        End Get
        Set(ByVal value As String)
            _CNTRYORGN = value
        End Set
    End Property

    Public Property INACTIVE() As Integer
        Get
            Return _INACTIVE
        End Get
        Set(ByVal value As Integer)
            _INACTIVE = value
        End Set
    End Property

    Public Property MINSHELF1() As Integer
        Get
            Return _MINSHELF1
        End Get
        Set(ByVal value As Integer)
            _MINSHELF1 = value
        End Set
    End Property

    Public Property MINSHELF2() As Integer
        Get
            Return _MINSHELF2
        End Get
        Set(ByVal value As Integer)
            _MINSHELF2 = value
        End Set
    End Property

    Public Property INCLUDEINDP() As Integer
        Get
            Return _INCLUDEINDP
        End Get
        Set(ByVal value As Integer)
            _INCLUDEINDP = value
        End Set
    End Property

    Public Property LOTEXPWARN() As Integer
        Get
            Return _LOTEXPWARN
        End Get
        Set(ByVal value As Integer)
            _LOTEXPWARN = value
        End Set
    End Property

    Public Property LOTEXPWARNDAYS() As Integer
        Get
            Return _LOTEXPWARNDAYS
        End Get
        Set(ByVal value As Integer)
            _LOTEXPWARNDAYS = value
        End Set
    End Property

    Public Property DEX_ROW_TS() As String
        Get
            Return _DEX_ROW_TS
        End Get
        Set(ByVal value As String)
            _DEX_ROW_TS = value
        End Set
    End Property

    Public Property DEX_ROW_ID() As Integer
        Get
            Return _DEX_ROW_ID
        End Get
        Set(ByVal value As Integer)
            _DEX_ROW_ID = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

End Class
