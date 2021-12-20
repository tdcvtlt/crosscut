Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPMBuilding
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String
    Dim _Description As String
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PMBuilding where pmbuildingID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PMBuilding where pmbuildingID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PMBuilding")
            If ds.Tables("t_PMBuilding").Rows.Count > 0 Then
                dr = ds.Tables("t_PMBuilding").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PMBuilding where pmbuildingID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PMBuilding")
            If ds.Tables("t_PMBuilding").Rows.Count > 0 Then
                dr = ds.Tables("t_PMBuilding").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PMBuildingInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.nvarchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.nvarchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@pmbuildingID", SqlDbType.Int, 0, "pmbuildingID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PMBuilding").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_PMBuilding").Rows.Count < 1 Then ds.Tables("t_PMBuilding").Rows.Add(dr)
            da.Update(ds, "t_PMBuilding")
            _ID = ds.Tables("t_PMBuilding").Rows(0).Item("pmbuildingID")
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
            oEvents.KeyField = "pmbuildingID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Sub Update_PMBuildingID(pmBuildingID As Int32, unitID As String())
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Using cmd = New SqlCommand(String.Format("update t_unit set pmbuildingid = null where pmbuildingid={0}", pmBuildingID), cn)
                cmd.ExecuteNonQuery()
                cmd.CommandText = String.Format("update t_unit set pmbuildingid={0} where unitid in ({1})", pmBuildingID, String.Join(",", unitID))
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message)
        Finally
            cn.Close()
        End Try
    End Sub

    Public Function Retrieve_PMBuildingID(pmBuildingID As Int32) As String()
        Dim ar() As String = New String() {}
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Using cmd = New SqlCommand(String.Format("select unitid from t_unit where pmbuildingid={0}", pmBuildingID), cn)
                Dim rdr = cmd.ExecuteReader()
                Dim dt = New DataTable()
                dt.Load(rdr)
                ar = dt.Rows.OfType(Of DataRow).Select(Function(x) x("unitid").ToString()).ToArray()
            End Using
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message)
        Finally
            cn.Close()
        End Try
        Return ar
    End Function

    Public Function Get_Buildings() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select PMBuildingID as ID, Name from t_PMBuilding where Active = 1"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
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

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property pmbuildingID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
End Class
