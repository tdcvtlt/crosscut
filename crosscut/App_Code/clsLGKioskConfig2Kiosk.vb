Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLGKioskConfig2Kiosk
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _KioskConfigID As Integer = 0
    Dim _KioskID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LGKioskConfig2Kiosk where KioskConfig2KioskID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LGKioskConfig2Kiosk where KioskConfig2KioskID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskConfig2Kiosk")
            If ds.Tables("t_LGKioskConfig2Kiosk").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskConfig2Kiosk").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("KioskConfigID") Is System.DBNull.Value) Then _KioskConfigID = dr("KioskConfigID")
        If Not (dr("KioskID") Is System.DBNull.Value) Then _KioskID = dr("KioskID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LGKioskConfig2Kiosk where KioskConfig2KioskID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskConfig2Kiosk")
            If ds.Tables("t_LGKioskConfig2Kiosk").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskConfig2Kiosk").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LGKioskConfig2KioskInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@KioskConfigID", SqlDbType.int, 0, "KioskConfigID")
                da.InsertCommand.Parameters.Add("@KioskID", SqlDbType.int, 0, "KioskID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@KioskConfig2KioskID", SqlDbType.Int, 0, "KioskConfig2KioskID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LGKioskConfig2Kiosk").NewRow
            End If
            Update_Field("KioskConfigID", _KioskConfigID, dr)
            Update_Field("KioskID", _KioskID, dr)
            If ds.Tables("t_LGKioskConfig2Kiosk").Rows.Count < 1 Then ds.Tables("t_LGKioskConfig2Kiosk").Rows.Add(dr)
            da.Update(ds, "t_LGKioskConfig2Kiosk")
            _ID = ds.Tables("t_LGKioskConfig2Kiosk").Rows(0).Item("KioskConfig2KioskID")
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
            oEvents.KeyField = "KioskConfig2KioskID"
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

    Public Property KioskConfigID() As Integer
        Get
            Return _KioskConfigID
        End Get
        Set(ByVal value As Integer)
            _KioskConfigID = value
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

    Public Property KioskConfig2KioskID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
