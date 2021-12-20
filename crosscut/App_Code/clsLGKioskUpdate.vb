Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLGKioskUpdate
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DateAvailable As String = ""
    Dim _DateUpdated As String = ""
    Dim _Active As Boolean = False
    Dim _Path As String = ""
    Dim _KioskID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LGKioskUpdate where KioskUpdateID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LGKioskUpdate where KioskUpdateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskUpdate")
            If ds.Tables("t_LGKioskUpdate").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskUpdate").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DateAvailable") Is System.DBNull.Value) Then _DateAvailable = dr("DateAvailable")
        If Not (dr("DateUpdated") Is System.DBNull.Value) Then _DateUpdated = dr("DateUpdated")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("Path") Is System.DBNull.Value) Then _Path = dr("Path")
        If Not (dr("KioskID") Is System.DBNull.Value) Then _KioskID = dr("KioskID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LGKioskUpdate where KioskUpdateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskUpdate")
            If ds.Tables("t_LGKioskUpdate").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskUpdate").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LGKioskUpdateInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DateAvailable", SqlDbType.smalldatetime, 0, "DateAvailable")
                da.InsertCommand.Parameters.Add("@DateUpdated", SqlDbType.smalldatetime, 0, "DateUpdated")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@Path", SqlDbType.text, 0, "Path")
                da.InsertCommand.Parameters.Add("@KioskID", SqlDbType.int, 0, "KioskID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@KioskUpdateID", SqlDbType.Int, 0, "KioskUpdateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LGKioskUpdate").NewRow
            End If
            Update_Field("DateAvailable", _DateAvailable, dr)
            Update_Field("DateUpdated", _DateUpdated, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("Path", _Path, dr)
            Update_Field("KioskID", _KioskID, dr)
            If ds.Tables("t_LGKioskUpdate").Rows.Count < 1 Then ds.Tables("t_LGKioskUpdate").Rows.Add(dr)
            da.Update(ds, "t_LGKioskUpdate")
            _ID = ds.Tables("t_LGKioskUpdate").Rows(0).Item("KioskUpdateID")
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
            oEvents.KeyField = "KioskUpdateID"
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

    Public Property DateAvailable() As String
        Get
            Return _DateAvailable
        End Get
        Set(ByVal value As String)
            _DateAvailable = value
        End Set
    End Property

    Public Property DateUpdated() As String
        Get
            Return _DateUpdated
        End Get
        Set(ByVal value As String)
            _DateUpdated = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(ByVal value As String)
            _Path = value
        End Set
    End Property

    Public Property KioskID() As Integer
        Get
            Return _KioskID
        End Get
        Set(ByVal value As Integer)
            _KioskID = value
        End Set
    End Property

    Public Property KioskUpdateID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
