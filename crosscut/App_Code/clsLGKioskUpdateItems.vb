Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLGKioskUpdateItems
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FileName As String = ""
    Dim _Active As Boolean = False
    Dim _KioskUpdateID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LGKioskUpdateItems where KioskUpdateItem = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LGKioskUpdateItems where KioskUpdateItem = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskUpdateItems")
            If ds.Tables("t_LGKioskUpdateItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskUpdateItems").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FileName") Is System.DBNull.Value) Then _FileName = dr("FileName")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("KioskUpdateID") Is System.DBNull.Value) Then _KioskUpdateID = dr("KioskUpdateID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LGKioskUpdateItems where KioskUpdateItem = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskUpdateItems")
            If ds.Tables("t_LGKioskUpdateItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskUpdateItems").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LGKioskUpdateItemsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FileName", SqlDbType.varchar, 0, "FileName")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@KioskUpdateID", SqlDbType.int, 0, "KioskUpdateID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@KioskUpdateItem", SqlDbType.Int, 0, "KioskUpdateItem")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LGKioskUpdateItems").NewRow
            End If
            Update_Field("FileName", _FileName, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("KioskUpdateID", _KioskUpdateID, dr)
            If ds.Tables("t_LGKioskUpdateItems").Rows.Count < 1 Then ds.Tables("t_LGKioskUpdateItems").Rows.Add(dr)
            da.Update(ds, "t_LGKioskUpdateItems")
            _ID = ds.Tables("t_LGKioskUpdateItems").Rows(0).Item("KioskUpdateItem")
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
            oEvents.KeyField = "KioskUpdateItem"
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

    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
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

    Public Property KioskUpdateID() As Integer
        Get
            Return _KioskUpdateID
        End Get
        Set(ByVal value As Integer)
            _KioskUpdateID = value
        End Set
    End Property

    Public Property KioskUpdateItem() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
