Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSSLBRequest
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _HRApproved As Integer = 0
    Dim _HRID As Integer = 0
    Dim _DateApproved As String = ""
    Dim _TotalSSLBHours As Integer = 0
    Dim _TotalUnpaidHours As Integer = 0
    Dim _DateCreated As String = ""
    Dim _RequestedByID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader
    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SSLBRequest where SSLBRequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SSLBRequest where SSLBRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SSLBRequest")
            If ds.Tables("t_SSLBRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_SSLBRequest").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("HRApproved") Is System.DBNull.Value) Then _HRApproved = dr("HRApproved")
        If Not (dr("HRID") Is System.DBNull.Value) Then _HRID = dr("HRID")
        If Not (dr("DateApproved") Is System.DBNull.Value) Then _DateApproved = dr("DateApproved")
        If Not (dr("TotalSSLBHours") Is System.DBNull.Value) Then _TotalSSLBHours = dr("TotalSSLBHours")
        If Not (dr("TotalUnpaidHours") Is System.DBNull.Value) Then _TotalUnpaidHours = dr("TotalUnpaidHours")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("RequestedByID") Is System.DBNull.Value) Then _RequestedByID = dr("RequestedByID")

    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SSLBRequest where SSLBRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SSLBRequest")
            If ds.Tables("t_SSLBRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_SSLBRequest").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SSLBRequestInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.Int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.Int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@HRApproved", SqlDbType.Int, 0, "HRApproved")
                da.InsertCommand.Parameters.Add("@HRID", SqlDbType.Int, 0, "HRID")
                da.InsertCommand.Parameters.Add("@DateApproved", SqlDbType.DateTime, 0, "DateApproved")
                da.InsertCommand.Parameters.Add("@TotalSSLBHours", SqlDbType.Int, 0, "TotalSSLBHours")
                da.InsertCommand.Parameters.Add("@TotalUnpaidHours", SqlDbType.Int, 0, "TotalUnpaidHours")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@RequestedByID", SqlDbType.Int, 0, "RequestedByID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SSLBRequestID", SqlDbType.Int, 0, "SSLBRequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SSLBRequest").NewRow
            End If
            Update_Field("DateApproved", _DateApproved, dr)
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("HRApproved", _HRApproved, dr)
            Update_Field("HRID", _HRID, dr)

            Update_Field("TotalSSLBHours", _TotalSSLBHours, dr)
            Update_Field("TotalUnpaidHours", _TotalUnpaidHours, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("RequestedByID", _RequestedByID, dr)
            If ds.Tables("t_SSLBRequest").Rows.Count < 1 Then ds.Tables("t_SSLBRequest").Rows.Add(dr)
            da.Update(ds, "t_SSLBRequest")
            _ID = ds.Tables("t_SSLBRequest").Rows(0).Item("SSLBRequestID")
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
            oEvents.KeyField = "SSLBRequestID"
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

    Public Function Check_SSLB_Balance(ByVal PersonnelID As Integer) As Double
        Dim sslbHours As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(AvailableSSLB) as SSLBBalance from v_SSLBBalances where personnelid = " & PersonnelID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                sslbHours = dread("SSLBBalance")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return sslbHours
    End Function

    Public Function Get_SSLB_Requests() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select pl.SSLBRequestID as ID, p.LastName + ', ' + p.FirstName as Employee, d.ComboItem as Department, pl.DateCreated, m.UserName as RequestedBy, pl.TotalSSLBHours, pl.TotalUnpaidHours from t_SSLBRequest pl inner join t_Personnel p on pl.PersonnelID = p.PersonnelID inner join t_ComboItems d on pl.DepartmentID = d.ComboItemID inner join t_personnel m on pl.RequestedByID = m.PersonnelID where pl.HRApproved = 0 order by p.LastName, p.FirstName"
        Return ds
    End Function

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As Integer)
            _DepartmentID = value
        End Set
    End Property

    Public Property HRApproved() As Integer
        Get
            Return _HRApproved
        End Get
        Set(ByVal value As Integer)
            _HRApproved = value
        End Set
    End Property

    Public Property HRID() As Integer
        Get
            Return _HRID
        End Get
        Set(ByVal value As Integer)
            _HRID = value
        End Set
    End Property

    Public Property DateApproved() As String
        Get
            Return _DateApproved
        End Get
        Set(ByVal value As String)
            _DateApproved = value
        End Set
    End Property

    Public Property TotalSSLBHours() As Integer
        Get
            Return _TotalSSLBHours
        End Get
        Set(ByVal value As Integer)
            _TotalSSLBHours = value
        End Set
    End Property

    Public Property TotalUnpaidHours() As Integer
        Get
            Return _TotalUnpaidHours
        End Get
        Set(ByVal value As Integer)
            _TotalUnpaidHours = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property RequestedByID() As Integer
        Get
            Return _RequestedByID
        End Get
        Set(ByVal value As Integer)
            _RequestedByID = value
        End Set
    End Property


    Public Property SSLBRequestID() As Integer
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property
End Class
