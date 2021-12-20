Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsPackageIssued
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Dim _ID As Integer = 0
    Dim _PackageIssuedID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _Cost As Double = 0
    Dim _StatusID As Integer = 0
    Dim _ExpirationDate As String = ""
    Dim _PurchaseDate As String = ""
    Dim _StatusDate As String = ""
    Dim _VendorID As Integer = 0
    Dim _Err As String = ""
    Private __LocationID As String
    Dim _UserID As Integer = 0

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Replace", cn)
    End Sub

    Private Property _LocationID As String
        Get
            Return __LocationID
        End Get
        Set(ByVal value As String)
            __LocationID = value
        End Set
    End Property

    Public Sub Load()
        Try
            Dim sql As String = "Select * from t_PackageIssued where PackageIssuedID = " & _PackageIssuedID
            cm.CommandText = sql
            cn.Open()
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                Fill_Values()
            End If
            dread.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try

    End Sub

    Private Sub Fill_Values()
        Try
            _PackageIssuedID = dread("PackageIssuedID")
            _ProspectID = IIf(dread("ProspectID") Is System.DBNull.Value, 0, dread("ProspectID"))
            _PackageID = IIf(dread("PackageID") Is System.DBNull.Value, 0, dread("PackageID"))
            _Cost = IIf(dread("Cost") Is System.DBNull.Value, 0, dread("Cost"))
            _PurchaseDate = dread("PurchaseDate") & ""
            _ExpirationDate = dread("ExpirationDate") & ""
            _StatusDate = dread("StatusDate") & ""
            _StatusID = IIf(dread("StatusID") Is System.DBNull.Value, 0, dread("StatusID"))
            _VendorID = IIf(dread("VendorID") Is System.DBNull.Value, 0, dread("VendorID"))
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function val_PkgID(ByVal ID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Packages from t_PackageIssued where PackageIssuedid = '" & ID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Packages") > 0 Then
                bValid = True
            Else
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property

    Public Property PackageIssuedID() As Integer
        Get
            Return _PackageIssuedID
        End Get
        Set(ByVal value As Integer)
            _PackageIssuedID = value
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

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property Cost() As Double
        Get
            Return _Cost
        End Get
        Set(ByVal value As Double)
            _Cost = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            If value <> _StatusID Then
                _StatusDate = System.DateTime.Now.ToShortDateString
            End If
            _StatusID = value
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

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property PurchaseDate() As String
        Get
            Return _PurchaseDate
        End Get
        Set(ByVal value As String)
            _PurchaseDate = value
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
    Public Property VendorID As Integer
        Get
            Return _VendorID
        End Get
        Set(value As Integer)
            _VendorID = value
        End Set
    End Property

    Public Function Search(ByVal filter As String, ByVal filterType As String, ByVal vendors As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If vendors = "0" Then
                If filter = "" Then
                    If filterType = "ID" Then
                        ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_Prospect p on pi.prospectid = p.prospectid order by packageissuedid asc"
                    Else
                        ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_Prospect p on pi.prospectid = p.prospectid order by p.lastname asc"
                    End If
                Else
                    If filterType = "ID" Then
                        ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_Prospect p on pi.prospectid = p.prospectid where pi.packageissuedid like '" & CInt(filter) & "%'"
                    Else
                        If InStr(filter, ",") > 0 Then
                            Dim sName(2) As String
                            sName = filter.Split(",")
                            ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_Prospect p on pi.prospectid = p.prospectid where p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'"
                        Else
                            ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_Prospect p on pi.prospectid = p.prospectid where p.lastname like '" & Replace(filter, "'", "''") & "%'"
                        End If
                    End If
                End If
            Else
                If filter = "" Then
                    If filterType = "ID" Then
                        ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID inner join t_Prospect p on pi.prospectid = p.prospectid where vp.VendorID in (" & vendors & ") order by packageissuedid asc"
                    Else
                        ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID inner join t_Prospect p on pi.prospectid = p.prospectid where vp.VendorID in (" & vendors & ") order by p.Lastname asc"
                    End If
                Else
                    If filterType = "ID" Then
                        ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_vendor2Package vp on pk.PackageID = vp.PackageID inner join t_Prospect p on pi.prospectid = p.prospectid where pi.packageissuedid like '" & CInt(filter) & "%' and vp.VendorID in (" & vendors & ")"
                    Else
                        If InStr(filter, ",") > 0 Then
                            Dim sName(2) As String
                            sName = filter.Split(",")
                            ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_vendor2Package vp on pk.PackageID = vp.PackageID inner join t_Prospect p on pi.prospectid = p.prospectid where p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%' and vp.VendorID in (" & vendors & ")"
                        Else
                            ds.SelectCommand = "Select Top 50 pi.PackageIssuedID, pk.Package, p.FirstName, p.LastName from t_PackageIssued pi inner join t_Package pk on pi.packageid = pk.packageid inner join t_vendor2Package vp on pk.PackageID = vp.PackageID inner join t_Prospect p on pi.prospectid = p.prospectid where p.lastname like '" & Trim(filter).Replace(New Char() {"'"}, "''") & "%' and vp.VendorID in (" & vendors & ")"
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List(ByVal prospectID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select pi.PackageIssuedID, p.Package, ps.ComboItem as Status from t_PackageIssued pi inner join t_Package p on pi.PackageID = p.PackageID left outer join t_ComboItems ps on pi.StatusID = ps.ComboItemID where pi.ProspectID = '" & prospectID & "'"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Save() As Boolean
        Try
            Dim bNew As Boolean
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageIssued where packageissuedid = " & _PackageIssuedID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "PkgIssued")
            If ds.Tables("PkgIssued").Rows.Count > 0 Then
                dr = ds.Tables("PkgIssued").Rows(0)
                bNew = False
            Else
                bNew = True
                da.InsertCommand = New SqlCommand("dbo.sp_PackageIssuedInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                'da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.Int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.Int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.Int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@Cost", SqlDbType.Money, 0, "Cost")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@ExpirationDate", SqlDbType.DateTime, 0, "ExpirationDate")
                da.InsertCommand.Parameters.Add("@PurchaseDate", SqlDbType.DateTime, 0, "PurchaseDate")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.Int, 0, "VendorID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageIssuedID", SqlDbType.Int, 0, "PackageIssuedID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("PkgIssued").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("Cost", _Cost, dr)
            Update_Field("StatusID", _StatusID, dr)
            '_Err = _StatusDate
            Update_Field("ExpirationDate", _ExpirationDate, dr)
            Update_Field("PurchaseDate", _PurchaseDate, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            'If _Birthdate = "" Then _Birthdate = System.DBNull.Value.ToString
            'Update_Field("Birthdate", _Birthdate, dr)
            Update_Field("VendorID", _VendorID, dr)
            If ds.Tables("PkgIssued").Rows.Count < 1 Then ds.Tables("PkgIssued").Rows.Add(dr)
            da.Update(ds, "PkgIssued")
            _PackageIssuedID = ds.Tables("PkgIssued").Rows(0).Item("PackageIssuedID")

            If bNew Then
                Dim oEvt As New clsEvents
                oEvt.Create_Create_Event("PackageIssuedID", _PackageIssuedID, 0, _UserID, "")
                oEvt = Nothing
                cm = New SqlCommand("sp_Build_Package", cn)
                cm.CommandType = CommandType.StoredProcedure
                cm.Parameters.Add(New SqlParameter("@ProspectID", SqlDbType.Int, 0))
                cm.Parameters.Add(New SqlParameter("@PackageID", SqlDbType.Int, 0))
                cm.Parameters.Add(New SqlParameter("@PackageIssuedID", SqlDbType.Int, 0))
                cm.Parameters.Add(New SqlParameter("@UserID", SqlDbType.Int, 0))
                cm.Parameters.Add(New SqlParameter("@Amount", SqlDbType.Money, 0))
                cm.Parameters("@ProspectID").Value = _ProspectID
                cm.Parameters("@PackageID").Value = _PackageID
                cm.Parameters("@PackageIssuedID").Value = _PackageIssuedID
                cm.Parameters("@UserID").Value = _UserID
                cm.Parameters("@Amount").Value = _Cost
                cm.ExecuteNonQuery()
            End If
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
            oEvents.KeyField = "PackageIssuedID"
            oEvents.KeyValue = _PackageIssuedID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function CXL_Package(ByVal pkgIssID As Integer) As Boolean
        Dim bCXL As Boolean = True
        Dim oTour As New clsTour
        oTour.UserID = _UserID
        Dim oRes As New clsReservations
        oRes.UserID = _UserID
        Dim oCombo As New clsComboItems
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Tour where PackageIssuedID = " & pkgIssID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    oTour.TourID = dread("TourID")
                    oTour.Load()
                    oTour.StatusID = oCombo.Lookup_ID("TourStatus", "Kicked")
                    oTour.Save()
                Loop
            End If
            dread.Close()
            cm.CommandText = "Select * from t_Reservations where PackageIssuedID = " & pkgIssID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    oRes.ReservationID = dread("ReservationID")
                    oRes.Load()
                    oRes.StatusID = oCombo.Lookup_ID("ReservationStatus", "Kicked")
                    oRes.Save()
                Loop
            End If
            dread.Close()
            oTour = Nothing
            oRes = Nothing
            oCombo = Nothing
        Catch ex As Exception
            _Err = ex.Message
            bCXL = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bCXL
    End Function

    Public Function Transfer_Package(prospect_id As Integer, package_issued_id As Integer, prospect_id_orginal As Integer, userDBID As Integer) As Integer
        Dim sql = New StringBuilder()
        Dim returnValue = 0

        sql.AppendFormat("update t_INVOICES set ProspectID={0} where KeyField='PACKAGEISSUEDID' and KeyValue={1};", prospect_id, package_issued_id)
        sql.AppendFormat("update t_INVOICES set ProspectID={0} where KeyField='RESERVATIONID' and KeyValue=(select reservationid from t_RESERVATIONS where PACKAGEISSUEDID={1});", prospect_id, package_issued_id)
        sql.AppendFormat("update t_INVOICES set ProspectID={0} where KeyField='TOURID' and KeyValue=(select tourid from t_TOUR where PACKAGEISSUEDID={1});", prospect_id, package_issued_id)
        sql.AppendFormat("update t_PACKAGEISSUED set ProspectID={0} where PACKAGEISSUEDID={1};", prospect_id, package_issued_id)
        sql.AppendFormat("update t_RESERVATIONS set ProspectID={0} where PACKAGEISSUEDID={1};", prospect_id, package_issued_id)
        sql.AppendFormat("update t_TOUR set ProspectID={0} where PACKAGEISSUEDID={1};", prospect_id, package_issued_id)
        sql.AppendFormat("insert into t_NOTE(KeyField, KeyValue, Note, DateCreated, CreatedByID) values ('ProspectID', {0}, 'Package {2} was moved from ProspectID#{3} to ProspectID#{0}', getdate(), {1});", prospect_id, userDBID, package_issued_id, prospect_id_orginal)
        sql.AppendFormat("insert into t_NOTE(KeyField, KeyValue, Note, DateCreated, CreatedByID) values ('ProspectID', {0}, 'Package {2} was moved from ProspectID#{0} to ProspectID#{3}', getdate(), {1});", prospect_id_orginal, userDBID, package_issued_id, prospect_id)
        sql.AppendFormat("insert into t_NOTE(KeyField, KeyValue, Note, DateCreated, CreatedByID) values ('PackageIssuedID', {0}, 'Package {0} was moved from ProspectID#{1} to ProspectID#{2}', getdate(), {3});", package_issued_id, prospect_id_orginal, prospect_id, userDBID)
        sql.AppendFormat("insert into t_EVENT(KeyField, KeyValue, Type, SubType, OldValue, NewValue, DateCreated, CreatedByID,FieldName) values ('PackageIssuedID',{0},'Change','',{1},{2},getdate(),{3},'ProspectID');", package_issued_id, prospect_id_orginal, prospect_id, userDBID)

        If 1 = 1 Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim tran As SqlTransaction = Nothing
                cn.Open()
                tran = cn.BeginTransaction()
                Try
                    Using cm = New SqlCommand(sql.ToString(), cn, tran)
                        returnValue = cm.ExecuteNonQuery()
                        tran.Commit()
                    End Using
                Catch ex As Exception
                    tran.Rollback()
                Finally
                    cn.Close()
                End Try
            End Using
        End If
        Return returnValue
    End Function
End Class


