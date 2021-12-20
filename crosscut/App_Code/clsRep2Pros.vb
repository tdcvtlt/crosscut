Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRep2Pros
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _DateAssigned As String = ""
    Dim _DateRemoved As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Rep2Pros where Rep2ProsID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Rep2Pros where Rep2ProsID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Rep2Pros")
            If ds.Tables("t_Rep2Pros").Rows.Count > 0 Then
                dr = ds.Tables("t_Rep2Pros").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("DateAssigned") Is System.DBNull.Value) Then _DateAssigned = dr("DateAssigned")
        If Not (dr("DateRemoved") Is System.DBNull.Value) Then _DateRemoved = dr("DateRemoved")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Rep2Pros where Rep2ProsID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Rep2Pros")
            If ds.Tables("t_Rep2Pros").Rows.Count > 0 Then
                dr = ds.Tables("t_Rep2Pros").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Rep2ProsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@DateAssigned", SqlDbType.smalldatetime, 0, "DateAssigned")
                da.InsertCommand.Parameters.Add("@DateRemoved", SqlDbType.smalldatetime, 0, "DateRemoved")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Rep2ProsID", SqlDbType.Int, 0, "Rep2ProsID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Rep2Pros").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("DateAssigned", _DateAssigned, dr)
            Update_Field("DateRemoved", _DateRemoved, dr)
            If ds.Tables("t_Rep2Pros").Rows.Count < 1 Then ds.Tables("t_Rep2Pros").Rows.Add(dr)
            da.Update(ds, "t_Rep2Pros")
            _ID = ds.Tables("t_Rep2Pros").Rows(0).Item("Rep2ProsID")
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
            oEvents.KeyField = "Rep2ProsID"
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

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property DateAssigned() As String
        Get
            Return _DateAssigned
        End Get
        Set(ByVal value As String)
            _DateAssigned = value
        End Set
    End Property

    Public Property DateRemoved() As String
        Get
            Return _DateRemoved
        End Get
        Set(ByVal value As String)
            _DateRemoved = value
        End Set
    End Property

    Public Property Rep2ProsID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
