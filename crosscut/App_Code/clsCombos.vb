Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Public Class clsCombos
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ComboName As String = ""
    Dim _Description As String = 0
    Dim _Err As String = ""

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As DataRow
    Dim ds As DataSet
    Dim dRead As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Combos where ComboID =" & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Combos where ComboID =" & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Combos")

            If ds.Tables("Combos").Rows.Count > 0 Then
                dr = ds.Tables("Combos").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        _ID = dr("ComboID")
        If Not (dr("ComboName") Is System.DBNull.Value) Then _ComboName = dr("ComboName")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Combos where ComboID = " & _ID
            da = New SqlDataAdapter(cm)

            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "Combos")

            If ds.Tables("Combos").Rows.Count > 0 Then
                dr = ds.Tables("Combos").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CombosInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ComboName", SqlDbType.VarChar, 0, "ComboName")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")

                Dim paramater As SqlParameter = da.InsertCommand.Parameters.Add("@ComboID", SqlDbType.Int, 0, "ComboID")
                paramater.Direction = ParameterDirection.Output
                dr = ds.Tables("Combos").NewRow
            End If

            Update_Field("ComboName", _ComboName, dr)
            Update_Field("Description", _Description, dr)

            If ds.Tables("Combos").Rows.Count < 1 Then ds.Tables("Combos").Rows.Add(dr)
            da.Update(ds, "Combos")
            _ID = ds.Tables("Combos").Rows(0).Item("ComboID")
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
            oEvents.KeyField = "ComboID"
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
    Public Function LookUp_Combo(ByVal comboName As String) As Integer
        Dim comboID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ComboID from t_Combos where comboname = '" & comboName & "'"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                comboID = dRead("ComboID")
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return comboID
    End Function
    Public Property ComboName() As String
        Get
            Return _ComboName
        End Get
        Set(ByVal value As String)
            _ComboName = value
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

    Public Property ComboID As integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
