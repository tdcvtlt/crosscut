Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLGKiosk
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _LocationID As Integer = 0
    Dim _License As String = ""
    Dim _Active As Boolean = False
    Dim _Key As String = ""
    Dim _Username As String = ""
    Dim _Password As String = ""
    Dim _Expiration As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LGKiosk where KioskID = " & _ID, cn)
    End Sub

    Public Sub Load_By_Key(ByVal key As String)
        Try
            cm.CommandText = "Select * from t_LGKiosk where [key] = '" & key & "'"
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKiosk")
            If ds.Tables("t_LGKiosk").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKiosk").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            da = Nothing
            ds = Nothing
        End Try

    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LGKiosk where KioskID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKiosk")
            If ds.Tables("t_LGKiosk").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKiosk").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("KioskID") Is System.DBNull.Value) Then _ID = dr("KioskID")
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("License") Is System.DBNull.Value) Then _License = dr("License")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("Key") Is System.DBNull.Value) Then _Key = dr("Key")
        If Not (dr("Username") Is System.DBNull.Value) Then _Username = dr("Username")
        If Not (dr("Password") Is System.DBNull.Value) Then _Password = dr("Password")
        If Not (dr("Expiration") Is System.DBNull.Value) Then _Expiration = dr("Expiration")
        If _Expiration = "" Then _Expiration = Date.Today.AddDays(-1)
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LGKiosk where KioskID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LGKiosk")
            If ds.Tables("t_LGKiosk").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKiosk").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LGKioskInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@License", SqlDbType.varchar, 0, "License")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@Key", SqlDbType.varchar, 0, "Key")
                da.InsertCommand.Parameters.Add("@Username", SqlDbType.varchar, 0, "Username")
                da.InsertCommand.Parameters.Add("@Password", SqlDbType.VarChar, 0, "Password")
                da.InsertCommand.Parameters.Add("@Expiration", SqlDbType.DateTime, 0, "Expiration")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@KioskID", SqlDbType.Int, 0, "KioskID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LGKiosk").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("License", _License, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("Key", _Key, dr)
            Update_Field("Username", _Username, dr)
            Update_Field("Password", _Password, dr)
            Update_Field("Expiration", _Expiration, dr)
            If ds.Tables("t_LGKiosk").Rows.Count < 1 Then ds.Tables("t_LGKiosk").Rows.Add(dr)
            da.Update(ds, "t_LGKiosk")
            _ID = ds.Tables("t_LGKiosk").Rows(0).Item("KioskID")
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
            oEvents.KeyField = "KioskID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Query(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            Dim sName(2) As String
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " KioskID as ID, Name, cs.comboitem as Location, License, k.Active, Expiration from t_LGKiosk k left outer join t_Comboitems cs on cs.comboitemid = k.locationid "
            If sFilterField <> "" Then
                sql += " where " & sFilterField & " like '" & sFilterValue & "%' "
            End If

            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
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

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property License() As String
        Get
            Return _License
        End Get
        Set(ByVal value As String)
            _License = value
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

    Public Property Key() As String
        Get
            Return _Key
        End Get
        Set(ByVal value As String)
            _Key = value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return _Username
        End Get
        Set(ByVal value As String)
            _Username = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Public Property Expiration As String
        Get
            Return _Expiration
        End Get
        Set(ByVal value As String)
            _Expiration = value
        End Set
    End Property

    Public Property KioskID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
