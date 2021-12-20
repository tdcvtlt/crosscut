Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRefurb
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _Description As String = ""
    Dim _RoomTypeID As Integer = 0
    Dim _RoomSizeID As Integer = 0
    Dim _UnitTypeID As Integer = 0
    Dim _UnitStyleID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Refurb where RefurbID = " & _ID, cn)
    End Sub

    Public Function List() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select r.RefurbID, r.Name, r.Description, coalesce(rt.comboitem,'All') as RoomType, coalesce(ut.comboitem,'All') as UnitType,  coalesce(us.comboitem,'All') as UnitStyle  from t_Refurb r left outer join t_Comboitems rt on rt.comboitemid=r.roomtypeid left outer join t_Comboitems ut on ut.comboitemid=r.UnitTypeID left outer join t_Comboitems us on us.comboitemid=r.unitstyleid order by RefurbID")
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Refurb where RefurbID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Refurb")
            If ds.Tables("t_Refurb").Rows.Count > 0 Then
                dr = ds.Tables("t_Refurb").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RefurbID") Is System.DBNull.Value) Then _ID = dr("RefurbID")
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("RoomTypeID") Is System.DBNull.Value) Then _RoomTypeID = dr("RoomTypeID")
        If Not (dr("RoomSizeID") Is System.DBNull.Value) Then _RoomSizeID = dr("RoomSizeID")
        If Not (dr("UnitTypeID") Is System.DBNull.Value) Then _UnitTypeID = dr("UnitTypeID")
        If Not (dr("UnitStyleID") Is System.DBNull.Value) Then _UnitStyleID = dr("UnitStyleID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Refurb where RefurbID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Refurb")
            If ds.Tables("t_Refurb").Rows.Count > 0 Then
                dr = ds.Tables("t_Refurb").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RefurbInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.VarChar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.Text, 0, "Description")
                da.InsertCommand.Parameters.Add("@RoomTypeID", SqlDbType.Int, 0, "RoomTypeID")
                da.InsertCommand.Parameters.Add("@RoomSizeID", SqlDbType.Int, 0, "RoomSizeID")
                da.InsertCommand.Parameters.Add("@UnitTypeID", SqlDbType.Int, 0, "UnitTypeID")
                da.InsertCommand.Parameters.Add("@UnitStyleID", SqlDbType.Int, 0, "UnitStyleID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RefurbID", SqlDbType.Int, 0, "RefurbID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Refurb").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("RoomTypeID", _RoomTypeID, dr)
            Update_Field("RoomSizeID", _RoomSizeID, dr)
            Update_Field("UnitTypeID", _UnitTypeID, dr)
            Update_Field("UnitStyleID", _UnitStyleID, dr)
            If ds.Tables("t_Refurb").Rows.Count < 1 Then ds.Tables("t_Refurb").Rows.Add(dr)
            da.Update(ds, "t_Refurb")
            _ID = ds.Tables("t_Refurb").Rows(0).Item("RefurbID")
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
            oEvents.KeyField = "RefurbID"
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

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property RoomTypeID() As Integer
        Get
            Return _RoomTypeID
        End Get
        Set(ByVal value As Integer)
            _RoomTypeID = value
        End Set
    End Property

    Public Property RoomSizeID() As Integer
        Get
            Return _RoomSizeID
        End Get
        Set(ByVal value As Integer)
            _RoomSizeID = value
        End Set
    End Property

    Public Property UnitTypeID() As Integer
        Get
            Return _UnitTypeID
        End Get
        Set(ByVal value As Integer)
            _UnitTypeID = value
        End Set
    End Property

    Public Property UnitStyleID() As Integer
        Get
            Return _UnitStyleID
        End Get
        Set(ByVal value As Integer)
            _UnitStyleID = value
        End Set
    End Property

    Public Property RefurbID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
