Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLGKioskCheck
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _KioskID As Integer = 0
    Dim _CheckInDate As String = ""
    Dim _Uploaded As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LGKioskCheck where CheckInID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LGKioskCheck where CheckInID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskCheck")
            If ds.Tables("t_LGKioskCheck").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskCheck").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("KioskID") Is System.DBNull.Value) Then _KioskID = dr("KioskID")
        If Not (dr("CheckInDate") Is System.DBNull.Value) Then _CheckInDate = dr("CheckInDate")
        If Not (dr("Uploaded") Is System.DBNull.Value) Then _Uploaded = dr("Uploaded")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LGKioskCheck where CheckInID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskCheck")
            If ds.Tables("t_LGKioskCheck").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskCheck").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LGKioskCheckInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@KioskID", SqlDbType.int, 0, "KioskID")
                da.InsertCommand.Parameters.Add("@CheckInDate", SqlDbType.smalldatetime, 0, "CheckInDate")
                da.InsertCommand.Parameters.Add("@Uploaded", SqlDbType.bit, 0, "Uploaded")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CheckInID", SqlDbType.Int, 0, "CheckInID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LGKioskCheck").NewRow
            End If
            Update_Field("KioskID", _KioskID, dr)
            Update_Field("CheckInDate", _CheckInDate, dr)
            Update_Field("Uploaded", _Uploaded, dr)
            If ds.Tables("t_LGKioskCheck").Rows.Count < 1 Then ds.Tables("t_LGKioskCheck").Rows.Add(dr)
            da.Update(ds, "t_LGKioskCheck")
            _ID = ds.Tables("t_LGKioskCheck").Rows(0).Item("CheckInID")
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
            oEvents.KeyField = "CheckInID"
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

    Public Property KioskID() As Integer
        Get
            Return _KioskID
        End Get
        Set(ByVal value As Integer)
            _KioskID = value
        End Set
    End Property

    Public Property CheckInDate() As String
        Get
            Return _CheckInDate
        End Get
        Set(ByVal value As String)
            _CheckInDate = value
        End Set
    End Property

    Public Property Uploaded() As Boolean
        Get
            Return _Uploaded
        End Get
        Set(ByVal value As Boolean)
            _Uploaded = value
        End Set
    End Property

    Public Property CheckInID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
