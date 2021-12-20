Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsDoNotSellListCodes
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Code As Integer = 0
    Dim _CreatedByID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _ExpirationDate As String = ""
    Dim _UsedByID As Integer = 0
    Dim _DateUsed As String = ""
    Dim _Source As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_DoNotSellListCodes where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_DoNotSellListCodes where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_DoNotSellListCodes")
            If ds.Tables("t_DoNotSellListCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_DoNotSellListCodes").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Code") Is System.DBNull.Value) Then _Code = dr("Code")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("ExpirationDate") Is System.DBNull.Value) Then _ExpirationDate = dr("ExpirationDate")
        If Not (dr("UsedByID") Is System.DBNull.Value) Then _UsedByID = dr("UsedByID")
        If Not (dr("DateUsed") Is System.DBNull.Value) Then _DateUsed = dr("DateUsed")
        If Not (dr("Source") Is System.DBNull.Value) Then _Source = dr("Source")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_DoNotSellListCodes where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_DoNotSellListCodes")
            If ds.Tables("t_DoNotSellListCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_DoNotSellListCodes").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_DoNotSellListCodesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Code", SqlDbType.int, 0, "Code")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@ExpirationDate", SqlDbType.datetime, 0, "ExpirationDate")
                da.InsertCommand.Parameters.Add("@UsedByID", SqlDbType.int, 0, "UsedByID")
                da.InsertCommand.Parameters.Add("@DateUsed", SqlDbType.DateTime, 0, "DateUsed")
                da.InsertCommand.Parameters.Add("@Source", SqlDbType.VarChar, 0, "Source")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_DoNotSellListCodes").NewRow
            End If
            Update_Field("Code", _Code, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("ExpirationDate", _ExpirationDate, dr)
            Update_Field("UsedByID", _UsedByID, dr)
            Update_Field("DateUsed", _DateUsed, dr)
            Update_Field("Source", _Source, dr)
            If ds.Tables("t_DoNotSellListCodes").Rows.Count < 1 Then ds.Tables("t_DoNotSellListCodes").Rows.Add(dr)
            da.Update(ds, "t_DoNotSellListCodes")
            _ID = ds.Tables("t_DoNotSellListCodes").Rows(0).Item("ID")
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
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Dupe_Check(ByVal code As Integer) As Boolean
        Dim dupe As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_DoNotSellListCodes where code = " & code
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dupe = False
            Else
                dupe = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dupe
    End Function

    Public Function Get_Code_ID(ByVal code As Integer) As Integer
        Dim ID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ID from t_DoNotSellListCodes where code = " & code
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ID = dread("ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ID
    End Function

    Public Function Validate_Code(ByVal code As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_DoNotSellListCodes where code = " & code & " and DateUsed Is Null and DATEDIFF(mi,ExpirationDate,'" & System.DateTime.Now & "') < 1"
            dread = cm.ExecuteReader
            bValid = dread.HasRows
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Invalid_Reason(ByVal code As Integer) As String
        Dim err As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_DoNotSellListCodes where code = " & code
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("UsedByID") > 0 Then
                    err = "You Have Entered A Code That Has Already Been Used. Please Obtain Another Code From The FINANCE DEPARTMENT."
                Else
                    err = "The Code You Have Entered Has Expired. Please Obtain Another Code From The FINANCE DEPARTMENT."
                End If
            Else
                err = "You Have Entered An Invalid Code."
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return err
    End Function

    Public Property Code() As Integer
        Get
            Return _Code
        End Get
        Set(ByVal value As Integer)
            _Code = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
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

    Public Property ExpirationDate() As String
        Get
            Return _ExpirationDate
        End Get
        Set(ByVal value As String)
            _ExpirationDate = value
        End Set
    End Property

    Public Property UsedByID() As Integer
        Get
            Return _UsedByID
        End Get
        Set(ByVal value As Integer)
            _UsedByID = value
        End Set
    End Property

    Public Property DateUsed() As String
        Get
            Return _DateUsed
        End Get
        Set(ByVal value As String)
            _DateUsed = value
        End Set
    End Property
    Public Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
