Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsContractMerge
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _UserName As String = ""
    Dim _ContractID As Integer = 0
    Dim _ContractNumber As String = ""
    Dim _DateCreated As String = ""
    Dim _Printed As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ContractMerge where ContractMergeID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ContractMerge where ContractMergeID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ContractMerge")
            If ds.Tables("t_ContractMerge").Rows.Count > 0 Then
                dr = ds.Tables("t_ContractMerge").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("UserName") Is System.DBNull.Value) Then _UserName = dr("UserName")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("ContractNumber") Is System.DBNull.Value) Then _ContractNumber = dr("ContractNumber")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("Printed") Is System.DBNull.Value) Then _Printed = dr("Printed")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ContractMerge where ContractMergeID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ContractMerge")
            If ds.Tables("t_ContractMerge").Rows.Count > 0 Then
                dr = ds.Tables("t_ContractMerge").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ContractMergeInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@UserName", SqlDbType.varchar, 0, "UserName")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@ContractNumber", SqlDbType.varchar, 0, "ContractNumber")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@Printed", SqlDbType.Bit, 0, "Printed")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ContractMergeID", SqlDbType.Int, 0, "ContractMergeID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ContractMerge").NewRow
            End If
            Update_Field("UserName", _UserName, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("ContractNumber", _ContractNumber, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("Printed", _Printed, dr)
            If ds.Tables("t_ContractMerge").Rows.Count < 1 Then ds.Tables("t_ContractMerge").Rows.Add(dr)
            da.Update(ds, "t_ContractMerge")
            _ID = ds.Tables("t_ContractMerge").Rows(0).Item("ContractMergeID")
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
            oEvents.KeyField = "ContractMergeID"
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

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property ContractNumber() As String
        Get
            Return _ContractNumber
        End Get
        Set(ByVal value As String)
            _ContractNumber = value
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

    Public Property Printed() As Boolean
        Get
            Return _Printed
        End Get
        Set(ByVal value As Boolean)
            _Printed = value
        End Set
    End Property

    Public Property ContractMergeID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
