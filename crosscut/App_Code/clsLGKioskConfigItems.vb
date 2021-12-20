Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLGKioskConfigItems
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _KioskConfigID As Integer = 0
    Dim _Item As String = ""
    Dim _Value As String = ""
    Dim _ParentID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LGKioskConfigItems where KioskConfigItemID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LGKioskConfigItems where KioskConfigItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskConfigItems")
            If ds.Tables("t_LGKioskConfigItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskConfigItems").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("KioskConfigID") Is System.DBNull.Value) Then _KioskConfigID = dr("KioskConfigID")
        If Not (dr("Item") Is System.DBNull.Value) Then _Item = dr("Item")
        If Not (dr("Value") Is System.DBNull.Value) Then _Value = dr("Value")
        If Not (dr("ParentID") Is System.DBNull.Value) Then _ParentID = dr("ParentID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LGKioskConfigItems where KioskConfigItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskConfigItems")
            If ds.Tables("t_LGKioskConfigItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskConfigItems").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LGKioskConfigItemsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@KioskConfigID", SqlDbType.int, 0, "KioskConfigID")
                da.InsertCommand.Parameters.Add("@Item", SqlDbType.varchar, 0, "Item")
                da.InsertCommand.Parameters.Add("@Value", SqlDbType.text, 0, "Value")
                da.InsertCommand.Parameters.Add("@ParentID", SqlDbType.int, 0, "ParentID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@KioskConfigItemID", SqlDbType.Int, 0, "KioskConfigItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LGKioskConfigItems").NewRow
            End If
            Update_Field("KioskConfigID", _KioskConfigID, dr)
            Update_Field("Item", _Item, dr)
            Update_Field("Value", _Value, dr)
            Update_Field("ParentID", _ParentID, dr)
            If ds.Tables("t_LGKioskConfigItems").Rows.Count < 1 Then ds.Tables("t_LGKioskConfigItems").Rows.Add(dr)
            da.Update(ds, "t_LGKioskConfigItems")
            _ID = ds.Tables("t_LGKioskConfigItems").Rows(0).Item("KioskConfigItemID")
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
            oEvents.KeyField = "KioskConfigItemID"
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

    Public Property Item() As String
        Get
            Return _Item
        End Get
        Set(ByVal value As String)
            _Item = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

    Public Property ParentID() As Integer
        Get
            Return _ParentID
        End Get
        Set(ByVal value As Integer)
            _ParentID = value
        End Set
    End Property

    Public Property KioskConfigItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
