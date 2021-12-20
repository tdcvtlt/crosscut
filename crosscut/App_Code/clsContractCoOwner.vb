Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient



Public Class clsContractCoOwner
    Dim _ID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _err As String

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dRead As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("", cn)
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        dr = Nothing
        dRead = Nothing
        MyBase.Finalize()
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select c.CoOwnerID as ID, c.ProspectID, case when co.prospectid = p.prospectid then p.SpouseFirstName else p.FirstName end as FirstName, case when p.prospectid = co.prospectid then p.spouselastname else p.LastName end as LastName from t_ContractCoOwner c inner join t_Prospect p on p.prospectid = c.prospectid inner join t_Contract co on co.contractid = c.contractid where c.ContractID = " & _ContractID
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Remove(ByVal ID As Integer) As Boolean
        Dim bAns As Boolean = False
        Try
            cn.Open()
            cm.CommandText = "Delete from t_ContractCoOwner where CoOwnerID = " & ID
            cm.ExecuteNonQuery()
            cn.Close()
            bAns = True
        Catch ex As Exception
            _err = ex.ToString
            bAns = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try

        Return bAns
    End Function

    Public Sub Save()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If _ID = 0 Then
                cm.CommandText = "Select * from t_ContractCoOwner where ProspectID = " & _ProspectID & " and ContractID = " & _ContractID
            Else
                cm.CommandText = "Select * from t_ContractCoOwner where CoOwnerID = " & _ID
            End If

            da = New SqlDataAdapter(cm)
            Dim sqlcmdbuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "ContractCoOwner")
            If ds.Tables("ContractCoOwner").Rows.Count > 0 Then
                dr = ds.Tables("ContractCoOwner").Rows(0)
            Else
                dr = ds.Tables("ContractCoOwner").NewRow
            End If
            dr("ProspectID") = _ProspectID
            dr("ContractID") = _ContractID

            If ds.Tables("ContractCoOwner").Rows.Count < 1 Then ds.Tables("ContractCoOwner").Rows.Add(dr)

            da.Update(ds, "ContractCoOwner")

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub


    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ContractCoOwner where CoOwnerID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "ContractCoOwner")
            If ds.Tables("ContractCoOwner").Rows.Count > 0 Then
                dr = ds.Tables("ContractCoOwner").Rows(0)
                Set_Values()
            End If

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _err = ex.ToString
        End Try
    End Sub

    Public Function Spouse_CoOwns(ByVal conID As Integer, ByVal prosID As Integer) As Boolean
        Dim bSpouse As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as CoOwners from t_ContractCoOwner where ContractID = " & conID & " and ProspectID = " & prosID
            dRead = cm.ExecuteReader
            dRead.Read()
            If dRead("CoOwners") = 0 Then
                bSpouse = False
            End If
            dRead.Close()
        Catch ex As Exception
            _err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bSpouse
    End Function

    Public Function Co_Owner_Count(ByVal conID As Integer, ByVal prosID As Integer) As Integer
        Dim bCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as CoOwners from t_ContractCoOwner where ContractID = " & conID & " and ProspectID <> " & prosID
            dRead = cm.ExecuteReader
            dRead.Read()
            bCount = dRead("CoOwners")
            dRead.Close()
        Catch ex As Exception
            _err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bCount
    End Function

    Public Function List_ReDeed_CoOwners(ByVal conID As Integer, ByVal prosID As Integer) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ProspectID")
        dt.Columns.Add("AddressID")
        dt.Columns.Add("PhoneID")
        dt.Columns.Add("EmailID")
        Try
            cn.Open()
            cm.CommandText = "Select xx.ProspectID, Case when xx.AddressID is null then 0 else xx.AddressID end as AddressID, Case when xx.PhoneID is null then 0 else xx.PhoneID end as PhoneID, Case when xx.EmailID is null then 0 else xx.EmailID end as EmailID from (Select ProspectID, (Select Top 1 addressID from t_ProspectAddress where prospectid = p.ProspectID and ActiveFlag = 1 and ContractAddress = 1) as AddressID, (Select Top 1 PhoneID from t_ProspectPhone where prospectid = p.ProspectID  and Active = 1) as PhoneID,  (Select Top 1 EmailID from t_ProspectEmail where prospectid = p.ProspectID and isPrimary = 1 and isActive = 1) as EmailID from t_ContractCoOwner p where p.ContractID = " & conID & " and p.ProspectID <> " & prosID & ") xx"
            dRead = cm.ExecuteReader
            Do While dRead.Read
                dr = dt.NewRow
                dr("ProspectID") = dRead("ProspectID")
                dr("AddressID") = dRead("AddressID")
                dr("PhoneID") = dRead("PhoneID")
                dr("EmailID") = dRead("EmailID")
                dt.Rows.Add(dr)
            Loop
        Catch ex As Exception
            _err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function
    Private Sub Set_Values()
        _ID = dr("ContractCoOwnerID")
        _ProspectID = IIf(Not (dr("ProspectID") Is System.DBNull.Value), dr("ProspectID"), 0)
        _ContractID = IIf(Not (dr("ContractID") Is System.DBNull.Value), dr("ContractID"), 0)
    End Sub

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _err
        End Get
    End Property
End Class
