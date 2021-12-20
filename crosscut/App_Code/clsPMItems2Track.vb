Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPMItems2Track
    Dim _UserID As Integer = 0
    Dim _ID As Int32 = 0
    Dim _name As String
    Dim _description As String
    Dim _categoryid As Integer = 0
    Dim _life As Integer = 0
    Dim _gpID As Int32
    Dim _gpPart As String = ""
    Dim _last_modified As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PMItems2Track where item2trackid = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PMItems2Track where item2trackid = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PMItems2Track")
            If ds.Tables("t_PMItems2Track").Rows.Count > 0 Then
                dr = ds.Tables("t_PMItems2Track").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("name") Is System.DBNull.Value) Then _name = dr("name")
        If Not (dr("description") Is System.DBNull.Value) Then _description = dr("description")
        If Not (dr("categoryid") Is System.DBNull.Value) Then _categoryid = dr("categoryid")
        If Not (dr("life") Is System.DBNull.Value) Then _life = dr("life")
        If Not (dr("gpID") Is System.DBNull.Value) Then _gpID = dr("gpID")
        If Not (dr("gpPart") Is System.DBNull.Value) Then _gpPart = dr("gpPart")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PMItems2Track where item2trackid = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PMItems2Track")
            If ds.Tables("t_PMItems2Track").Rows.Count > 0 Then
                dr = ds.Tables("t_PMItems2Track").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PMItems2TrackInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@name", SqlDbType.nchar, 0, "name")
                da.InsertCommand.Parameters.Add("@description", SqlDbType.NVarChar, 50, "description")
                da.InsertCommand.Parameters.Add("@categoryid", SqlDbType.int, 0, "categoryid")
                da.InsertCommand.Parameters.Add("@life", SqlDbType.int, 0, "life")
                da.InsertCommand.Parameters.Add("@gpID", SqlDbType.NChar, 0, "gpID")
                da.InsertCommand.Parameters.Add("@gpPart", SqlDbType.NVarChar, 50, "gpPart")
                da.InsertCommand.Parameters.Add("@last_modified", SqlDbType.DateTime, 0, "last_modified")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@item2trackid", SqlDbType.Int, 0, "item2trackid")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PMItems2Track").NewRow
            End If
            Update_Field("name", _name, dr)
            Update_Field("description", _description, dr)
            Update_Field("categoryid", _categoryid, dr)
            Update_Field("life", _life, dr)
            Update_Field("gpID", _gpID, dr)
            Update_Field("gpPart", _gpPart, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_PMItems2Track").Rows.Count < 1 Then ds.Tables("t_PMItems2Track").Rows.Add(dr)
            da.Update(ds, "t_PMItems2Track")
            _ID = ds.Tables("t_PMItems2Track").Rows(0).Item("item2trackid")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField).ToString(), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "item2trackid"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Search(ByVal filter As String, ByVal incDeActive As Boolean) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter <> "" Then
                If incDeActive = True Then
                    ds.SelectCommand = "Select Item2TrackID as ID, Name, Description from t_PMItems2Track where name like '%" & filter & "%' order by name asc"
                Else
                    ds.SelectCommand = "Select Item2TrackID as ID, Name, Description from t_PMItems2Track where name like '%" & filter & "%' and Active = 1 order by name asc"
                End If
            Else
                If incDeActive = True Then
                    ds.SelectCommand = "Select Item2TrackID as ID, Name, Description from t_PMItems2Track order by name asc"
                Else
                    ds.SelectCommand = "Select Item2TrackID as ID, Name, Description from t_PMItems2Track where active = 1 order by name asc"
                End If
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Public Property categoryid() As Integer
        Get
            Return _categoryid
        End Get
        Set(ByVal value As Integer)
            _categoryid = value
        End Set
    End Property

    Public Property life() As Integer
        Get
            Return _life
        End Get
        Set(ByVal value As Integer)
            _life = value
        End Set
    End Property

    Public Property gpID() As Int32
        Get
            Return _gpID
        End Get
        Set(ByVal value As Int32)
            _gpID = value
        End Set
    End Property

    Public Property last_modified() As String
        Get
            Return _last_modified
        End Get
        Set(ByVal value As String)
            _last_modified = value
        End Set
    End Property

    Public Property item2trackid() As Int32
        Get
            Return _ID
        End Get
        Set(ByVal value As Int32)
            _ID = value
        End Set
    End Property

    Public Property gpPart As string
        Get
            Return _gpPart
        End Get
        Set(value As String)
            _gpPart = value
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

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(value As Boolean)
            _Active = value
        End Set
    End Property
End Class
