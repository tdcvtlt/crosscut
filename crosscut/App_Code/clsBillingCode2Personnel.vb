Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsBillingCode2Personnel
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _BillingCodeID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _TitleID As Integer = 0
    Dim _FixedAmount As Decimal = 0
    Dim _PercentageAmount As Decimal = 0
    Dim _CampaignID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_BillingCode2Personnel where BillingCode2PersonnelID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_BillingCode2Personnel where BillingCode2PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_BillingCode2Personnel")
            If ds.Tables("t_BillingCode2Personnel").Rows.Count > 0 Then
                dr = ds.Tables("t_BillingCode2Personnel").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("BillingCodeID") Is System.DBNull.Value) Then _BillingCodeID = dr("BillingCodeID")
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("TitleID") Is System.DBNull.Value) Then _TitleID = dr("TitleID")
        If Not (dr("FixedAmount") Is System.DBNull.Value) Then _FixedAmount = dr("FixedAmount")
        If Not (dr("PercentageAmount") Is System.DBNull.Value) Then _PercentageAmount = dr("PercentageAmount")
        If Not (dr("CampaignID") Is System.DBNull.Value) Then _CampaignID = dr("CampaignID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_BillingCode2Personnel where BillingCode2PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_BillingCode2Personnel")
            If ds.Tables("t_BillingCode2Personnel").Rows.Count > 0 Then
                dr = ds.Tables("t_BillingCode2Personnel").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_BillingCode2PersonnelInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@BillingCodeID", SqlDbType.int, 0, "BillingCodeID")
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@TitleID", SqlDbType.int, 0, "TitleID")
                da.InsertCommand.Parameters.Add("@FixedAmount", SqlDbType.float, 0, "FixedAmount")
                da.InsertCommand.Parameters.Add("@PercentageAmount", SqlDbType.float, 0, "PercentageAmount")
                da.InsertCommand.Parameters.Add("@CampaignID", SqlDbType.int, 0, "CampaignID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@BillingCode2PersonnelID", SqlDbType.Int, 0, "BillingCode2PersonnelID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_BillingCode2Personnel").NewRow
            End If
            Update_Field("BillingCodeID", _BillingCodeID, dr)
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("TitleID", _TitleID, dr)
            Update_Field("FixedAmount", _FixedAmount, dr)
            Update_Field("PercentageAmount", _PercentageAmount, dr)
            Update_Field("CampaignID", _CampaignID, dr)
            If ds.Tables("t_BillingCode2Personnel").Rows.Count < 1 Then ds.Tables("t_BillingCode2Personnel").Rows.Add(dr)
            da.Update(ds, "t_BillingCode2Personnel")
            _ID = ds.Tables("t_BillingCode2Personnel").Rows(0).Item("BillingCode2PersonnelID")
            cn.Close()
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "BillingCode2PersonnelID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function List_Items(ByVal codeID As Integer, ByVal campID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select b.BillingCode2PersonnelID as ID, Case when p.Firstname is null then '' + p.Lastname when p.Lastname is null then p.Firstname + '' else p.Firstname + '' + ' ' + p.lastname end as Personnel, pt.ComboItem as Title, b.FixedAmount, b.PercentageAmount from t_BillingCode2Personnel b inner join t_Personnel p on b.PersonnelID = p.PersonnelID inner join t_ComboItems pt on b.TitleID = pt.CombOitemid where (b.CampaignID = " & campID & " or b.CampaignID = 0) and (b.BillingCodeID = " & codeID & " or b.BillingCodeID = 0) order by p.LastName"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Personnel(ByVal codeID As Integer, ByVal campID As Integer) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ID")
        dt.Columns.Add("PersonnelID")
        dt.Columns.Add("LastName")
        dt.Columns.Add("FirstName")
        dt.Columns.Add("TitleID")
        dt.Columns.Add("Title")
        dt.Columns.Add("Percentage")
        dt.Columns.Add("FixedAmount")
        dt.Columns.Add("Dirty")
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select bp.PersonnelID, p.LastName, p.FirstName, bp.TitleID, bpt.ComboItem as Title, bp.PercentageAmount, bp.FixedAmount from t_BillingCode2Personnel bp inner join t_Personnel p on bp.PersonnelID = p.PersonnelID inner join t_ComboItems bpt on bp.TitleID = bpt.ComboItemID where (bp.CampaignID = " & campID & " or bp.CampaignID = 0) and (bp.BillingCodeID = " & codeID & " or bp.BillingCodeID = 0) order by p.Lastname asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Dim ID As Integer = -1
                Do While dread.Read
                    dr = dt.NewRow
                    dr("ID") = ID
                    dr("PersonnelID") = dread("PersonnelID")
                    dr("LastName") = dread("LastName")
                    dr("FirstName") = dread("FirstName")
                    dr("TitleID") = dread("TitleID")
                    dr("Title") = dread("Title")
                    dr("Percentage") = dread("PercentageAmount")
                    dr("FixedAmount") = dread("FixedAmount")
                    dr("Dirty") = True
                    dt.Rows.Add(dr)
                    ID = ID - 1
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function
    Public Function Remove_Item(ByVal id As Integer) As Boolean
        Dim bRemoved As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_BillingCode2Personnel where billingcode2personnelid = " & id
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function
    Public Property BillingCodeID() As Integer
        Get
            Return _BillingCodeID
        End Get
        Set(ByVal value As Integer)
            _BillingCodeID = value
        End Set
    End Property

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property TitleID() As Integer
        Get
            Return _TitleID
        End Get
        Set(ByVal value As Integer)
            _TitleID = value
        End Set
    End Property

    Public Property FixedAmount() As Decimal
        Get
            Return _FixedAmount
        End Get
        Set(ByVal value As Decimal)
            _FixedAmount = value
        End Set
    End Property

    Public Property PercentageAmount() As Decimal
        Get
            Return _PercentageAmount
        End Get
        Set(ByVal value As Decimal)
            _PercentageAmount = value
        End Set
    End Property

    Public Property CampaignID() As Integer
        Get
            Return _CampaignID
        End Get
        Set(ByVal value As Integer)
            _CampaignID = value
        End Set
    End Property

    Public Property BillingCode2PersonnelID() As Integer
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
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
