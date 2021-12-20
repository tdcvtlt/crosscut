Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Windows.Forms
Imports System.Data.OleDb
Imports System.ComponentModel

Public Class clsIV00102
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ITEMNMBR As String = ""
    Dim _LOCNCODE As String = ""
    Dim _BINNMBR As String = ""
    Dim _RCRDTYPE As Integer = 0
    Dim _PRIMVNDR As String = ""
    Dim _ITMFRFLG As Integer = 0
    Dim _BGNGQTY As Decimal = 0.0
    Dim _LSORDQTY As Decimal = 0.0
    Dim _LRCPTQTY As Decimal = 0.0
    Dim _LSTORDDT As String = ""
    Dim _LSORDVND As String = ""
    Dim _LSRCPTDT As String = ""
    Dim _QTYRQSTN As Decimal = 0.0
    Dim _QTYONORD As Decimal = 0.0
    Dim _QTYBKORD As Decimal = 0.0
    Dim _QTY_Drop_Shipped As Decimal = 0.0
    Dim _QTYINUSE As Decimal = 0.0
    Dim _QTYINSVC As Decimal = 0.0
    Dim _QTYRTRND As Decimal = 0.0
    Dim _QTYDMGED As Decimal = 0.0
    Dim _QTYONHND As Decimal = 0.0
    Dim _ATYALLOC As Decimal = 0.0
    Dim _QTYCOMTD As Decimal = 0.0
    Dim _QTYSOLD As Decimal = 0.0
    Dim _NXTCNTDT As String = ""
    Dim _NXTCNTTM As String = ""
    Dim _LSTCNTDT As String = ""
    Dim _LSTCNTTM As String = ""
    Dim _STCKCNTINTRVL As Integer = 0
    Dim _Landed_Cost_Group_ID As String = ""
    Dim _BUYERID As String = ""
    Dim _PLANNERID As String = ""
    Dim _ORDERPOLICY As Integer = 0
    Dim _FXDORDRQTY As Decimal = 0.0
    Dim _ORDRPNTQTY As Decimal = 0.0
    Dim _NMBROFDYS As Integer = 0
    Dim _MNMMORDRQTY As Decimal = 0.0
    Dim _MXMMORDRQTY As Decimal = 0.0
    Dim _ORDERMULTIPLE As Decimal = 0.0
    Dim _REPLENISHMENTMETHOD As Integer = 0
    Dim _SHRINKAGEFACTOR As Decimal = 0.0
    Dim _PRCHSNGLDTM As Decimal = 0.0
    Dim _MNFCTRNGFXDLDTM As Decimal = 0.0
    Dim _MNFCTRNGVRBLLDTM As Decimal = 0.0
    Dim _STAGINGLDTME As Decimal = 0.0
    Dim _PLNNNGTMFNCDYS As Integer = 0
    Dim _DMNDTMFNCPRDS As Integer = 0
    Dim _INCLDDINPLNNNG As Integer = 0
    Dim _CALCULATEATP As Integer = 0
    Dim _AUTOCHKATP As Integer = 0
    Dim _PLNFNLPAB As Integer = 0
    Dim _FRCSTCNSMPTNPRD As Integer = 0
    Dim _ORDRUPTOLVL As Decimal = 0.0
    Dim _SFTYSTCKQTY As Decimal = 0.0
    Dim _REORDERVARIANCE As Decimal = 0.0
    Dim _PORECEIPTBIN As String = ""
    Dim _PORETRNBIN As String = ""
    Dim _SOFULFILLMENTBIN As String = ""
    Dim _SORETURNBIN As String = ""
    Dim _BOMRCPTBIN As String = ""
    Dim _MATERIALISSUEBIN As String = ""
    Dim _MORECEIPTBIN As String = ""
    Dim _REPAIRISSUESBIN As String = ""
    Dim _ReplenishmentLevel As Integer = 0
    Dim _POPOrderMethod As Integer = 0
    Dim _MasterLocationCode As String = ""
    Dim _POPVendorSelection As Integer = 0
    Dim _POPPricingSelection As Integer = 0
    Dim _PurchasePrice As Decimal = 0.0
    Dim _IncludeAllocations As Integer = 0
    Dim _IncludeBackorders As Integer = 0
    Dim _IncludeRequisitions As Integer = 0
    Dim _PICKTICKETITEMOPT As Integer = 0
    Dim _INCLDMRPMOVEIN As Integer = 0
    Dim _INCLDMRPMOVEOUT As Integer = 0
    Dim _INCLDMRPCANCEL As Integer = 0
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
        cm = New SqlCommand("Select * from t_IV00102 where DEX_ROW_ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_IV00102 where DEX_ROW_ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_IV00102")
            If ds.Tables("t_IV00102").Rows.Count > 0 Then
                dr = ds.Tables("t_IV00102").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ITEMNMBR") Is System.DBNull.Value) Then _ITEMNMBR = dr("ITEMNMBR")
        If Not (dr("LOCNCODE") Is System.DBNull.Value) Then _LOCNCODE = dr("LOCNCODE")
        If Not (dr("BINNMBR") Is System.DBNull.Value) Then _BINNMBR = dr("BINNMBR")
        If Not (dr("RCRDTYPE") Is System.DBNull.Value) Then _RCRDTYPE = dr("RCRDTYPE")
        If Not (dr("PRIMVNDR") Is System.DBNull.Value) Then _PRIMVNDR = dr("PRIMVNDR")
        If Not (dr("ITMFRFLG") Is System.DBNull.Value) Then _ITMFRFLG = dr("ITMFRFLG")
        If Not (dr("BGNGQTY") Is System.DBNull.Value) Then _BGNGQTY = dr("BGNGQTY")
        If Not (dr("LSORDQTY") Is System.DBNull.Value) Then _LSORDQTY = dr("LSORDQTY")
        If Not (dr("LRCPTQTY") Is System.DBNull.Value) Then _LRCPTQTY = dr("LRCPTQTY")
        If Not (dr("LSTORDDT") Is System.DBNull.Value) Then _LSTORDDT = dr("LSTORDDT")
        If Not (dr("LSORDVND") Is System.DBNull.Value) Then _LSORDVND = dr("LSORDVND")
        If Not (dr("LSRCPTDT") Is System.DBNull.Value) Then _LSRCPTDT = dr("LSRCPTDT")
        If Not (dr("QTYRQSTN") Is System.DBNull.Value) Then _QTYRQSTN = dr("QTYRQSTN")
        If Not (dr("QTYONORD") Is System.DBNull.Value) Then _QTYONORD = dr("QTYONORD")
        If Not (dr("QTYBKORD") Is System.DBNull.Value) Then _QTYBKORD = dr("QTYBKORD")
        If Not (dr("QTY_Drop_Shipped") Is System.DBNull.Value) Then _QTY_Drop_Shipped = dr("QTY_Drop_Shipped")
        If Not (dr("QTYINUSE") Is System.DBNull.Value) Then _QTYINUSE = dr("QTYINUSE")
        If Not (dr("QTYINSVC") Is System.DBNull.Value) Then _QTYINSVC = dr("QTYINSVC")
        If Not (dr("QTYRTRND") Is System.DBNull.Value) Then _QTYRTRND = dr("QTYRTRND")
        If Not (dr("QTYDMGED") Is System.DBNull.Value) Then _QTYDMGED = dr("QTYDMGED")
        If Not (dr("QTYONHND") Is System.DBNull.Value) Then _QTYONHND = dr("QTYONHND")
        If Not (dr("ATYALLOC") Is System.DBNull.Value) Then _ATYALLOC = dr("ATYALLOC")
        If Not (dr("QTYCOMTD") Is System.DBNull.Value) Then _QTYCOMTD = dr("QTYCOMTD")
        If Not (dr("QTYSOLD") Is System.DBNull.Value) Then _QTYSOLD = dr("QTYSOLD")
        If Not (dr("NXTCNTDT") Is System.DBNull.Value) Then _NXTCNTDT = dr("NXTCNTDT")
        If Not (dr("NXTCNTTM") Is System.DBNull.Value) Then _NXTCNTTM = dr("NXTCNTTM")
        If Not (dr("LSTCNTDT") Is System.DBNull.Value) Then _LSTCNTDT = dr("LSTCNTDT")
        If Not (dr("LSTCNTTM") Is System.DBNull.Value) Then _LSTCNTTM = dr("LSTCNTTM")
        If Not (dr("STCKCNTINTRVL") Is System.DBNull.Value) Then _STCKCNTINTRVL = dr("STCKCNTINTRVL")
        If Not (dr("Landed_Cost_Group_ID") Is System.DBNull.Value) Then _Landed_Cost_Group_ID = dr("Landed_Cost_Group_ID")
        If Not (dr("BUYERID") Is System.DBNull.Value) Then _BUYERID = dr("BUYERID")
        If Not (dr("PLANNERID") Is System.DBNull.Value) Then _PLANNERID = dr("PLANNERID")
        If Not (dr("ORDERPOLICY") Is System.DBNull.Value) Then _ORDERPOLICY = dr("ORDERPOLICY")
        If Not (dr("FXDORDRQTY") Is System.DBNull.Value) Then _FXDORDRQTY = dr("FXDORDRQTY")
        If Not (dr("ORDRPNTQTY") Is System.DBNull.Value) Then _ORDRPNTQTY = dr("ORDRPNTQTY")
        If Not (dr("NMBROFDYS") Is System.DBNull.Value) Then _NMBROFDYS = dr("NMBROFDYS")
        If Not (dr("MNMMORDRQTY") Is System.DBNull.Value) Then _MNMMORDRQTY = dr("MNMMORDRQTY")
        If Not (dr("MXMMORDRQTY") Is System.DBNull.Value) Then _MXMMORDRQTY = dr("MXMMORDRQTY")
        If Not (dr("ORDERMULTIPLE") Is System.DBNull.Value) Then _ORDERMULTIPLE = dr("ORDERMULTIPLE")
        If Not (dr("REPLENISHMENTMETHOD") Is System.DBNull.Value) Then _REPLENISHMENTMETHOD = dr("REPLENISHMENTMETHOD")
        If Not (dr("SHRINKAGEFACTOR") Is System.DBNull.Value) Then _SHRINKAGEFACTOR = dr("SHRINKAGEFACTOR")
        If Not (dr("PRCHSNGLDTM") Is System.DBNull.Value) Then _PRCHSNGLDTM = dr("PRCHSNGLDTM")
        If Not (dr("MNFCTRNGFXDLDTM") Is System.DBNull.Value) Then _MNFCTRNGFXDLDTM = dr("MNFCTRNGFXDLDTM")
        If Not (dr("MNFCTRNGVRBLLDTM") Is System.DBNull.Value) Then _MNFCTRNGVRBLLDTM = dr("MNFCTRNGVRBLLDTM")
        If Not (dr("STAGINGLDTME") Is System.DBNull.Value) Then _STAGINGLDTME = dr("STAGINGLDTME")
        If Not (dr("PLNNNGTMFNCDYS") Is System.DBNull.Value) Then _PLNNNGTMFNCDYS = dr("PLNNNGTMFNCDYS")
        If Not (dr("DMNDTMFNCPRDS") Is System.DBNull.Value) Then _DMNDTMFNCPRDS = dr("DMNDTMFNCPRDS")
        If Not (dr("INCLDDINPLNNNG") Is System.DBNull.Value) Then _INCLDDINPLNNNG = dr("INCLDDINPLNNNG")
        If Not (dr("CALCULATEATP") Is System.DBNull.Value) Then _CALCULATEATP = dr("CALCULATEATP")
        If Not (dr("AUTOCHKATP") Is System.DBNull.Value) Then _AUTOCHKATP = dr("AUTOCHKATP")
        If Not (dr("PLNFNLPAB") Is System.DBNull.Value) Then _PLNFNLPAB = dr("PLNFNLPAB")
        If Not (dr("FRCSTCNSMPTNPRD") Is System.DBNull.Value) Then _FRCSTCNSMPTNPRD = dr("FRCSTCNSMPTNPRD")
        If Not (dr("ORDRUPTOLVL") Is System.DBNull.Value) Then _ORDRUPTOLVL = dr("ORDRUPTOLVL")
        If Not (dr("SFTYSTCKQTY") Is System.DBNull.Value) Then _SFTYSTCKQTY = dr("SFTYSTCKQTY")
        If Not (dr("REORDERVARIANCE") Is System.DBNull.Value) Then _REORDERVARIANCE = dr("REORDERVARIANCE")
        If Not (dr("PORECEIPTBIN") Is System.DBNull.Value) Then _PORECEIPTBIN = dr("PORECEIPTBIN")
        If Not (dr("PORETRNBIN") Is System.DBNull.Value) Then _PORETRNBIN = dr("PORETRNBIN")
        If Not (dr("SOFULFILLMENTBIN") Is System.DBNull.Value) Then _SOFULFILLMENTBIN = dr("SOFULFILLMENTBIN")
        If Not (dr("SORETURNBIN") Is System.DBNull.Value) Then _SORETURNBIN = dr("SORETURNBIN")
        If Not (dr("BOMRCPTBIN") Is System.DBNull.Value) Then _BOMRCPTBIN = dr("BOMRCPTBIN")
        If Not (dr("MATERIALISSUEBIN") Is System.DBNull.Value) Then _MATERIALISSUEBIN = dr("MATERIALISSUEBIN")
        If Not (dr("MORECEIPTBIN") Is System.DBNull.Value) Then _MORECEIPTBIN = dr("MORECEIPTBIN")
        If Not (dr("REPAIRISSUESBIN") Is System.DBNull.Value) Then _REPAIRISSUESBIN = dr("REPAIRISSUESBIN")
        If Not (dr("ReplenishmentLevel") Is System.DBNull.Value) Then _ReplenishmentLevel = dr("ReplenishmentLevel")
        If Not (dr("POPOrderMethod") Is System.DBNull.Value) Then _POPOrderMethod = dr("POPOrderMethod")
        If Not (dr("MasterLocationCode") Is System.DBNull.Value) Then _MasterLocationCode = dr("MasterLocationCode")
        If Not (dr("POPVendorSelection") Is System.DBNull.Value) Then _POPVendorSelection = dr("POPVendorSelection")
        If Not (dr("POPPricingSelection") Is System.DBNull.Value) Then _POPPricingSelection = dr("POPPricingSelection")
        If Not (dr("PurchasePrice") Is System.DBNull.Value) Then _PurchasePrice = dr("PurchasePrice")
        If Not (dr("IncludeAllocations") Is System.DBNull.Value) Then _IncludeAllocations = dr("IncludeAllocations")
        If Not (dr("IncludeBackorders") Is System.DBNull.Value) Then _IncludeBackorders = dr("IncludeBackorders")
        If Not (dr("IncludeRequisitions") Is System.DBNull.Value) Then _IncludeRequisitions = dr("IncludeRequisitions")
        If Not (dr("PICKTICKETITEMOPT") Is System.DBNull.Value) Then _PICKTICKETITEMOPT = dr("PICKTICKETITEMOPT")
        If Not (dr("INCLDMRPMOVEIN") Is System.DBNull.Value) Then _INCLDMRPMOVEIN = dr("INCLDMRPMOVEIN")
        If Not (dr("INCLDMRPMOVEOUT") Is System.DBNull.Value) Then _INCLDMRPMOVEOUT = dr("INCLDMRPMOVEOUT")
        If Not (dr("INCLDMRPCANCEL") Is System.DBNull.Value) Then _INCLDMRPCANCEL = dr("INCLDMRPCANCEL")
        'If Not (dr("DEX_ROW_ID") Is System.DBNull.Value) Then _DEX_ROW_ID = dr("DEX_ROW_ID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_IV00102 where DEX_ROW_ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_IV00102")
            If ds.Tables("t_IV00102").Rows.Count > 0 Then
                dr = ds.Tables("t_IV00102").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_IV00102Insert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ITEMNMBR", SqlDbType.Char, 0, "ITEMNMBR")
                da.InsertCommand.Parameters.Add("@LOCNCODE", SqlDbType.Char, 0, "LOCNCODE")
                da.InsertCommand.Parameters.Add("@BINNMBR", SqlDbType.Char, 0, "BINNMBR")
                da.InsertCommand.Parameters.Add("@RCRDTYPE", SqlDbType.SmallInt, 0, "RCRDTYPE")
                da.InsertCommand.Parameters.Add("@PRIMVNDR", SqlDbType.Char, 0, "PRIMVNDR")
                da.InsertCommand.Parameters.Add("@ITMFRFLG", SqlDbType.TinyInt, 0, "ITMFRFLG")
                da.InsertCommand.Parameters.Add("@BGNGQTY", SqlDbType.Decimal, 0, "BGNGQTY")
                da.InsertCommand.Parameters.Add("@LSORDQTY", SqlDbType.Decimal, 0, "LSORDQTY")
                da.InsertCommand.Parameters.Add("@LRCPTQTY", SqlDbType.Decimal, 0, "LRCPTQTY")
                da.InsertCommand.Parameters.Add("@LSTORDDT", SqlDbType.DateTime, 0, "LSTORDDT")
                da.InsertCommand.Parameters.Add("@LSORDVND", SqlDbType.Char, 0, "LSORDVND")
                da.InsertCommand.Parameters.Add("@LSRCPTDT", SqlDbType.DateTime, 0, "LSRCPTDT")
                da.InsertCommand.Parameters.Add("@QTYRQSTN", SqlDbType.Decimal, 0, "QTYRQSTN")
                da.InsertCommand.Parameters.Add("@QTYONORD", SqlDbType.Decimal, 0, "QTYONORD")
                da.InsertCommand.Parameters.Add("@QTYBKORD", SqlDbType.Decimal, 0, "QTYBKORD")
                da.InsertCommand.Parameters.Add("@QTY_Drop_Shipped", SqlDbType.Decimal, 0, "QTY_Drop_Shipped")
                da.InsertCommand.Parameters.Add("@QTYINUSE", SqlDbType.Decimal, 0, "QTYINUSE")
                da.InsertCommand.Parameters.Add("@QTYINSVC", SqlDbType.Decimal, 0, "QTYINSVC")
                da.InsertCommand.Parameters.Add("@QTYRTRND", SqlDbType.Decimal, 0, "QTYRTRND")
                da.InsertCommand.Parameters.Add("@QTYDMGED", SqlDbType.Decimal, 0, "QTYDMGED")
                da.InsertCommand.Parameters.Add("@QTYONHND", SqlDbType.Decimal, 0, "QTYONHND")
                da.InsertCommand.Parameters.Add("@ATYALLOC", SqlDbType.Decimal, 0, "ATYALLOC")
                da.InsertCommand.Parameters.Add("@QTYCOMTD", SqlDbType.Decimal, 0, "QTYCOMTD")
                da.InsertCommand.Parameters.Add("@QTYSOLD", SqlDbType.Decimal, 0, "QTYSOLD")
                da.InsertCommand.Parameters.Add("@NXTCNTDT", SqlDbType.DateTime, 0, "NXTCNTDT")
                da.InsertCommand.Parameters.Add("@NXTCNTTM", SqlDbType.DateTime, 0, "NXTCNTTM")
                da.InsertCommand.Parameters.Add("@LSTCNTDT", SqlDbType.DateTime, 0, "LSTCNTDT")
                da.InsertCommand.Parameters.Add("@LSTCNTTM", SqlDbType.DateTime, 0, "LSTCNTTM")
                da.InsertCommand.Parameters.Add("@STCKCNTINTRVL", SqlDbType.SmallInt, 0, "STCKCNTINTRVL")
                da.InsertCommand.Parameters.Add("@Landed_Cost_Group_ID", SqlDbType.Char, 0, "Landed_Cost_Group_ID")
                da.InsertCommand.Parameters.Add("@BUYERID", SqlDbType.Char, 0, "BUYERID")
                da.InsertCommand.Parameters.Add("@PLANNERID", SqlDbType.Char, 0, "PLANNERID")
                da.InsertCommand.Parameters.Add("@ORDERPOLICY", SqlDbType.SmallInt, 0, "ORDERPOLICY")
                da.InsertCommand.Parameters.Add("@FXDORDRQTY", SqlDbType.Decimal, 0, "FXDORDRQTY")
                da.InsertCommand.Parameters.Add("@ORDRPNTQTY", SqlDbType.Decimal, 0, "ORDRPNTQTY")
                da.InsertCommand.Parameters.Add("@NMBROFDYS", SqlDbType.SmallInt, 0, "NMBROFDYS")
                da.InsertCommand.Parameters.Add("@MNMMORDRQTY", SqlDbType.Decimal, 0, "MNMMORDRQTY")
                da.InsertCommand.Parameters.Add("@MXMMORDRQTY", SqlDbType.Decimal, 0, "MXMMORDRQTY")
                da.InsertCommand.Parameters.Add("@ORDERMULTIPLE", SqlDbType.Decimal, 0, "ORDERMULTIPLE")
                da.InsertCommand.Parameters.Add("@REPLENISHMENTMETHOD", SqlDbType.SmallInt, 0, "REPLENISHMENTMETHOD")
                da.InsertCommand.Parameters.Add("@SHRINKAGEFACTOR", SqlDbType.Decimal, 0, "SHRINKAGEFACTOR")
                da.InsertCommand.Parameters.Add("@PRCHSNGLDTM", SqlDbType.Decimal, 0, "PRCHSNGLDTM")
                da.InsertCommand.Parameters.Add("@MNFCTRNGFXDLDTM", SqlDbType.Decimal, 0, "MNFCTRNGFXDLDTM")
                da.InsertCommand.Parameters.Add("@MNFCTRNGVRBLLDTM", SqlDbType.Decimal, 0, "MNFCTRNGVRBLLDTM")
                da.InsertCommand.Parameters.Add("@STAGINGLDTME", SqlDbType.Decimal, 0, "STAGINGLDTME")
                da.InsertCommand.Parameters.Add("@PLNNNGTMFNCDYS", SqlDbType.SmallInt, 0, "PLNNNGTMFNCDYS")
                da.InsertCommand.Parameters.Add("@DMNDTMFNCPRDS", SqlDbType.SmallInt, 0, "DMNDTMFNCPRDS")
                da.InsertCommand.Parameters.Add("@INCLDDINPLNNNG", SqlDbType.TinyInt, 0, "INCLDDINPLNNNG")
                da.InsertCommand.Parameters.Add("@CALCULATEATP", SqlDbType.TinyInt, 0, "CALCULATEATP")
                da.InsertCommand.Parameters.Add("@AUTOCHKATP", SqlDbType.TinyInt, 0, "AUTOCHKATP")
                da.InsertCommand.Parameters.Add("@PLNFNLPAB", SqlDbType.TinyInt, 0, "PLNFNLPAB")
                da.InsertCommand.Parameters.Add("@FRCSTCNSMPTNPRD", SqlDbType.SmallInt, 0, "FRCSTCNSMPTNPRD")
                da.InsertCommand.Parameters.Add("@ORDRUPTOLVL", SqlDbType.Decimal, 0, "ORDRUPTOLVL")
                da.InsertCommand.Parameters.Add("@SFTYSTCKQTY", SqlDbType.Decimal, 0, "SFTYSTCKQTY")
                da.InsertCommand.Parameters.Add("@REORDERVARIANCE", SqlDbType.Decimal, 0, "REORDERVARIANCE")
                da.InsertCommand.Parameters.Add("@PORECEIPTBIN", SqlDbType.Char, 0, "PORECEIPTBIN")
                da.InsertCommand.Parameters.Add("@PORETRNBIN", SqlDbType.Char, 0, "PORETRNBIN")
                da.InsertCommand.Parameters.Add("@SOFULFILLMENTBIN", SqlDbType.Char, 0, "SOFULFILLMENTBIN")
                da.InsertCommand.Parameters.Add("@SORETURNBIN", SqlDbType.Char, 0, "SORETURNBIN")
                da.InsertCommand.Parameters.Add("@BOMRCPTBIN", SqlDbType.Char, 0, "BOMRCPTBIN")
                da.InsertCommand.Parameters.Add("@MATERIALISSUEBIN", SqlDbType.Char, 0, "MATERIALISSUEBIN")
                da.InsertCommand.Parameters.Add("@MORECEIPTBIN", SqlDbType.Char, 0, "MORECEIPTBIN")
                da.InsertCommand.Parameters.Add("@REPAIRISSUESBIN", SqlDbType.Char, 0, "REPAIRISSUESBIN")
                da.InsertCommand.Parameters.Add("@ReplenishmentLevel", SqlDbType.SmallInt, 0, "ReplenishmentLevel")
                da.InsertCommand.Parameters.Add("@POPOrderMethod", SqlDbType.SmallInt, 0, "POPOrderMethod")
                da.InsertCommand.Parameters.Add("@MasterLocationCode", SqlDbType.Char, 0, "MasterLocationCode")
                da.InsertCommand.Parameters.Add("@POPVendorSelection", SqlDbType.SmallInt, 0, "POPVendorSelection")
                da.InsertCommand.Parameters.Add("@POPPricingSelection", SqlDbType.SmallInt, 0, "POPPricingSelection")
                da.InsertCommand.Parameters.Add("@PurchasePrice", SqlDbType.Decimal, 0, "PurchasePrice")
                da.InsertCommand.Parameters.Add("@IncludeAllocations", SqlDbType.TinyInt, 0, "IncludeAllocations")
                da.InsertCommand.Parameters.Add("@IncludeBackorders", SqlDbType.TinyInt, 0, "IncludeBackorders")
                da.InsertCommand.Parameters.Add("@IncludeRequisitions", SqlDbType.TinyInt, 0, "IncludeRequisitions")
                da.InsertCommand.Parameters.Add("@PICKTICKETITEMOPT", SqlDbType.SmallInt, 0, "PICKTICKETITEMOPT")
                da.InsertCommand.Parameters.Add("@INCLDMRPMOVEIN", SqlDbType.TinyInt, 0, "INCLDMRPMOVEIN")
                da.InsertCommand.Parameters.Add("@INCLDMRPMOVEOUT", SqlDbType.TinyInt, 0, "INCLDMRPMOVEOUT")
                da.InsertCommand.Parameters.Add("@INCLDMRPCANCEL", SqlDbType.TinyInt, 0, "INCLDMRPCANCEL")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@DEX_ROW_ID", SqlDbType.Int, 0, "DEX_ROW_ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_IV00102").NewRow
            End If
            Update_Field("ItemNMBR", _ITEMNMBR, dr)
            Update_Field("LOCNCODE", _LOCNCODE, dr)
            Update_Field("BINNMBR", _BINNMBR, dr)
            Update_Field("RCRDTYPE", _RCRDTYPE, dr)
            Update_Field("PRIMVNDR", _PRIMVNDR, dr)
            Update_Field("ITMFRFLG", _ITMFRFLG, dr)
            Update_Field("BGNGQTY", _BGNGQTY, dr)
            Update_Field("LSORDQTY", _LSORDQTY, dr)
            Update_Field("LRCPTQTY", _LRCPTQTY, dr)
            Update_Field("LSTORDDT", _LSTORDDT, dr)
            Update_Field("LSORDVND", _LSORDVND, dr)
            Update_Field("LSRCPTDT", _LSRCPTDT, dr)
            Update_Field("QTYRQSTN", _QTYRQSTN, dr)
            Update_Field("QTYONORD", _QTYONORD, dr)
            Update_Field("QTYBKORD", _QTYBKORD, dr)
            Update_Field("QTY_Drop_Shipped", _QTY_Drop_Shipped, dr)
            Update_Field("QTYINUSE", _QTYINUSE, dr)
            Update_Field("QTYINSVC", _QTYINSVC, dr)
            Update_Field("QTYRTRND", _QTYRTRND, dr)
            Update_Field("QTYDMGED", _QTYDMGED, dr)
            Update_Field("QTYONHND", _QTYONHND, dr)
            Update_Field("ATYALLOC", _ATYALLOC, dr)
            Update_Field("QTYCOMTD", _QTYCOMTD, dr)
            Update_Field("QTYSOLD", _QTYSOLD, dr)
            Update_Field("NXTCNTDT", _NXTCNTDT, dr)
            Update_Field("NXTCNTTM", _NXTCNTTM, dr)
            Update_Field("LSTCNTDT", _LSTCNTDT, dr)
            Update_Field("LSTCNTTM", _LSTCNTTM, dr)
            Update_Field("STCKCNTINTRVL", _STCKCNTINTRVL, dr)
            Update_Field("Landed_Cost_Group_ID", _Landed_Cost_Group_ID, dr)
            Update_Field("BUYERID", _BUYERID, dr)
            Update_Field("PLANNERID", _PLANNERID, dr)
            Update_Field("ORDERPOLICY", _ORDERPOLICY, dr)
            Update_Field("FXDORDRQTY", _FXDORDRQTY, dr)
            Update_Field("ORDRPNTQTY", _ORDRPNTQTY, dr)
            Update_Field("NMBROFDYS", _NMBROFDYS, dr)
            Update_Field("MNMMORDRQTY", _MNMMORDRQTY, dr)
            Update_Field("MXMMORDRQTY", _MXMMORDRQTY, dr)
            Update_Field("ORDERMULTIPLE", _ORDERMULTIPLE, dr)
            Update_Field("REPLENISHMENTMETHOD", _REPLENISHMENTMETHOD, dr)
            Update_Field("SHRINKAGEFACTOR", _SHRINKAGEFACTOR, dr)
            Update_Field("PRCHSNGLDTM", _PRCHSNGLDTM, dr)
            Update_Field("MNFCTRNGFXDLDTM", _MNFCTRNGFXDLDTM, dr)
            Update_Field("MNFCTRNGVRBLLDTM", _MNFCTRNGVRBLLDTM, dr)
            Update_Field("STAGINGLDTME", _STAGINGLDTME, dr)
            Update_Field("PLNNNGTMFNCDYS", _PLNNNGTMFNCDYS, dr)
            Update_Field("DMNDTMFNCPRDS", _DMNDTMFNCPRDS, dr)
            Update_Field("INCLDDINPLNNNG", _INCLDDINPLNNNG, dr)
            Update_Field("CALCULATEATP", _CALCULATEATP, dr)
            Update_Field("AUTOCHKATP", _AUTOCHKATP, dr)
            Update_Field("PLNFNLPAB", _PLNFNLPAB, dr)
            Update_Field("FRCSTCNSMPTNPRD", _FRCSTCNSMPTNPRD, dr)
            Update_Field("ORDRUPTOLVL", _ORDRUPTOLVL, dr)
            Update_Field("SFTYSTCKQTY", _SFTYSTCKQTY, dr)
            Update_Field("REORDERVARIANCE", _REORDERVARIANCE, dr)
            Update_Field("PORECEIPTBIN", _PORECEIPTBIN, dr)
            Update_Field("PORETRNBIN", _PORETRNBIN, dr)
            Update_Field("SOFULFILLMENTBIN", _SOFULFILLMENTBIN, dr)
            Update_Field("SORETURNBIN", _SORETURNBIN, dr)
            Update_Field("BOMRCPTBIN", _BOMRCPTBIN, dr)
            Update_Field("MATERIALISSUEBIN", _MATERIALISSUEBIN, dr)
            Update_Field("MORECEIPTBIN", _MORECEIPTBIN, dr)
            Update_Field("REPAIRISSUESBIN", _REPAIRISSUESBIN, dr)
            Update_Field("ReplenishmentLevel", _ReplenishmentLevel, dr)
            Update_Field("POPOrderMethod", _POPOrderMethod, dr)
            Update_Field("MasterLocationCode", _MasterLocationCode, dr)
            Update_Field("POPVendorSelection", _POPVendorSelection, dr)
            Update_Field("POPPricingSelection", _POPPricingSelection, dr)
            Update_Field("PurchasePrice", _PurchasePrice, dr)
            Update_Field("IncludeAllocations", _IncludeAllocations, dr)
            Update_Field("IncludeBackorders", _IncludeBackorders, dr)
            Update_Field("IncludeRequisitions", _IncludeRequisitions, dr)
            Update_Field("PICKTICKETITEMOPT", _PICKTICKETITEMOPT, dr)
            Update_Field("INCLDMRPMOVEIN", _INCLDMRPMOVEIN, dr)
            Update_Field("INCLDMRPMOVEOUT", _INCLDMRPMOVEOUT, dr)
            Update_Field("INCLDMRPCANCEL", _INCLDMRPCANCEL, dr)
            If ds.Tables("t_IV00102").Rows.Count < 1 Then ds.Tables("t_IV00102").Rows.Add(dr)
            da.Update(ds, "t_IV00102")
            _ID = ds.Tables("t_IV00102").Rows(0).Item("DEX_ROW_ID")
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
            oEvents.KeyField = "InventoryItemID"
            Dim oclsInv As New clsIV00101
            oEvents.KeyValue = oclsInv.get_Parent_ID(_ID)
            oclsInv = Nothing
            oEvents.Create_Event()
            drow(sField) = RTrim(sValue)
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Get_Qty_On_Hand(ByVal itemID As Integer) As Integer
        Dim qtyOnHand As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select case when iv2.QTYONHND is null then 0 else iv2.QTYONHND end as OnHand from t_IV00101 iv1 inner join t_IV00102 iv2 on RTrim(iv1.ITEMNMBR) = RTrim(iv2.ITEMNMBR) where iv2.LOCNCODE = 'WAREHOUSE' and iv1.DEX_ROW_ID = " & itemID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                qtyOnHand = dread("OnHand")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return qtyOnHand
    End Function

    Public Function Get_Min_Order_Amt(ByVal itemID As Integer) As Decimal
        Dim minOrderAmt As Decimal = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select case when iv2.MNMMORDRQTY is null then 0 else iv2.MNMMORDRQTY end as MinOrderAmt from t_IV00101 iv1 inner join t_IV00102 iv2 on RTrim(iv1.ITEMNMBR) = RTrim(iv2.ITEMNMBR) where iv2.LOCNCODE = 'WAREHOUSE' and iv1.DEX_ROW_ID = " & itemID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                minOrderAmt = dread("MinOrderAmt")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return minOrderAmt
    End Function

    Public Function Get_Par_Amt(ByVal itemID As Integer) As Decimal
        Dim parAmt As Decimal = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select case when iv2.SFTYSTCKQTY is null then 0 else iv2.SFTYSTCKQTY end as ParAmt from t_IV00101 iv1 inner join t_IV00102 iv2 on RTrim(iv1.ITEMNMBR) = RTrim(iv2.ITEMNMBR) where iv2.LOCNCODE = 'WAREHOUSE' and iv1.DEX_ROW_ID = " & itemID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                parAmt = dread("ParAmt")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return parAmt
    End Function

    Public Function Get_Item_ID(ByVal itemID As Integer) As Integer
        Dim ID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select iv2.Dex_Row_ID from t_IV00101 iv1 inner join t_IV00102 iv2 on RTrim(iv1.ITEMNMBR) = RTRIM(iv2.ITEMNMBR) where iv2.LOCNCODE = 'WAREHOUSE' and iv1.DEX_ROW_ID = " & itemID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ID = dread("DEX_ROW_ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ID
    End Function

    Public Property ITEMNMBR() As String
        Get
            Return _ITEMNMBR
        End Get
        Set(ByVal value As String)
            _ITEMNMBR = value
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

    Public Property BINNMBR() As String
        Get
            Return _BINNMBR
        End Get
        Set(ByVal value As String)
            _BINNMBR = value
        End Set
    End Property

    Public Property RCRDTYPE() As Integer
        Get
            Return _RCRDTYPE
        End Get
        Set(ByVal value As Integer)
            _RCRDTYPE = value
        End Set
    End Property

    Public Property PRIMVNDR() As String
        Get
            Return _PRIMVNDR
        End Get
        Set(ByVal value As String)
            _PRIMVNDR = value
        End Set
    End Property

    Public Property ITMFRFLG() As Decimal
        Get
            Return _ITMFRFLG
        End Get
        Set(ByVal value As Decimal)
            _ITMFRFLG = value
        End Set
    End Property

    Public Property BGNGQTY() As Decimal
        Get
            Return _BGNGQTY
        End Get
        Set(ByVal value As Decimal)
            _BGNGQTY = value
        End Set
    End Property

    Public Property LSORDQTY() As Decimal
        Get
            Return _LSORDQTY
        End Get
        Set(ByVal value As Decimal)
            _LSORDQTY = value
        End Set
    End Property

    Public Property LRCPTQTY() As Decimal
        Get
            Return _LRCPTQTY
        End Get
        Set(ByVal value As Decimal)
            _LRCPTQTY = value
        End Set
    End Property

    Public Property LSTORDDT() As String
        Get
            Return _LSTORDDT
        End Get
        Set(ByVal value As String)
            _LSTORDDT = value
        End Set
    End Property

    Public Property LSORDVND() As String
        Get
            Return _LSORDVND
        End Get
        Set(ByVal value As String)
            _LSORDVND = value
        End Set
    End Property

    Public Property LSRCPTDT() As String
        Get
            Return _LSRCPTDT
        End Get
        Set(ByVal value As String)
            _LSRCPTDT = value
        End Set
    End Property

    Public Property QTYRQSTN() As Decimal
        Get
            Return _QTYRQSTN
        End Get
        Set(ByVal value As Decimal)
            _QTYRQSTN = value
        End Set
    End Property

    Public Property QTYONORD() As Decimal
        Get
            Return _QTYONORD
        End Get
        Set(ByVal value As Decimal)
            _QTYONORD = value
        End Set
    End Property

    Public Property QTYBKORD() As Decimal
        Get
            Return _QTYBKORD
        End Get
        Set(ByVal value As Decimal)
            _QTYBKORD = value
        End Set
    End Property

    Public Property QTY_Drop_Shipped() As Decimal
        Get
            Return _QTY_Drop_Shipped
        End Get
        Set(ByVal value As Decimal)
            _QTY_Drop_Shipped = value
        End Set
    End Property

    Public Property QTYINUSE() As Decimal
        Get
            Return _QTYINUSE
        End Get
        Set(ByVal value As Decimal)
            _QTYINUSE = value
        End Set
    End Property

    Public Property QTYINSVC() As Decimal
        Get
            Return _QTYINSVC
        End Get
        Set(ByVal value As Decimal)
            _QTYINSVC = value
        End Set
    End Property

    Public Property QTYRTRND() As Decimal
        Get
            Return _QTYRTRND
        End Get
        Set(ByVal value As Decimal)
            _QTYRTRND = value
        End Set
    End Property

    Public Property QTYDMGED() As Decimal
        Get
            Return _QTYDMGED
        End Get
        Set(ByVal value As Decimal)
            _QTYDMGED = value
        End Set
    End Property

    Public Property QTYONHND() As Decimal
        Get
            Return _QTYONHND
        End Get
        Set(ByVal value As Decimal)
            _QTYONHND = value
        End Set
    End Property

    Public Property ATYALLOC() As Decimal
        Get
            Return _ATYALLOC
        End Get
        Set(ByVal value As Decimal)
            _ATYALLOC = value
        End Set
    End Property

    Public Property QTYCOMTD() As Decimal
        Get
            Return _QTYCOMTD
        End Get
        Set(ByVal value As Decimal)
            _QTYCOMTD = value
        End Set
    End Property

    Public Property QTYSOLD() As Decimal
        Get
            Return _QTYSOLD
        End Get
        Set(ByVal value As Decimal)
            _QTYSOLD = value
        End Set
    End Property

    Public Property NXTCNTDT() As String
        Get
            Return _NXTCNTDT
        End Get
        Set(ByVal value As String)
            _NXTCNTDT = value
        End Set
    End Property

    Public Property NXTCNTTM() As String
        Get
            Return _NXTCNTTM
        End Get
        Set(ByVal value As String)
            _NXTCNTTM = value
        End Set
    End Property

    Public Property LSTCNTDT() As String
        Get
            Return _LSTCNTDT
        End Get
        Set(ByVal value As String)
            _LSTCNTDT = value
        End Set
    End Property

    Public Property LSTCNTTM() As String
        Get
            Return _LSTCNTTM
        End Get
        Set(ByVal value As String)
            _LSTCNTTM = value
        End Set
    End Property

    Public Property STCKCNTINTRVL() As Integer
        Get
            Return _STCKCNTINTRVL
        End Get
        Set(ByVal value As Integer)
            _STCKCNTINTRVL = value
        End Set
    End Property

    Public Property Landed_Cost_Group_ID() As String
        Get
            Return _Landed_Cost_Group_ID
        End Get
        Set(ByVal value As String)
            _Landed_Cost_Group_ID = value
        End Set
    End Property

    Public Property BUYERID() As String
        Get
            Return _BUYERID
        End Get
        Set(ByVal value As String)
            _BUYERID = value
        End Set
    End Property

    Public Property PLANNERID() As String
        Get
            Return _PLANNERID
        End Get
        Set(ByVal value As String)
            _PLANNERID = value
        End Set
    End Property

    Public Property ORDERPOLICY() As Integer
        Get
            Return _ORDERPOLICY
        End Get
        Set(ByVal value As Integer)
            _ORDERPOLICY = value
        End Set
    End Property

    Public Property FXDORDRQTY() As Decimal
        Get
            Return _FXDORDRQTY
        End Get
        Set(ByVal value As Decimal)
            _FXDORDRQTY = value
        End Set
    End Property

    Public Property ORDRPNTQTY() As Decimal
        Get
            Return _ORDRPNTQTY
        End Get
        Set(ByVal value As Decimal)
            _ORDRPNTQTY = value
        End Set
    End Property

    Public Property NMBROFDYS() As Integer
        Get
            Return _NMBROFDYS
        End Get
        Set(ByVal value As Integer)
            _NMBROFDYS = value
        End Set
    End Property

    Public Property MNMMORDRQTY() As Decimal
        Get
            Return _MNMMORDRQTY
        End Get
        Set(ByVal value As Decimal)
            _MNMMORDRQTY = value
        End Set
    End Property

    Public Property MXMMORDRQTY() As Decimal
        Get
            Return _MXMMORDRQTY
        End Get
        Set(ByVal value As Decimal)
            _MXMMORDRQTY = value
        End Set
    End Property

    Public Property ORDERMULTIPLE() As Decimal
        Get
            Return _ORDERMULTIPLE
        End Get
        Set(ByVal value As Decimal)
            _ORDERMULTIPLE = value
        End Set
    End Property

    Public Property REPLENISHMENTMETHOD() As Integer
        Get
            Return _REPLENISHMENTMETHOD
        End Get
        Set(ByVal value As Integer)
            _REPLENISHMENTMETHOD = value
        End Set
    End Property

    Public Property SHRINKAGEFACTOR() As Decimal
        Get
            Return _SHRINKAGEFACTOR
        End Get
        Set(ByVal value As Decimal)
            _SHRINKAGEFACTOR = value
        End Set
    End Property

    Public Property PRCHSNGLDTM() As Decimal
        Get
            Return _PRCHSNGLDTM
        End Get
        Set(ByVal value As Decimal)
            _PRCHSNGLDTM = value
        End Set
    End Property

    Public Property MNFCTRNGFXDLDTM() As Decimal
        Get
            Return _MNFCTRNGFXDLDTM
        End Get
        Set(ByVal value As Decimal)
            _MNFCTRNGFXDLDTM = value
        End Set
    End Property

    Public Property MNFCTRNGVRBLLDTM() As Decimal
        Get
            Return _MNFCTRNGVRBLLDTM
        End Get
        Set(ByVal value As Decimal)
            _MNFCTRNGVRBLLDTM = value
        End Set
    End Property

    Public Property STAGINGLDTME() As Decimal
        Get
            Return _STAGINGLDTME
        End Get
        Set(ByVal value As Decimal)
            _STAGINGLDTME = value
        End Set
    End Property

    Public Property PLNNNGTMFNCDYS() As Integer
        Get
            Return _PLNNNGTMFNCDYS
        End Get
        Set(ByVal value As Integer)
            _PLNNNGTMFNCDYS = value
        End Set
    End Property

    Public Property DMNDTMFNCPRDS() As Integer
        Get
            Return _DMNDTMFNCPRDS
        End Get
        Set(ByVal value As Integer)
            _DMNDTMFNCPRDS = value
        End Set
    End Property

    Public Property INCLDDINPLNNNG() As Integer
        Get
            Return _INCLDDINPLNNNG
        End Get
        Set(ByVal value As Integer)
            _INCLDDINPLNNNG = value
        End Set
    End Property

    Public Property CALCULATEATP() As Integer
        Get
            Return _CALCULATEATP
        End Get
        Set(ByVal value As Integer)
            _CALCULATEATP = value
        End Set
    End Property

    Public Property AUTOCHKATP() As Integer
        Get
            Return _AUTOCHKATP
        End Get
        Set(ByVal value As Integer)
            _AUTOCHKATP = value
        End Set
    End Property

    Public Property PLNFNLPAB() As Integer
        Get
            Return _PLNFNLPAB
        End Get
        Set(ByVal value As Integer)
            _PLNFNLPAB = value
        End Set
    End Property

    Public Property FRCSTCNSMPTNPRD() As Integer
        Get
            Return _FRCSTCNSMPTNPRD
        End Get
        Set(ByVal value As Integer)
            _FRCSTCNSMPTNPRD = value
        End Set
    End Property

    Public Property ORDRUPTOLVL() As Decimal
        Get
            Return _ORDRUPTOLVL
        End Get
        Set(ByVal value As Decimal)
            _ORDRUPTOLVL = value
        End Set
    End Property

    Public Property SFTYSTCKQTY() As Decimal
        Get
            Return _SFTYSTCKQTY
        End Get
        Set(ByVal value As Decimal)
            _SFTYSTCKQTY = value
        End Set
    End Property

    Public Property REORDERVARIANCE() As Decimal
        Get
            Return _REORDERVARIANCE
        End Get
        Set(ByVal value As Decimal)
            _REORDERVARIANCE = value
        End Set
    End Property

    Public Property PORECEIPTBIN() As String
        Get
            Return _PORECEIPTBIN
        End Get
        Set(ByVal value As String)
            _PORECEIPTBIN = value
        End Set
    End Property

    Public Property PORETRNBIN() As String
        Get
            Return _PORETRNBIN
        End Get
        Set(ByVal value As String)
            _PORETRNBIN = value
        End Set
    End Property

    Public Property SOFULFILLMENTBIN() As String
        Get
            Return _SOFULFILLMENTBIN
        End Get
        Set(ByVal value As String)
            _SOFULFILLMENTBIN = value
        End Set
    End Property

    Public Property SORETURNBIN() As String
        Get
            Return _SORETURNBIN
        End Get
        Set(ByVal value As String)
            _SORETURNBIN = value
        End Set
    End Property

    Public Property BOMRCPTBIN() As String
        Get
            Return _BOMRCPTBIN
        End Get
        Set(ByVal value As String)
            _BOMRCPTBIN = value
        End Set
    End Property

    Public Property MATERIALISSUEBIN() As String
        Get
            Return _MATERIALISSUEBIN
        End Get
        Set(ByVal value As String)
            _MATERIALISSUEBIN = value
        End Set
    End Property

    Public Property MORECEIPTBIN() As String
        Get
            Return _MORECEIPTBIN
        End Get
        Set(ByVal value As String)
            _MORECEIPTBIN = value
        End Set
    End Property

    Public Property REPAIRISSUESBIN() As String
        Get
            Return _REPAIRISSUESBIN
        End Get
        Set(ByVal value As String)
            _REPAIRISSUESBIN = value
        End Set
    End Property

    Public Property ReplenishmentLevel() As Integer
        Get
            Return _ReplenishmentLevel
        End Get
        Set(ByVal value As Integer)
            _ReplenishmentLevel = value
        End Set
    End Property

    Public Property POPOrderMethod() As Integer
        Get
            Return _POPOrderMethod
        End Get
        Set(ByVal value As Integer)
            _POPOrderMethod = value
        End Set
    End Property

    Public Property MasterLocationCode() As String
        Get
            Return _MasterLocationCode
        End Get
        Set(ByVal value As String)
            _MasterLocationCode = value
        End Set
    End Property

    Public Property POPVendorSelection() As Integer
        Get
            Return _POPVendorSelection
        End Get
        Set(ByVal value As Integer)
            _POPVendorSelection = value
        End Set
    End Property

    Public Property POPPricingSelection() As Integer
        Get
            Return _POPPricingSelection
        End Get
        Set(ByVal value As Integer)
            _POPPricingSelection = value
        End Set
    End Property

    Public Property PurchasePrice() As Decimal
        Get
            Return _PurchasePrice
        End Get
        Set(ByVal value As Decimal)
            _PurchasePrice = value
        End Set
    End Property

    Public Property IncludeAllocations() As Integer
        Get
            Return _IncludeAllocations
        End Get
        Set(ByVal value As Integer)
            _IncludeAllocations = value
        End Set
    End Property

    Public Property IncludeBackorders() As Integer
        Get
            Return _IncludeBackorders
        End Get
        Set(ByVal value As Integer)
            _IncludeBackorders = value
        End Set
    End Property

    Public Property IncludeRequisitions() As Integer
        Get
            Return _IncludeRequisitions
        End Get
        Set(ByVal value As Integer)
            _IncludeRequisitions = value
        End Set
    End Property

    Public Property PICKTICKETITEMOPT() As Decimal
        Get
            Return _PICKTICKETITEMOPT
        End Get
        Set(ByVal value As Decimal)
            _PICKTICKETITEMOPT = value
        End Set
    End Property

    Public Property INCLDMRPMOVEIN() As Integer
        Get
            Return _INCLDMRPMOVEIN
        End Get
        Set(ByVal value As Integer)
            _INCLDMRPMOVEIN = value
        End Set
    End Property

    Public Property INCLDMRPMOVEOUT() As Integer
        Get
            Return _INCLDMRPMOVEOUT
        End Get
        Set(ByVal value As Integer)
            _INCLDMRPMOVEOUT = value
        End Set
    End Property

    Public Property INCLDMRPCANCEL() As Integer
        Get
            Return _INCLDMRPCANCEL
        End Get
        Set(ByVal value As Integer)
            _INCLDMRPCANCEL = value
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
