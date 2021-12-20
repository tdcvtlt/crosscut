Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsContractUpDownGrade
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _OldContractID As Integer = 0
    Dim _NewContractID As Integer = 0
    Dim _SaleType As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ContractUpDownGrade where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ContractUpDownGrade where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ContractUpDownGrade")
            If ds.Tables("t_ContractUpDownGrade").Rows.Count > 0 Then
                dr = ds.Tables("t_ContractUpDownGrade").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("OldContractID") Is System.DBNull.Value) Then _OldContractID = dr("OldContractID")
        If Not (dr("NewContractID") Is System.DBNull.Value) Then _NewContractID = dr("NewContractID")
        If Not (dr("SaleType") Is System.DBNull.Value) Then _SaleType = dr("SaleType")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ContractUpDownGrade where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ContractUpDownGrade")
            If ds.Tables("t_ContractUpDownGrade").Rows.Count > 0 Then
                dr = ds.Tables("t_ContractUpDownGrade").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ContractUpDownGradeInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@OldContractID", SqlDbType.Int, 0, "OldContractID")
                da.InsertCommand.Parameters.Add("@NewContractID", SqlDbType.Int, 0, "NewContractID")
                da.InsertCommand.Parameters.Add("@SaleType", SqlDbType.VarChar, 0, "SaleType")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ContractUpDownGrade").NewRow
            End If
            Update_Field("OldContractID", _OldContractID, dr)
            Update_Field("NewContractID", _NewContractID, dr)
            Update_Field("SaleType", _SaleType, dr)
            If ds.Tables("t_ContractUpDownGrade").Rows.Count < 1 Then ds.Tables("t_ContractUpDownGrade").Rows.Add(dr)
            da.Update(ds, "t_ContractUpDownGrade")
            _ID = ds.Tables("t_ContractUpDownGrade").Rows(0).Item("ID")
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
            oEvents.KeyField = "ID"
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

    Public Function GetID(ByVal newID As Integer) As Integer
        Dim ID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ID from t_ContractUpDownGrade where newContractID = '" & newID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ID = dread("ID")
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ID
    End Function

    Public Function Delete_ID(ByVal ID As Integer) As Boolean
        Dim bDeleted As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_ContractUpDownGrade where ID = '" & ID & "'"
            cm.ExecuteNonQuery()
            bDeleted = True
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bDeleted
    End Function

    Public Function Get_Old_ContractID(ByVal conID As Integer) As Integer
        Dim oldConID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select OldContractID from t_ContractUpDownGrade where NewContractID = '" & conID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                oldConID = dread("OldContractID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return oldConID
    End Function

    Public Property OldContractID() As Integer
        Get
            Return _OldContractID
        End Get
        Set(ByVal value As Integer)
            _OldContractID = value
        End Set
    End Property

    Public Property NewContractID() As Integer
        Get
            Return _NewContractID
        End Get
        Set(ByVal value As Integer)
            _NewContractID = value
        End Set
    End Property

    Public Property SaleType() As String
        Get
            Return _SaleType
        End Get
        Set(ByVal value As String)
            _SaleType = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property
End Class
