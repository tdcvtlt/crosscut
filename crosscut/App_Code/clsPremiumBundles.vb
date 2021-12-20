Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPremiumBundles
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PremiumBundles where PremiumBundleID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PremiumBundles where PremiumBundleID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PremiumBundles")
            If ds.Tables("t_PremiumBundles").Rows.Count > 0 Then
                dr = ds.Tables("t_PremiumBundles").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PremiumBundles where PremiumBundleID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PremiumBundles")
            If ds.Tables("t_PremiumBundles").Rows.Count > 0 Then
                dr = ds.Tables("t_PremiumBundles").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PremiumBundlesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.VarChar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PremiumBundleID", SqlDbType.Int, 0, "PremiumBundleID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PremiumBundles").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_PremiumBundles").Rows.Count < 1 Then ds.Tables("t_PremiumBundles").Rows.Add(dr)
            da.Update(ds, "t_PremiumBundles")
            _ID = ds.Tables("t_PremiumBundles").Rows(0).Item("PremiumBundleID")
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
            oEvents.KeyField = "PremiumBundleID"
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

    Public Function List_Premium_Bundles(ByVal filter As String, ByVal filterText As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If Filter = "Name" Then
            If filterText = "" Then
                ds.SelectCommand = "Select * from t_PremiumBundles order by name asc"
            Else
                ds.SelectCommand = "Select * from t_PremiumBundles where name like '" & filterText & "%' order by name asc"
            End If
        Else
            If filterText = "" Then
                ds.SelectCommand = "Select * from t_PremiumBundles order by ID asc"
            Else
                ds.SelectCommand = "Select * from t_PremiumBundles where ID like '" & filterText & "%' order by ID asc"
            End If
        End If
        Return ds
    End Function

    Public Function Get_Premium_Bundles() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select PremiumBundleID, Name from t_PremiumBundles order by name asc"
        Return ds
    End Function

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

    Public Property PremiumBundleID() As Integer
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
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

End Class
