Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsQuickCheckHist
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _StatusID As Integer = 0
    Dim _CreateById As Integer = 0
    Dim _StatusDate As String = ""
    Dim _UpdatedById As Integer = 0
    Dim _QuickCheckID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_QuickCheckHist where QuickCheckHistID = " & _ID, cn)
    End Sub

    Public Function List() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select h.QuickCheckHistID, h.RoomID, s.ComboItem as Status, q.Name, p.UserName as CreatedBy,h.DateCreated, p2.UserName as LastUpdatedBy, h.StatusDate from t_QuickCheckHist h	inner join t_ComboItems s on s.ComboItemID=h.StatusID inner join t_QuickCheck q on q.QuickCheckID=h.QuickCheckID inner join t_Personnel p on p.PersonnelID=h.CreateById	inner join t_Personnel p2 on p2.PersonnelID=h.UpdatedById")
    End Function

    Public Function List_By_Room(RoomID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select h.QuickCheckHistID, h.RoomID, s.ComboItem as Status, q.Name, p.UserName as CreatedBy,h.DateCreated, p2.UserName as LastUpdatedBy, h.StatusDate from t_QuickCheckHist h	inner join t_ComboItems s on s.ComboItemID=h.StatusID inner join t_QuickCheck q on q.QuickCheckID=h.QuickCheckID inner join t_Personnel p on p.PersonnelID=h.CreateById	inner join t_Personnel p2 on p2.PersonnelID=h.UpdatedById where h.roomid=" & RoomID & " order by h.QuickCheckHistID desc")
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_QuickCheckHist where QuickCheckHistID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_QuickCheckHist")
            If ds.Tables("t_QuickCheckHist").Rows.Count > 0 Then
                dr = ds.Tables("t_QuickCheckHist").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("QuickCheckHistID") Is System.DBNull.Value) Then _ID = dr("QuickCheckHistID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("CreateById") Is System.DBNull.Value) Then _CreateById = dr("CreateById")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("UpdatedById") Is System.DBNull.Value) Then _UpdatedById = dr("UpdatedById")
        If Not (dr("QuickCheckID") Is System.DBNull.Value) Then _QuickCheckID = dr("QuickCheckID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_QuickCheckHist where QuickCheckHistID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_QuickCheckHist")
            If ds.Tables("t_QuickCheckHist").Rows.Count > 0 Then
                dr = ds.Tables("t_QuickCheckHist").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_QuickCheckHistInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.Int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@CreateById", SqlDbType.Int, 0, "CreateById")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@UpdatedById", SqlDbType.Int, 0, "UpdatedById")
                da.InsertCommand.Parameters.Add("@QuickCheckID", SqlDbType.Int, 0, "QuickCheckID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@QuickCheckHistID", SqlDbType.Int, 0, "QuickCheckHistID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_QuickCheckHist").NewRow
            End If
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("CreateById", _CreateById, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("UpdatedById", _UpdatedById, dr)
            Update_Field("QuickCheckID", _QuickCheckID, dr)
            If ds.Tables("t_QuickCheckHist").Rows.Count < 1 Then ds.Tables("t_QuickCheckHist").Rows.Add(dr)
            da.Update(ds, "t_QuickCheckHist")
            _ID = ds.Tables("t_QuickCheckHist").Rows(0).Item("QuickCheckHistID")
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
            oEvents.KeyField = "QuickCheckHistID"
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

    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
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

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

    Public Property CreateById() As Integer
        Get
            Return _CreateById
        End Get
        Set(ByVal value As Integer)
            _CreateById = value
        End Set
    End Property

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property UpdatedById() As Integer
        Get
            Return _UpdatedById
        End Get
        Set(ByVal value As Integer)
            _UpdatedById = value
        End Set
    End Property

    Public Property QuickCheckID() As Integer
        Get
            Return _QuickCheckID
        End Get
        Set(ByVal value As Integer)
            _QuickCheckID = value
        End Set
    End Property

    Public Property QuickCheckHistID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
