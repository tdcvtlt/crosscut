Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Data.OleDb
Imports System.Web.UI.WebControls

Public Class clsPM00200
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _VENDORID As String = ""
    Dim _VENDNAME As String = ""
    Dim _VNDCHKNM As String = ""
    Dim _VENDSHNM As String = ""
    Dim _VADDCDPR As String = ""
    Dim _VADCDPAD As String = ""
    Dim _VADCDSFR As String = ""
    Dim _VADCDTRO As String = ""
    Dim _VNDCLSID As String = ""
    Dim _VNDCNTCT As String = ""
    Dim _ADDRESS1 As String = ""
    Dim _ADDRESS2 As String = ""
    Dim _ADDRESS3 As String = ""
    Dim _CITY As String = ""
    Dim _STATE As String = ""
    Dim _ZIPCODE As String = ""
    Dim _COUNTRY As String = ""
    Dim _PHNUMBR1 As String = ""
    Dim _PHNUMBR2 As String = ""
    Dim _PHONE3 As String = ""
    Dim _FAXNUMBR As String = ""
    Dim _UPSZONE As String = ""
    Dim _SHIPMTHD As String = ""
    Dim _TAXSCHID As String = ""
    Dim _ACNMVNDR As String = ""
    Dim _TXIDNMBR As String = ""
    Dim _VENDSTTS As Integer = 0
    Dim _CURNCYID As String = ""
    Dim _TXRGNNUM As String = ""
    Dim _PARVENID As String = ""
    Dim _TRDDISCT As Integer = 0
    Dim _TEN99TYPE As Integer = 0
    Dim _TEN99BOXNUMBER As Integer = 0
    Dim _MINORDER As Decimal = 0.0
    Dim _PYMTRMID As String = ""
    Dim _MINPYTYP As Integer = 0
    Dim _MINPYPCT As Integer = 0
    Dim _MINPYDLR As Decimal = 0.0
    Dim _MXIAFVND As Integer = 0
    Dim _MAXINDLR As Decimal = 0.0
    Dim _COMMENT1 As String = ""
    Dim _COMMENT2 As String = ""
    Dim _USERDEF1 As String = ""
    Dim _USERDEF2 As String = ""
    Dim _CRLMTDLR As Decimal = 0.0
    Dim _PYMNTPRI As String = ""
    Dim _KPCALHST As Integer = 0
    Dim _KGLDSTHS As Integer = 0
    Dim _KPERHIST As Integer = 0
    Dim _KPTRXHST As Integer = 0
    Dim _HOLD As Integer = 0
    Dim _PTCSHACF As Integer = 0
    Dim _CREDTLMT As Integer = 0
    Dim _WRITEOFF As Integer = 0
    Dim _MXWOFAMT As Decimal = 0.0
    Dim _SBPPSDED As Integer = 0
    Dim _PPSTAXRT As Integer = 0
    Dim _DXVARNUM As String = ""
    Dim _CRTCOMDT As String = ""
    Dim _CRTEXPDT As String = ""
    Dim _RTOBUTKN As Integer = 0
    Dim _XPDTOBLG As Integer = 0
    Dim _PRSPAYEE As Integer = 0
    Dim _PMAPINDX As Integer = 0
    Dim _PMCSHIDX As Integer = 0
    Dim _PMDAVIDX As Integer = 0
    Dim _PMDTKIDX As Integer = 0
    Dim _PMFINIDX As Integer = 0
    Dim _PMMSCHIX As Integer = 0
    Dim _PMFRTIDX As Integer = 0
    Dim _PMTAXIDX As Integer = 0
    Dim _PMWRTIDX As Integer = 0
    Dim _PMPRCHIX As Integer = 0
    Dim _PMRTNGIX As Integer = 0
    Dim _PMTDSCIX As Integer = 0
    Dim _ACPURIDX As Integer = 0
    Dim _PURPVIDX As Integer = 0
    Dim _NOTEINDX As Decimal = 0.0
    Dim _CHEKBKID As String = ""
    Dim _MODIFDT As String = ""
    Dim _CREATDDT As String = ""
    Dim _RATETPID As String = ""
    Dim _Revalue_Vendor As Integer = 0
    Dim _Post_Results_To As Integer = 0
    Dim _FREEONBOARD As Integer = 0
    Dim _GOVCRPID As String = ""
    Dim _GOVINDID As String = ""
    Dim _DISGRPER As Integer = 0
    Dim _DUEGRPER As Integer = 0
    Dim _DOCFMTID As String = ""
    Dim _TaxInvRecvd As Integer = 0
    Dim _USERLANG As Integer = 0
    Dim _WithholdingType As Integer = 0
    Dim _WithholdingFormType As Integer = 0
    Dim _WithholdingEntityType As Integer = 0
    Dim _TaxFileNumMode As Integer = 0
    Dim _BRTHDATE As String = ""
    Dim _LaborPmtType As Integer = 0
    Dim _CCode As String = ""
    Dim _DECLID As String = ""
    Dim _CBVAT As Integer = 0
    Dim _Workflow_Approval_Status As Integer = 0
    Dim _Workflow_Priority As Integer = 0
    Dim _DEX_ROW_TS As String = ""
    Dim _DEX_ROW_ID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PM00200 where DEX_ROW_ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PM00200 where DEX_ROW_ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PM00200")
            If ds.Tables("t_PM00200").Rows.Count > 0 Then
                dr = ds.Tables("t_PM00200").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("VENDORID") Is System.DBNull.Value) Then _VENDORID = dr("VENDORID")
        If Not (dr("VENDNAME") Is System.DBNull.Value) Then _VENDNAME = dr("VENDNAME")
        If Not (dr("VNDCHKNM") Is System.DBNull.Value) Then _VNDCHKNM = dr("VNDCHKNM")
        If Not (dr("VENDSHNM") Is System.DBNull.Value) Then _VENDSHNM = dr("VENDSHNM")
        If Not (dr("VADDCDPR") Is System.DBNull.Value) Then _VADDCDPR = dr("VADDCDPR")
        If Not (dr("VADCDPAD") Is System.DBNull.Value) Then _VADCDPAD = dr("VADCDPAD")
        If Not (dr("VADCDSFR") Is System.DBNull.Value) Then _VADCDSFR = dr("VADCDSFR")
        If Not (dr("VADCDTRO") Is System.DBNull.Value) Then _VADCDTRO = dr("VADCDTRO")
        If Not (dr("VNDCLSID") Is System.DBNull.Value) Then _VNDCLSID = dr("VNDCLSID")
        If Not (dr("VNDCNTCT") Is System.DBNull.Value) Then _VNDCNTCT = dr("VNDCNTCT")
        If Not (dr("ADDRESS1") Is System.DBNull.Value) Then _ADDRESS1 = dr("ADDRESS1")
        If Not (dr("ADDRESS2") Is System.DBNull.Value) Then _ADDRESS2 = dr("ADDRESS2")
        If Not (dr("ADDRESS3") Is System.DBNull.Value) Then _ADDRESS3 = dr("ADDRESS3")
        If Not (dr("CITY") Is System.DBNull.Value) Then _CITY = dr("CITY")
        If Not (dr("STATE") Is System.DBNull.Value) Then _STATE = dr("STATE")
        If Not (dr("ZIPCODE") Is System.DBNull.Value) Then _ZIPCODE = dr("ZIPCODE")
        If Not (dr("COUNTRY") Is System.DBNull.Value) Then _COUNTRY = dr("COUNTRY")
        If Not (dr("PHNUMBR1") Is System.DBNull.Value) Then _PHNUMBR1 = dr("PHNUMBR1")
        If Not (dr("PHNUMBR2") Is System.DBNull.Value) Then _PHNUMBR2 = dr("PHNUMBR2")
        If Not (dr("PHONE3") Is System.DBNull.Value) Then _PHONE3 = dr("PHONE3")
        If Not (dr("FAXNUMBR") Is System.DBNull.Value) Then _FAXNUMBR = dr("FAXNUMBR")
        If Not (dr("UPSZONE") Is System.DBNull.Value) Then _UPSZONE = dr("UPSZONE")
        If Not (dr("SHIPMTHD") Is System.DBNull.Value) Then _SHIPMTHD = dr("SHIPMTHD")
        If Not (dr("TAXSCHID") Is System.DBNull.Value) Then _TAXSCHID = dr("TAXSCHID")
        If Not (dr("ACNMVNDR") Is System.DBNull.Value) Then _ACNMVNDR = dr("ACNMVNDR")
        If Not (dr("TXIDNMBR") Is System.DBNull.Value) Then _TXIDNMBR = dr("TXIDNMBR")
        If Not (dr("VENDSTTS") Is System.DBNull.Value) Then _VENDSTTS = dr("VENDSTTS")
        If Not (dr("CURNCYID") Is System.DBNull.Value) Then _CURNCYID = dr("CURNCYID")
        If Not (dr("TXRGNNUM") Is System.DBNull.Value) Then _TXRGNNUM = dr("TXRGNNUM")
        If Not (dr("PARVENID") Is System.DBNull.Value) Then _PARVENID = dr("PARVENID")
        If Not (dr("TRDDISCT") Is System.DBNull.Value) Then _TRDDISCT = dr("TRDDISCT")
        If Not (dr("TEN99TYPE") Is System.DBNull.Value) Then _TEN99TYPE = dr("TEN99TYPE")
        If Not (dr("TEN99BOXNUMBER") Is System.DBNull.Value) Then _TEN99BOXNUMBER = dr("TEN99BOXNUMBER")
        If Not (dr("MINORDER") Is System.DBNull.Value) Then _MINORDER = dr("MINORDER")
        If Not (dr("PYMTRMID") Is System.DBNull.Value) Then _PYMTRMID = dr("PYMTRMID")
        If Not (dr("MINPYTYP") Is System.DBNull.Value) Then _MINPYTYP = dr("MINPYTYP")
        If Not (dr("MINPYPCT") Is System.DBNull.Value) Then _MINPYPCT = dr("MINPYPCT")
        If Not (dr("MINPYDLR") Is System.DBNull.Value) Then _MINPYDLR = dr("MINPYDLR")
        If Not (dr("MXIAFVND") Is System.DBNull.Value) Then _MXIAFVND = dr("MXIAFVND")
        If Not (dr("MAXINDLR") Is System.DBNull.Value) Then _MAXINDLR = dr("MAXINDLR")
        If Not (dr("COMMENT1") Is System.DBNull.Value) Then _COMMENT1 = dr("COMMENT1")
        If Not (dr("COMMENT2") Is System.DBNull.Value) Then _COMMENT2 = dr("COMMENT2")
        If Not (dr("USERDEF1") Is System.DBNull.Value) Then _USERDEF1 = dr("USERDEF1")
        If Not (dr("USERDEF2") Is System.DBNull.Value) Then _USERDEF2 = dr("USERDEF2")
        If Not (dr("CRLMTDLR") Is System.DBNull.Value) Then _CRLMTDLR = dr("CRLMTDLR")
        If Not (dr("PYMNTPRI") Is System.DBNull.Value) Then _PYMNTPRI = dr("PYMNTPRI")
        If Not (dr("KPCALHST") Is System.DBNull.Value) Then _KPCALHST = dr("KPCALHST")
        If Not (dr("KGLDSTHS") Is System.DBNull.Value) Then _KGLDSTHS = dr("KGLDSTHS")
        If Not (dr("KPERHIST") Is System.DBNull.Value) Then _KPERHIST = dr("KPERHIST")
        If Not (dr("KPTRXHST") Is System.DBNull.Value) Then _KPTRXHST = dr("KPTRXHST")
        If Not (dr("HOLD") Is System.DBNull.Value) Then _HOLD = dr("HOLD")
        If Not (dr("PTCSHACF") Is System.DBNull.Value) Then _PTCSHACF = dr("PTCSHACF")
        If Not (dr("CREDTLMT") Is System.DBNull.Value) Then _CREDTLMT = dr("CREDTLMT")
        If Not (dr("WRITEOFF") Is System.DBNull.Value) Then _WRITEOFF = dr("WRITEOFF")
        If Not (dr("MXWOFAMT") Is System.DBNull.Value) Then _MXWOFAMT = dr("MXWOFAMT")
        If Not (dr("SBPPSDED") Is System.DBNull.Value) Then _SBPPSDED = dr("SBPPSDED")
        If Not (dr("PPSTAXRT") Is System.DBNull.Value) Then _PPSTAXRT = dr("PPSTAXRT")
        If Not (dr("DXVARNUM") Is System.DBNull.Value) Then _DXVARNUM = dr("DXVARNUM")
        If Not (dr("CRTCOMDT") Is System.DBNull.Value) Then _CRTCOMDT = dr("CRTCOMDT")
        If Not (dr("CRTEXPDT") Is System.DBNull.Value) Then _CRTEXPDT = dr("CRTEXPDT")
        If Not (dr("RTOBUTKN") Is System.DBNull.Value) Then _RTOBUTKN = dr("RTOBUTKN")
        If Not (dr("XPDTOBLG") Is System.DBNull.Value) Then _XPDTOBLG = dr("XPDTOBLG")
        If Not (dr("PRSPAYEE") Is System.DBNull.Value) Then _PRSPAYEE = dr("PRSPAYEE")
        If Not (dr("PMAPINDX") Is System.DBNull.Value) Then _PMAPINDX = dr("PMAPINDX")
        If Not (dr("PMCSHIDX") Is System.DBNull.Value) Then _PMCSHIDX = dr("PMCSHIDX")
        If Not (dr("PMDAVIDX") Is System.DBNull.Value) Then _PMDAVIDX = dr("PMDAVIDX")
        If Not (dr("PMDTKIDX") Is System.DBNull.Value) Then _PMDTKIDX = dr("PMDTKIDX")
        If Not (dr("PMFINIDX") Is System.DBNull.Value) Then _PMFINIDX = dr("PMFINIDX")
        If Not (dr("PMMSCHIX") Is System.DBNull.Value) Then _PMMSCHIX = dr("PMMSCHIX")
        If Not (dr("PMFRTIDX") Is System.DBNull.Value) Then _PMFRTIDX = dr("PMFRTIDX")
        If Not (dr("PMTAXIDX") Is System.DBNull.Value) Then _PMTAXIDX = dr("PMTAXIDX")
        If Not (dr("PMWRTIDX") Is System.DBNull.Value) Then _PMWRTIDX = dr("PMWRTIDX")
        If Not (dr("PMPRCHIX") Is System.DBNull.Value) Then _PMPRCHIX = dr("PMPRCHIX")
        If Not (dr("PMRTNGIX") Is System.DBNull.Value) Then _PMRTNGIX = dr("PMRTNGIX")
        If Not (dr("PMTDSCIX") Is System.DBNull.Value) Then _PMTDSCIX = dr("PMTDSCIX")
        If Not (dr("ACPURIDX") Is System.DBNull.Value) Then _ACPURIDX = dr("ACPURIDX")
        If Not (dr("PURPVIDX") Is System.DBNull.Value) Then _PURPVIDX = dr("PURPVIDX")
        If Not (dr("NOTEINDX") Is System.DBNull.Value) Then _NOTEINDX = dr("NOTEINDX")
        If Not (dr("CHEKBKID") Is System.DBNull.Value) Then _CHEKBKID = dr("CHEKBKID")
        If Not (dr("MODIFDT") Is System.DBNull.Value) Then _MODIFDT = dr("MODIFDT")
        If Not (dr("CREATDDT") Is System.DBNull.Value) Then _CREATDDT = dr("CREATDDT")
        If Not (dr("RATETPID") Is System.DBNull.Value) Then _RATETPID = dr("RATETPID")
        If Not (dr("Revalue_Vendor") Is System.DBNull.Value) Then _Revalue_Vendor = dr("Revalue_Vendor")
        If Not (dr("Post_Results_To") Is System.DBNull.Value) Then _Post_Results_To = dr("Post_Results_To")
        If Not (dr("FREEONBOARD") Is System.DBNull.Value) Then _FREEONBOARD = dr("FREEONBOARD")
        If Not (dr("GOVCRPID") Is System.DBNull.Value) Then _GOVCRPID = dr("GOVCRPID")
        If Not (dr("GOVINDID") Is System.DBNull.Value) Then _GOVINDID = dr("GOVINDID")
        If Not (dr("DISGRPER") Is System.DBNull.Value) Then _DISGRPER = dr("DISGRPER")
        If Not (dr("DUEGRPER") Is System.DBNull.Value) Then _DUEGRPER = dr("DUEGRPER")
        If Not (dr("DOCFMTID") Is System.DBNull.Value) Then _DOCFMTID = dr("DOCFMTID")
        If Not (dr("TaxInvRecvd") Is System.DBNull.Value) Then _TaxInvRecvd = dr("TaxInvRecvd")
        If Not (dr("USERLANG") Is System.DBNull.Value) Then _USERLANG = dr("USERLANG")
        If Not (dr("WithholdingType") Is System.DBNull.Value) Then _WithholdingType = dr("WithholdingType")
        If Not (dr("WithholdingFormType") Is System.DBNull.Value) Then _WithholdingFormType = dr("WithholdingFormType")
        If Not (dr("WithholdingEntityType") Is System.DBNull.Value) Then _WithholdingEntityType = dr("WithholdingEntityType")
        If Not (dr("TaxFileNumMode") Is System.DBNull.Value) Then _TaxFileNumMode = dr("TaxFileNumMode")
        If Not (dr("BRTHDATE") Is System.DBNull.Value) Then _BRTHDATE = dr("BRTHDATE")
        If Not (dr("LaborPmtType") Is System.DBNull.Value) Then _LaborPmtType = dr("LaborPmtType")
        If Not (dr("CCode") Is System.DBNull.Value) Then _CCode = dr("CCode")
        If Not (dr("DECLID") Is System.DBNull.Value) Then _DECLID = dr("DECLID")
        If Not (dr("CBVAT") Is System.DBNull.Value) Then _CBVAT = dr("CBVAT")
        If Not (dr("Workflow_Approval_Status") Is System.DBNull.Value) Then _Workflow_Approval_Status = dr("Workflow_Approval_Status")
        If Not (dr("Workflow_Priority") Is System.DBNull.Value) Then _Workflow_Priority = dr("Workflow_Priority")
        If Not (dr("DEX_ROW_TS") Is System.DBNull.Value) Then _DEX_ROW_TS = dr("DEX_ROW_TS")
        'If Not (dr("DEX_ROW_ID") Is System.DBNull.Value) Then _DEX_ROW_ID = dr("DEX_ROW_ID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PM00200 where DEX_ROW_ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PM00200")
            If ds.Tables("t_PM00200").Rows.Count > 0 Then
                dr = ds.Tables("t_PM00200").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PM00200Insert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@VENDORID", SqlDbType.Char, 0, "VENDORID")
                da.InsertCommand.Parameters.Add("@VENDNAME", SqlDbType.Char, 0, "VENDNAME")
                da.InsertCommand.Parameters.Add("@VNDCHKNM", SqlDbType.Char, 0, "VNDCHKNM")
                da.InsertCommand.Parameters.Add("@VENDSHNM", SqlDbType.Char, 0, "VENDSHNM")
                da.InsertCommand.Parameters.Add("@VADDCDPR", SqlDbType.Char, 0, "VADDCDPR")
                da.InsertCommand.Parameters.Add("@VADCDPAD", SqlDbType.Char, 0, "VADCDPAD")
                da.InsertCommand.Parameters.Add("@VADCDSFR", SqlDbType.Char, 0, "VADCDSFR")
                da.InsertCommand.Parameters.Add("@VADCDTRO", SqlDbType.Char, 0, "VADCDTRO")
                da.InsertCommand.Parameters.Add("@VNDCLSID", SqlDbType.Char, 0, "VNDCLSID")
                da.InsertCommand.Parameters.Add("@VNDCNTCT", SqlDbType.Char, 0, "VNDCNTCT")
                da.InsertCommand.Parameters.Add("@ADDRESS1", SqlDbType.Char, 0, "ADDRESS1")
                da.InsertCommand.Parameters.Add("@ADDRESS2", SqlDbType.Char, 0, "ADDRESS2")
                da.InsertCommand.Parameters.Add("@ADDRESS3", SqlDbType.Char, 0, "ADDRESS3")
                da.InsertCommand.Parameters.Add("@CITY", SqlDbType.Char, 0, "CITY")
                da.InsertCommand.Parameters.Add("@STATE", SqlDbType.Char, 0, "STATE")
                da.InsertCommand.Parameters.Add("@ZIPCODE", SqlDbType.Char, 0, "ZIPCODE")
                da.InsertCommand.Parameters.Add("@COUNTRY", SqlDbType.Char, 0, "COUNTRY")
                da.InsertCommand.Parameters.Add("@PHNUMBR1", SqlDbType.Char, 0, "PHNUMBR1")
                da.InsertCommand.Parameters.Add("@PHNUMBR2", SqlDbType.Char, 0, "PHNUMBR2")
                da.InsertCommand.Parameters.Add("@PHONE3", SqlDbType.Char, 0, "PHONE3")
                da.InsertCommand.Parameters.Add("@FAXNUMBR", SqlDbType.Char, 0, "FAXNUMBR")
                da.InsertCommand.Parameters.Add("@UPSZONE", SqlDbType.Char, 0, "UPSZONE")
                da.InsertCommand.Parameters.Add("@SHIPMTHD", SqlDbType.Char, 0, "SHIPMTHD")
                da.InsertCommand.Parameters.Add("@TAXSCHID", SqlDbType.Char, 0, "TAXSCHID")
                da.InsertCommand.Parameters.Add("@ACNMVNDR", SqlDbType.Char, 0, "ACNMVNDR")
                da.InsertCommand.Parameters.Add("@TXIDNMBR", SqlDbType.Char, 0, "TXIDNMBR")
                da.InsertCommand.Parameters.Add("@VENDSTTS", SqlDbType.SmallInt, 0, "VENDSTTS")
                da.InsertCommand.Parameters.Add("@CURNCYID", SqlDbType.Char, 0, "CURNCYID")
                da.InsertCommand.Parameters.Add("@TXRGNNUM", SqlDbType.Char, 0, "TXRGNNUM")
                da.InsertCommand.Parameters.Add("@PARVENID", SqlDbType.Char, 0, "PARVENID")
                da.InsertCommand.Parameters.Add("@TRDDISCT", SqlDbType.SmallInt, 0, "TRDDISCT")
                da.InsertCommand.Parameters.Add("@TEN99TYPE", SqlDbType.SmallInt, 0, "TEN99TYPE")
                da.InsertCommand.Parameters.Add("@TEN99BOXNUMBER", SqlDbType.SmallInt, 0, "TEN99BOXNUMBER")
                da.InsertCommand.Parameters.Add("@MINORDER", SqlDbType.Decimal, 0, "MINORDER")
                da.InsertCommand.Parameters.Add("@PYMTRMID", SqlDbType.Char, 0, "PYMTRMID")
                da.InsertCommand.Parameters.Add("@MINPYTYP", SqlDbType.SmallInt, 0, "MINPYTYP")
                da.InsertCommand.Parameters.Add("@MINPYPCT", SqlDbType.SmallInt, 0, "MINPYPCT")
                da.InsertCommand.Parameters.Add("@MINPYDLR", SqlDbType.Decimal, 0, "MINPYDLR")
                da.InsertCommand.Parameters.Add("@MXIAFVND", SqlDbType.SmallInt, 0, "MXIAFVND")
                da.InsertCommand.Parameters.Add("@MAXINDLR", SqlDbType.Decimal, 0, "MAXINDLR")
                da.InsertCommand.Parameters.Add("@COMMENT1", SqlDbType.Char, 0, "COMMENT1")
                da.InsertCommand.Parameters.Add("@COMMENT2", SqlDbType.Char, 0, "COMMENT2")
                da.InsertCommand.Parameters.Add("@USERDEF1", SqlDbType.Char, 0, "USERDEF1")
                da.InsertCommand.Parameters.Add("@USERDEF2", SqlDbType.Char, 0, "USERDEF2")
                da.InsertCommand.Parameters.Add("@CRLMTDLR", SqlDbType.Decimal, 0, "CRLMTDLR")
                da.InsertCommand.Parameters.Add("@PYMNTPRI", SqlDbType.Char, 0, "PYMNTPRI")
                da.InsertCommand.Parameters.Add("@KPCALHST", SqlDbType.TinyInt, 0, "KPCALHST")
                da.InsertCommand.Parameters.Add("@KGLDSTHS", SqlDbType.TinyInt, 0, "KGLDSTHS")
                da.InsertCommand.Parameters.Add("@KPERHIST", SqlDbType.TinyInt, 0, "KPERHIST")
                da.InsertCommand.Parameters.Add("@KPTRXHST", SqlDbType.TinyInt, 0, "KPTRXHST")
                da.InsertCommand.Parameters.Add("@HOLD", SqlDbType.TinyInt, 0, "HOLD")
                da.InsertCommand.Parameters.Add("@PTCSHACF", SqlDbType.SmallInt, 0, "PTCSHACF")
                da.InsertCommand.Parameters.Add("@CREDTLMT", SqlDbType.SmallInt, 0, "CREDTLMT")
                da.InsertCommand.Parameters.Add("@WRITEOFF", SqlDbType.SmallInt, 0, "WRITEOFF")
                da.InsertCommand.Parameters.Add("@MXWOFAMT", SqlDbType.Decimal, 0, "MXWOFAMT")
                da.InsertCommand.Parameters.Add("@SBPPSDED", SqlDbType.TinyInt, 0, "SBPPSDED")
                da.InsertCommand.Parameters.Add("@PPSTAXRT", SqlDbType.SmallInt, 0, "PPSTAXRT")
                da.InsertCommand.Parameters.Add("@DXVARNUM", SqlDbType.Char, 0, "DXVARNUM")
                da.InsertCommand.Parameters.Add("@CRTCOMDT", SqlDbType.DateTime, 0, "CRTCOMDT")
                da.InsertCommand.Parameters.Add("@CRTEXPDT", SqlDbType.DateTime, 0, "CRTEXPDT")
                da.InsertCommand.Parameters.Add("@RTOBUTKN", SqlDbType.TinyInt, 0, "RTOBUTKN")
                da.InsertCommand.Parameters.Add("@XPDTOBLG", SqlDbType.TinyInt, 0, "XPDTOBLG")
                da.InsertCommand.Parameters.Add("@PRSPAYEE", SqlDbType.TinyInt, 0, "PRSPAYEE")
                da.InsertCommand.Parameters.Add("@PMAPINDX", SqlDbType.Int, 0, "PMAPINDX")
                da.InsertCommand.Parameters.Add("@PMCSHIDX", SqlDbType.Int, 0, "PMCSHIDX")
                da.InsertCommand.Parameters.Add("@PMDAVIDX", SqlDbType.Int, 0, "PMDAVIDX")
                da.InsertCommand.Parameters.Add("@PMDTKIDX", SqlDbType.Int, 0, "PMDTKIDX")
                da.InsertCommand.Parameters.Add("@PMFINIDX", SqlDbType.Int, 0, "PMFINIDX")
                da.InsertCommand.Parameters.Add("@PMMSCHIX", SqlDbType.Int, 0, "PMMSCHIX")
                da.InsertCommand.Parameters.Add("@PMFRTIDX", SqlDbType.Int, 0, "PMFRTIDX")
                da.InsertCommand.Parameters.Add("@PMTAXIDX", SqlDbType.Int, 0, "PMTAXIDX")
                da.InsertCommand.Parameters.Add("@PMWRTIDX", SqlDbType.Int, 0, "PMWRTIDX")
                da.InsertCommand.Parameters.Add("@PMPRCHIX", SqlDbType.Int, 0, "PMPRCHIX")
                da.InsertCommand.Parameters.Add("@PMRTNGIX", SqlDbType.Int, 0, "PMRTNGIX")
                da.InsertCommand.Parameters.Add("@PMTDSCIX", SqlDbType.Int, 0, "PMTDSCIX")
                da.InsertCommand.Parameters.Add("@ACPURIDX", SqlDbType.Int, 0, "ACPURIDX")
                da.InsertCommand.Parameters.Add("@PURPVIDX", SqlDbType.Int, 0, "PURPVIDX")
                da.InsertCommand.Parameters.Add("@NOTEINDX", SqlDbType.Decimal, 0, "NOTEINDX")
                da.InsertCommand.Parameters.Add("@CHEKBKID", SqlDbType.Char, 0, "CHEKBKID")
                da.InsertCommand.Parameters.Add("@MODIFDT", SqlDbType.DateTime, 0, "MODIFDT")
                da.InsertCommand.Parameters.Add("@CREATDDT", SqlDbType.DateTime, 0, "CREATDDT")
                da.InsertCommand.Parameters.Add("@RATETPID", SqlDbType.Char, 0, "RATETPID")
                da.InsertCommand.Parameters.Add("@Revalue_Vendor", SqlDbType.TinyInt, 0, "Revalue_Vendor")
                da.InsertCommand.Parameters.Add("@Post_Results_To", SqlDbType.SmallInt, 0, "Post_Results_To")
                da.InsertCommand.Parameters.Add("@FREEONBOARD", SqlDbType.SmallInt, 0, "FREEONBOARD")
                da.InsertCommand.Parameters.Add("@GOVCRPID", SqlDbType.Char, 0, "GOVCRPID")
                da.InsertCommand.Parameters.Add("@GOVINDID", SqlDbType.Char, 0, "GOVINDID")
                da.InsertCommand.Parameters.Add("@DISGRPER", SqlDbType.SmallInt, 0, "DISGRPER")
                da.InsertCommand.Parameters.Add("@DUEGRPER", SqlDbType.SmallInt, 0, "DUEGRPER")
                da.InsertCommand.Parameters.Add("@DOCFMTID", SqlDbType.Char, 0, "DOCFMTID")
                da.InsertCommand.Parameters.Add("@TaxInvRecvd", SqlDbType.TinyInt, 0, "TaxInvRecvd")
                da.InsertCommand.Parameters.Add("@USERLANG", SqlDbType.SmallInt, 0, "USERLANG")
                da.InsertCommand.Parameters.Add("@WithholdingType", SqlDbType.SmallInt, 0, "WithholdingType")
                da.InsertCommand.Parameters.Add("@WithholdingFormType", SqlDbType.SmallInt, 0, "WithholdingFormType")
                da.InsertCommand.Parameters.Add("@WithholdingEntityType", SqlDbType.SmallInt, 0, "WithholdingEntityType")
                da.InsertCommand.Parameters.Add("@TaxFileNumMode", SqlDbType.SmallInt, 0, "TaxFileNumMode")
                da.InsertCommand.Parameters.Add("@BRTHDATE", SqlDbType.DateTime, 0, "BRTHDATE")
                da.InsertCommand.Parameters.Add("@LaborPmtType", SqlDbType.SmallInt, 0, "LaborPmtType")
                da.InsertCommand.Parameters.Add("@CCode", SqlDbType.Char, 0, "CCode")
                da.InsertCommand.Parameters.Add("@DECLID", SqlDbType.Char, 0, "DECLID")
                da.InsertCommand.Parameters.Add("@CBVAT", SqlDbType.TinyInt, 0, "CBVAT")
                da.InsertCommand.Parameters.Add("@Workflow_Approval_Status", SqlDbType.SmallInt, 0, "Workflow_Approval_Status")
                da.InsertCommand.Parameters.Add("@Workflow_Priority", SqlDbType.SmallInt, 0, "Workflow_Priority")
                da.InsertCommand.Parameters.Add("@DEX_ROW_TS", SqlDbType.DateTime, 0, "DEX_ROW_TS")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@DEX_ROW_ID", SqlDbType.Int, 0, "DEX_ROW_ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PM00200").NewRow
            End If
            Update_Field("VENDORID", _VENDORID, dr)
            Update_Field("VENDNAME", _VENDNAME, dr)
            Update_Field("VNDCHKNM", _VNDCHKNM, dr)
            Update_Field("VENDSHNM", _VENDSHNM, dr)
            Update_Field("VADDCDPR", _VADDCDPR, dr)
            Update_Field("VADCDPAD", _VADCDPAD, dr)
            Update_Field("VADCDSFR", _VADCDSFR, dr)
            Update_Field("VADCDTRO", _VADCDTRO, dr)
            Update_Field("VNDCLSID", _VNDCLSID, dr)
            Update_Field("VNDCNTCT", _VNDCNTCT, dr)
            Update_Field("ADDRESS1", _ADDRESS1, dr)
            Update_Field("ADDRESS2", _ADDRESS2, dr)
            Update_Field("ADDRESS3", _ADDRESS3, dr)
            Update_Field("CITY", _CITY, dr)
            Update_Field("STATE", _STATE, dr)
            Update_Field("ZIPCODE", _ZIPCODE, dr)
            Update_Field("COUNTRY", _COUNTRY, dr)
            Update_Field("PHNUMBR1", _PHNUMBR1, dr)
            Update_Field("PHNUMBR2", _PHNUMBR2, dr)
            Update_Field("PHONE3", _PHONE3, dr)
            Update_Field("FAXNUMBR", _FAXNUMBR, dr)
            Update_Field("UPSZONE", _UPSZONE, dr)
            Update_Field("SHIPMTHD", _SHIPMTHD, dr)
            Update_Field("TAXSCHID", _TAXSCHID, dr)
            Update_Field("ACNMVNDR", _ACNMVNDR, dr)
            Update_Field("TXIDNMBR", _TXIDNMBR, dr)
            Update_Field("VENDSTTS", _VENDSTTS, dr)
            Update_Field("CURNCYID", _CURNCYID, dr)
            Update_Field("TXRGNNUM", _TXRGNNUM, dr)
            Update_Field("PARVENID", _PARVENID, dr)
            Update_Field("TRDDISCT", _TRDDISCT, dr)
            Update_Field("TEN99TYPE", _TEN99TYPE, dr)
            Update_Field("TEN99BOXNUMBER", _TEN99BOXNUMBER, dr)
            Update_Field("MINORDER", _MINORDER, dr)
            Update_Field("PYMTRMID", _PYMTRMID, dr)
            Update_Field("MINPYTYP", _MINPYTYP, dr)
            Update_Field("MINPYPCT", _MINPYPCT, dr)
            Update_Field("MINPYDLR", _MINPYDLR, dr)
            Update_Field("MXIAFVND", _MXIAFVND, dr)
            Update_Field("MAXINDLR", _MAXINDLR, dr)
            Update_Field("COMMENT1", _COMMENT1, dr)
            Update_Field("COMMENT2", _COMMENT2, dr)
            Update_Field("USERDEF1", _USERDEF1, dr)
            Update_Field("USERDEF2", _USERDEF2, dr)
            Update_Field("CRLMTDLR", _CRLMTDLR, dr)
            Update_Field("PYMNTPRI", _PYMNTPRI, dr)
            Update_Field("KPCALHST", _KPCALHST, dr)
            Update_Field("KGLDSTHS", _KGLDSTHS, dr)
            Update_Field("KPERHIST", _KPERHIST, dr)
            Update_Field("KPTRXHST", _KPTRXHST, dr)
            Update_Field("HOLD", _HOLD, dr)
            Update_Field("PTCSHACF", _PTCSHACF, dr)
            Update_Field("CREDTLMT", _CREDTLMT, dr)
            Update_Field("WRITEOFF", _WRITEOFF, dr)
            Update_Field("MXWOFAMT", _MXWOFAMT, dr)
            Update_Field("SBPPSDED", _SBPPSDED, dr)
            Update_Field("PPSTAXRT", _PPSTAXRT, dr)
            Update_Field("DXVARNUM", _DXVARNUM, dr)
            Update_Field("CRTCOMDT", _CRTCOMDT, dr)
            Update_Field("CRTEXPDT", _CRTEXPDT, dr)
            Update_Field("RTOBUTKN", _RTOBUTKN, dr)
            Update_Field("XPDTOBLG", _XPDTOBLG, dr)
            Update_Field("PRSPAYEE", _PRSPAYEE, dr)
            Update_Field("PMAPINDX", _PMAPINDX, dr)
            Update_Field("PMCSHIDX", _PMCSHIDX, dr)
            Update_Field("PMDAVIDX", _PMDAVIDX, dr)
            Update_Field("PMDTKIDX", _PMDTKIDX, dr)
            Update_Field("PMFINIDX", _PMFINIDX, dr)
            Update_Field("PMMSCHIX", _PMMSCHIX, dr)
            Update_Field("PMFRTIDX", _PMFRTIDX, dr)
            Update_Field("PMTAXIDX", _PMTAXIDX, dr)
            Update_Field("PMWRTIDX", _PMWRTIDX, dr)
            Update_Field("PMPRCHIX", _PMPRCHIX, dr)
            Update_Field("PMRTNGIX", _PMRTNGIX, dr)
            Update_Field("PMTDSCIX", _PMTDSCIX, dr)
            Update_Field("ACPURIDX", _ACPURIDX, dr)
            Update_Field("PURPVIDX", _PURPVIDX, dr)
            Update_Field("NOTEINDX", _NOTEINDX, dr)
            Update_Field("CHEKBKID", _CHEKBKID, dr)
            Update_Field("MODIFDT", _MODIFDT, dr)
            Update_Field("CREATDDT", _CREATDDT, dr)
            Update_Field("RATETPID", _RATETPID, dr)
            Update_Field("Revalue_Vendor", _Revalue_Vendor, dr)
            Update_Field("Post_Results_To", _Post_Results_To, dr)
            Update_Field("FREEONBOARD", _FREEONBOARD, dr)
            Update_Field("GOVCRPID", _GOVCRPID, dr)
            Update_Field("GOVINDID", _GOVINDID, dr)
            Update_Field("DISGRPER", _DISGRPER, dr)
            Update_Field("DUEGRPER", _DUEGRPER, dr)
            Update_Field("DOCFMTID", _DOCFMTID, dr)
            Update_Field("TaxInvRecvd", _TaxInvRecvd, dr)
            Update_Field("USERLANG", _USERLANG, dr)
            Update_Field("WithholdingType", _WithholdingType, dr)
            Update_Field("WithholdingFormType", _WithholdingFormType, dr)
            Update_Field("WithholdingEntityType", _WithholdingEntityType, dr)
            Update_Field("TaxFileNumMode", _TaxFileNumMode, dr)
            Update_Field("BRTHDATE", _BRTHDATE, dr)
            Update_Field("LaborPmtType", _LaborPmtType, dr)
            Update_Field("CCode", _CCode, dr)
            Update_Field("DECLID", _DECLID, dr)
            Update_Field("CBVAT", _CBVAT, dr)
            Update_Field("Workflow_Approval_Status", _Workflow_Approval_Status, dr)
            Update_Field("Workflow_Priority", _Workflow_Priority, dr)
            Update_Field("DEX_ROW_TS", _DEX_ROW_TS, dr)
            If ds.Tables("t_PM00200").Rows.Count < 1 Then ds.Tables("t_PM00200").Rows.Add(dr)
            da.Update(ds, "t_PM00200")
            _ID = ds.Tables("t_PM00200").Rows(0).Item("DEX_ROW_ID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
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
            oEvents.KeyField = "GPVENDORID"
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

    Public Function List_Vendors(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If filter = "" Then
            ds.SelectCommand = "Select DEX_ROW_ID as ID, VENDORID, VENDNAME as Vendor from t_PM00200 order by VENDORID asc"
        Else
            ds.SelectCommand = "Select DEX_ROW_ID as ID, VENDORID, VENDNAME as Vendor from t_PM00200 where VENDORID like '" & filter & "%'"
        End If
        Return ds
    End Function

    Public Function Get_States() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select Distinct State from t_PM00200 order by State asc"
        Return ds
    End Function

    Public Property VENDNAME() As String
        Get
            Return _VENDNAME
        End Get
        Set(ByVal value As String)
            _VENDNAME = value
        End Set
    End Property

    Public Property VNDCHKNM() As String
        Get
            Return _VNDCHKNM
        End Get
        Set(ByVal value As String)
            _VNDCHKNM = value
        End Set
    End Property

    Public Property VENDSHNM() As String
        Get
            Return _VENDSHNM
        End Get
        Set(ByVal value As String)
            _VENDSHNM = value
        End Set
    End Property

    Public Property VADDCDPR() As String
        Get
            Return _VADDCDPR
        End Get
        Set(ByVal value As String)
            _VADDCDPR = value
        End Set
    End Property

    Public Property VADCDPAD() As String
        Get
            Return _VADCDPAD
        End Get
        Set(ByVal value As String)
            _VADCDPAD = value
        End Set
    End Property

    Public Property VADCDSFR() As String
        Get
            Return _VADCDSFR
        End Get
        Set(ByVal value As String)
            _VADCDSFR = value
        End Set
    End Property

    Public Property VADCDTRO() As String
        Get
            Return _VADCDTRO
        End Get
        Set(ByVal value As String)
            _VADCDTRO = value
        End Set
    End Property

    Public Property VNDCLSID() As String
        Get
            Return _VNDCLSID
        End Get
        Set(ByVal value As String)
            _VNDCLSID = value
        End Set
    End Property

    Public Property VNDCNTCT() As String
        Get
            Return _VNDCNTCT
        End Get
        Set(ByVal value As String)
            _VNDCNTCT = value
        End Set
    End Property

    Public Property ADDRESS1() As String
        Get
            Return _ADDRESS1
        End Get
        Set(ByVal value As String)
            _ADDRESS1 = value
        End Set
    End Property

    Public Property ADDRESS2() As String
        Get
            Return _ADDRESS2
        End Get
        Set(ByVal value As String)
            _ADDRESS2 = value
        End Set
    End Property

    Public Property ADDRESS3() As String
        Get
            Return _ADDRESS3
        End Get
        Set(ByVal value As String)
            _ADDRESS3 = value
        End Set
    End Property

    Public Property CITY() As String
        Get
            Return _CITY
        End Get
        Set(ByVal value As String)
            _CITY = value
        End Set
    End Property

    Public Property STATE() As String
        Get
            Return _STATE
        End Get
        Set(ByVal value As String)
            _STATE = value
        End Set
    End Property

    Public Property ZIPCODE() As String
        Get
            Return _ZIPCODE
        End Get
        Set(ByVal value As String)
            _ZIPCODE = value
        End Set
    End Property

    Public Property COUNTRY() As String
        Get
            Return _COUNTRY
        End Get
        Set(ByVal value As String)
            _COUNTRY = value
        End Set
    End Property

    Public Property PHNUMBR1() As String
        Get
            Return _PHNUMBR1
        End Get
        Set(ByVal value As String)
            _PHNUMBR1 = value
        End Set
    End Property

    Public Property PHNUMBR2() As String
        Get
            Return _PHNUMBR2
        End Get
        Set(ByVal value As String)
            _PHNUMBR2 = value
        End Set
    End Property

    Public Property PHONE3() As String
        Get
            Return _PHONE3
        End Get
        Set(ByVal value As String)
            _PHONE3 = value
        End Set
    End Property

    Public Property FAXNUMBR() As String
        Get
            Return _FAXNUMBR
        End Get
        Set(ByVal value As String)
            _FAXNUMBR = value
        End Set
    End Property

    Public Property UPSZONE() As String
        Get
            Return _UPSZONE
        End Get
        Set(ByVal value As String)
            _UPSZONE = value
        End Set
    End Property

    Public Property SHIPMTHD() As String
        Get
            Return _SHIPMTHD
        End Get
        Set(ByVal value As String)
            _SHIPMTHD = value
        End Set
    End Property

    Public Property TAXSCHID() As String
        Get
            Return _TAXSCHID
        End Get
        Set(ByVal value As String)
            _TAXSCHID = value
        End Set
    End Property

    Public Property ACNMVNDR() As String
        Get
            Return _ACNMVNDR
        End Get
        Set(ByVal value As String)
            _ACNMVNDR = value
        End Set
    End Property

    Public Property TXIDNMBR() As String
        Get
            Return _TXIDNMBR
        End Get
        Set(ByVal value As String)
            _TXIDNMBR = value
        End Set
    End Property

    Public Property VENDSTTS() As Integer
        Get
            Return _VENDSTTS
        End Get
        Set(ByVal value As Integer)
            _VENDSTTS = value
        End Set
    End Property

    Public Property CURNCYID() As String
        Get
            Return _CURNCYID
        End Get
        Set(ByVal value As String)
            _CURNCYID = value
        End Set
    End Property

    Public Property TXRGNNUM() As String
        Get
            Return _TXRGNNUM
        End Get
        Set(ByVal value As String)
            _TXRGNNUM = value
        End Set
    End Property

    Public Property PARVENID() As String
        Get
            Return _PARVENID
        End Get
        Set(ByVal value As String)
            _PARVENID = value
        End Set
    End Property

    Public Property TRDDISCT() As Integer
        Get
            Return _TRDDISCT
        End Get
        Set(ByVal value As Integer)
            _TRDDISCT = value
        End Set
    End Property

    Public Property TEN99TYPE() As Integer
        Get
            Return _TEN99TYPE
        End Get
        Set(ByVal value As Integer)
            _TEN99TYPE = value
        End Set
    End Property

    Public Property TEN99BOXNUMBER() As Integer
        Get
            Return _TEN99BOXNUMBER
        End Get
        Set(ByVal value As Integer)
            _TEN99BOXNUMBER = value
        End Set
    End Property

    Public Property MINORDER() As Decimal
        Get
            Return _MINORDER
        End Get
        Set(ByVal value As Decimal)
            _MINORDER = value
        End Set
    End Property

    Public Property PYMTRMID() As String
        Get
            Return _PYMTRMID
        End Get
        Set(ByVal value As String)
            _PYMTRMID = value
        End Set
    End Property

    Public Property MINPYTYP() As Integer
        Get
            Return _MINPYTYP
        End Get
        Set(ByVal value As Integer)
            _MINPYTYP = value
        End Set
    End Property

    Public Property MINPYPCT() As Integer
        Get
            Return _MINPYPCT
        End Get
        Set(ByVal value As Integer)
            _MINPYPCT = value
        End Set
    End Property

    Public Property MINPYDLR() As Decimal
        Get
            Return _MINPYDLR
        End Get
        Set(ByVal value As Decimal)
            _MINPYDLR = value
        End Set
    End Property

    Public Property MXIAFVND() As Integer
        Get
            Return _MXIAFVND
        End Get
        Set(ByVal value As Integer)
            _MXIAFVND = value
        End Set
    End Property

    Public Property MAXINDLR() As Decimal
        Get
            Return _MAXINDLR
        End Get
        Set(ByVal value As Decimal)
            _MAXINDLR = value
        End Set
    End Property

    Public Property COMMENT1() As String
        Get
            Return _COMMENT1
        End Get
        Set(ByVal value As String)
            _COMMENT1 = value
        End Set
    End Property

    Public Property COMMENT2() As String
        Get
            Return _COMMENT2
        End Get
        Set(ByVal value As String)
            _COMMENT2 = value
        End Set
    End Property

    Public Property USERDEF1() As String
        Get
            Return _USERDEF1
        End Get
        Set(ByVal value As String)
            _USERDEF1 = value
        End Set
    End Property

    Public Property USERDEF2() As String
        Get
            Return _USERDEF2
        End Get
        Set(ByVal value As String)
            _USERDEF2 = value
        End Set
    End Property

    Public Property CRLMTDLR() As Decimal
        Get
            Return _CRLMTDLR
        End Get
        Set(ByVal value As Decimal)
            _CRLMTDLR = value
        End Set
    End Property

    Public Property PYMNTPRI() As String
        Get
            Return _PYMNTPRI
        End Get
        Set(ByVal value As String)
            _PYMNTPRI = value
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

    Public Property KGLDSTHS() As Integer
        Get
            Return _KGLDSTHS
        End Get
        Set(ByVal value As Integer)
            _KGLDSTHS = value
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

    Public Property HOLD() As Integer
        Get
            Return _HOLD
        End Get
        Set(ByVal value As Integer)
            _HOLD = value
        End Set
    End Property

    Public Property PTCSHACF() As Integer
        Get
            Return _PTCSHACF
        End Get
        Set(ByVal value As Integer)
            _PTCSHACF = value
        End Set
    End Property

    Public Property CREDTLMT() As Integer
        Get
            Return _CREDTLMT
        End Get
        Set(ByVal value As Integer)
            _CREDTLMT = value
        End Set
    End Property

    Public Property WRITEOFF() As Integer
        Get
            Return _WRITEOFF
        End Get
        Set(ByVal value As Integer)
            _WRITEOFF = value
        End Set
    End Property

    Public Property MXWOFAMT() As Decimal
        Get
            Return _MXWOFAMT
        End Get
        Set(ByVal value As Decimal)
            _MXWOFAMT = value
        End Set
    End Property

    Public Property SBPPSDED() As Integer
        Get
            Return _SBPPSDED
        End Get
        Set(ByVal value As Integer)
            _SBPPSDED = value
        End Set
    End Property

    Public Property PPSTAXRT() As Integer
        Get
            Return _PPSTAXRT
        End Get
        Set(ByVal value As Integer)
            _PPSTAXRT = value
        End Set
    End Property

    Public Property DXVARNUM() As String
        Get
            Return _DXVARNUM
        End Get
        Set(ByVal value As String)
            _DXVARNUM = value
        End Set
    End Property

    Public Property CRTCOMDT() As String
        Get
            Return _CRTCOMDT
        End Get
        Set(ByVal value As String)
            _CRTCOMDT = value
        End Set
    End Property

    Public Property CRTEXPDT() As String
        Get
            Return _CRTEXPDT
        End Get
        Set(ByVal value As String)
            _CRTEXPDT = value
        End Set
    End Property

    Public Property RTOBUTKN() As Integer
        Get
            Return _RTOBUTKN
        End Get
        Set(ByVal value As Integer)
            _RTOBUTKN = value
        End Set
    End Property

    Public Property XPDTOBLG() As Integer
        Get
            Return _XPDTOBLG
        End Get
        Set(ByVal value As Integer)
            _XPDTOBLG = value
        End Set
    End Property

    Public Property PRSPAYEE() As Integer
        Get
            Return _PRSPAYEE
        End Get
        Set(ByVal value As Integer)
            _PRSPAYEE = value
        End Set
    End Property

    Public Property PMAPINDX() As Integer
        Get
            Return _PMAPINDX
        End Get
        Set(ByVal value As Integer)
            _PMAPINDX = value
        End Set
    End Property

    Public Property PMCSHIDX() As Integer
        Get
            Return _PMCSHIDX
        End Get
        Set(ByVal value As Integer)
            _PMCSHIDX = value
        End Set
    End Property

    Public Property PMDAVIDX() As Integer
        Get
            Return _PMDAVIDX
        End Get
        Set(ByVal value As Integer)
            _PMDAVIDX = value
        End Set
    End Property

    Public Property PMDTKIDX() As Integer
        Get
            Return _PMDTKIDX
        End Get
        Set(ByVal value As Integer)
            _PMDTKIDX = value
        End Set
    End Property

    Public Property PMFINIDX() As Integer
        Get
            Return _PMFINIDX
        End Get
        Set(ByVal value As Integer)
            _PMFINIDX = value
        End Set
    End Property

    Public Property PMMSCHIX() As Integer
        Get
            Return _PMMSCHIX
        End Get
        Set(ByVal value As Integer)
            _PMMSCHIX = value
        End Set
    End Property

    Public Property PMFRTIDX() As Integer
        Get
            Return _PMFRTIDX
        End Get
        Set(ByVal value As Integer)
            _PMFRTIDX = value
        End Set
    End Property

    Public Property PMTAXIDX() As Integer
        Get
            Return _PMTAXIDX
        End Get
        Set(ByVal value As Integer)
            _PMTAXIDX = value
        End Set
    End Property

    Public Property PMWRTIDX() As Integer
        Get
            Return _PMWRTIDX
        End Get
        Set(ByVal value As Integer)
            _PMWRTIDX = value
        End Set
    End Property

    Public Property PMPRCHIX() As Integer
        Get
            Return _PMPRCHIX
        End Get
        Set(ByVal value As Integer)
            _PMPRCHIX = value
        End Set
    End Property

    Public Property PMRTNGIX() As Integer
        Get
            Return _PMRTNGIX
        End Get
        Set(ByVal value As Integer)
            _PMRTNGIX = value
        End Set
    End Property

    Public Property PMTDSCIX() As Integer
        Get
            Return _PMTDSCIX
        End Get
        Set(ByVal value As Integer)
            _PMTDSCIX = value
        End Set
    End Property

    Public Property ACPURIDX() As Integer
        Get
            Return _ACPURIDX
        End Get
        Set(ByVal value As Integer)
            _ACPURIDX = value
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

    Public Property NOTEINDX() As Decimal
        Get
            Return _NOTEINDX
        End Get
        Set(ByVal value As Decimal)
            _NOTEINDX = value
        End Set
    End Property

    Public Property CHEKBKID() As String
        Get
            Return _CHEKBKID
        End Get
        Set(ByVal value As String)
            _CHEKBKID = value
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

    Public Property RATETPID() As String
        Get
            Return _RATETPID
        End Get
        Set(ByVal value As String)
            _RATETPID = value
        End Set
    End Property

    Public Property Revalue_Vendor() As Integer
        Get
            Return _Revalue_Vendor
        End Get
        Set(ByVal value As Integer)
            _Revalue_Vendor = value
        End Set
    End Property

    Public Property Post_Results_To() As Integer
        Get
            Return _Post_Results_To
        End Get
        Set(ByVal value As Integer)
            _Post_Results_To = value
        End Set
    End Property

    Public Property FREEONBOARD() As Integer
        Get
            Return _FREEONBOARD
        End Get
        Set(ByVal value As Integer)
            _FREEONBOARD = value
        End Set
    End Property

    Public Property GOVCRPID() As String
        Get
            Return _GOVCRPID
        End Get
        Set(ByVal value As String)
            _GOVCRPID = value
        End Set
    End Property

    Public Property GOVINDID() As String
        Get
            Return _GOVINDID
        End Get
        Set(ByVal value As String)
            _GOVINDID = value
        End Set
    End Property

    Public Property DISGRPER() As Integer
        Get
            Return _DISGRPER
        End Get
        Set(ByVal value As Integer)
            _DISGRPER = value
        End Set
    End Property

    Public Property DUEGRPER() As Integer
        Get
            Return _DUEGRPER
        End Get
        Set(ByVal value As Integer)
            _DUEGRPER = value
        End Set
    End Property

    Public Property DOCFMTID() As String
        Get
            Return _DOCFMTID
        End Get
        Set(ByVal value As String)
            _DOCFMTID = value
        End Set
    End Property

    Public Property TaxInvRecvd() As Integer
        Get
            Return _TaxInvRecvd
        End Get
        Set(ByVal value As Integer)
            _TaxInvRecvd = value
        End Set
    End Property

    Public Property USERLANG() As Integer
        Get
            Return _USERLANG
        End Get
        Set(ByVal value As Integer)
            _USERLANG = value
        End Set
    End Property

    Public Property WithholdingType() As Integer
        Get
            Return _WithholdingType
        End Get
        Set(ByVal value As Integer)
            _WithholdingType = value
        End Set
    End Property

    Public Property WithholdingFormType() As Integer
        Get
            Return _WithholdingFormType
        End Get
        Set(ByVal value As Integer)
            _WithholdingFormType = value
        End Set
    End Property

    Public Property WithholdingEntityType() As Integer
        Get
            Return _WithholdingEntityType
        End Get
        Set(ByVal value As Integer)
            _WithholdingEntityType = value
        End Set
    End Property

    Public Property TaxFileNumMode() As Integer
        Get
            Return _TaxFileNumMode
        End Get
        Set(ByVal value As Integer)
            _TaxFileNumMode = value
        End Set
    End Property

    Public Property BRTHDATE() As String
        Get
            Return _BRTHDATE
        End Get
        Set(ByVal value As String)
            _BRTHDATE = value
        End Set
    End Property

    Public Property LaborPmtType() As Integer
        Get
            Return _LaborPmtType
        End Get
        Set(ByVal value As Integer)
            _LaborPmtType = value
        End Set
    End Property

    Public Property CCode() As String
        Get
            Return _CCode
        End Get
        Set(ByVal value As String)
            _CCode = value
        End Set
    End Property

    Public Property DECLID() As String
        Get
            Return _DECLID
        End Get
        Set(ByVal value As String)
            _DECLID = value
        End Set
    End Property

    Public Property CBVAT() As Integer
        Get
            Return _CBVAT
        End Get
        Set(ByVal value As Integer)
            _CBVAT = value
        End Set
    End Property

    Public Property Workflow_Approval_Status() As Integer
        Get
            Return _Workflow_Approval_Status
        End Get
        Set(ByVal value As Integer)
            _Workflow_Approval_Status = value
        End Set
    End Property

    Public Property Workflow_Priority() As Integer
        Get
            Return _Workflow_Priority
        End Get
        Set(ByVal value As Integer)
            _Workflow_Priority = value
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

    Public Property VENDORID() As String
        Get
            Return _VENDORID
        End Get
        Set(ByVal value As String)
            _VENDORID = value
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
