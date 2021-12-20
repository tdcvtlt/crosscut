Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web

Public Class clsLetters
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LetterContent As String = ""
    Dim _Name As String = ""
    Dim _TypeID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Letters where LetterID = " & _ID, cn)
    End Sub

    Public Function List(ByVal id As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select l.LetterID, l.Name, lt.comboitem as Type from t_Letters l left outer join t_Comboitems lt on lt.comboitemid = l.typeid " & IIf(id > 0, "where LetterID = " & id, ""))
    End Function

    Public Function ListTypes(ByVal type As String) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select i.comboitemid as ID, i.Comboitem as Type from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'LetterType'  " & IIf(type <> "", " and i.comboitem like '" & type & "%'", "") & " order by i.comboitem")
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Letters where LetterID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Letters")
            If ds.Tables("t_Letters").Rows.Count > 0 Then
                dr = ds.Tables("t_Letters").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Get_Letter_Text(ByVal LetterID As Integer, ByVal KeyID As Integer) As String
        Dim ret As String = ""
        Try
            da = New SqlDataAdapter(cm)
            ds = New DataSet

            'Get the source view for the type 
            cm.CommandText = "Select v.* from t_LetterViews v inner join t_Letters l on l.typeid = v.typeid where l.letterid = " & LetterID & " and v.source = 1"
            da.Fill(ds, "SourceView")
            'Get the source record
            cm.CommandText = "Select * from " & ds.Tables("SourceView").Rows(0)("View").ToString & " where " & ds.Tables("SourceView").Rows(0)("KeyField").ToString & " = '" & KeyID & "'"
            da.Fill(ds, "SourceRecord")
            'get the list of views order by viewid
            cm.CommandText = "Select * from t_LetterViews v where v.typeid = " & ds.Tables("SourceView").Rows(0)("TypeID").ToString & " order by viewid"
            da.Fill(ds, "Views")
            'get the tags and order by viewid
            cm.CommandText = "Select * from t_LetterTags t where t.typeid = " & ds.Tables("SourceView").Rows(0)("TypeID").ToString & " order by viewid"
            da.Fill(ds, "Tags")
            'get the letter
            cm.CommandText = "Select * from t_Letters where letterid = " & LetterID
            da.Fill(ds, "Letter")
            'get tag styles
            cm.CommandText = "Select * from t_LetterTagStyles"
            da.Fill(ds, "Styles")

            'Get letter text
            ret = HttpUtility.HtmlDecode(ds.Tables("Letter").Rows(0)("LetterContent").ToString)

            Dim LastViewID As Integer = 0
            Dim bComplete As Boolean = False
            'Loop through the tags
            For i As Integer = 0 To ds.Tables("Tags").Rows.Count - 1
                If Not ds.Tables("CurrentView") Is Nothing Then
                    ds.Tables.Remove("CurrentView")
                End If
                If ds.Tables("Tags").Rows(i)("Tag").ToString.ToUpper = "<DATE>" Then
                    ret = Replace(ret, "<DATE>", DateTime.Now.ToShortDateString)
                Else
                    For x = 0 To ds.Tables("Views").Rows.Count - 1
                        If ds.Tables("Views").Rows(x)("ViewID").ToString = ds.Tables("Tags").Rows(i)("ViewID").ToString And ds.Tables("SourceRecord").Rows.Count > 0 Then
                            cm.CommandText = "Select * from " & ds.Tables("Views").Rows(IIf(LastViewID = 0, x, x + 1))("View").ToString & " where " & ds.Tables("Tags").Rows(i)("KeyFieldName").ToString & " = '" & ds.Tables("SourceRecord").Rows(0)(ds.Tables("Tags").Rows(i)("KeyFieldValue").ToString).ToString & "'"
                            da.Fill(ds, "CurrentView")
                            Exit For
                        End If
                    Next
                    If Not (ds.Tables("CurrentView") Is Nothing) Then
                        If ds.Tables("CurrentView").Rows.Count > 0 Then
                            If ds.Tables("CurrentView").Columns(ds.Tables("Tags").Rows(i)("FieldName").ToString).DataType.ToString = System.Type.GetType("System.DateTime").ToString Then
                                Dim dt As DateTime = DateTime.MaxValue
                                If DateTime.TryParse(ds.Tables("CurrentView").Rows(0)(ds.Tables("Tags").Rows(i)("FieldName").ToString).ToString, dt) Then
                                    ret = Replace(ret, ds.Tables("Tags").Rows(i)("Tag").ToString, FormatDateTime(ds.Tables("CurrentView").Rows(0)(ds.Tables("Tags").Rows(i)("FieldName").ToString).ToString, DateFormat.ShortDate))
                                Else
                                    ret = Replace(ret, ds.Tables("Tags").Rows(i)("Tag").ToString, "")
                                End If

                                'ret = Replace(ret, ds.Tables("Tags").Rows(i)("Tag").ToString, FormatDateTime(ds.Tables("CurrentView").Rows(0)(ds.Tables("Tags").Rows(i)("FieldName").ToString).ToString, DateFormat.ShortDate))
                            Else
                                'Add Looping
                                Dim NewText As String = ""
                                If ds.Tables("Tags").Rows(i)("TagStyleID").ToString & "" <> "" And ds.Tables("Tags").Rows(i)("TagStyleID").ToString & "" <> "0" Then
                                    'has a looping tag -- match ids
                                    For x = 0 To ds.Tables("Styles").Rows.Count - 1
                                        If ds.Tables("Styles").Rows(x)("TagStyleID").ToString = ds.Tables("Tags").Rows(i)("TagStyleID").ToString Then
                                            If ds.Tables("Styles").Rows(x)("Style").ToString.ToUpper = "CSV" Or ds.Tables("Styles").Rows(x)("Style").ToString.ToUpper = "COMMA SEPARATED VALUES" Then
                                                NewText = Loop_Items(ds.Tables("CurrentView"), ds.Tables("Tags").Rows(i)("FieldName").ToString, "", ", ")
                                                NewText = Left(NewText, Len(NewText) - 2)
                                            Else
                                                NewText = ds.Tables("Styles").Rows(x)("StartingTag").ToString
                                                NewText &= Loop_Items(ds.Tables("CurrentView"), ds.Tables("Tags").Rows(i)("FieldName").ToString, ds.Tables("Styles").Rows(x)("ItemTag").ToString, Get_Closing_Separater_Tag(ds.Tables("Styles").Rows(x)("ItemTag").ToString))
                                                NewText &= Get_Closing_Separater_Tag(ds.Tables("Styles").Rows(x)("StartingTag").ToString)
                                            End If
                                            Exit For
                                        Else
                                            NewText = "No Style Found (" & ds.Tables("Tags").Rows(i)("TagStyleID").ToString & ")"
                                        End If
                                    Next
                                Else
                                    NewText = ds.Tables("CurrentView").Rows(0)(ds.Tables("Tags").Rows(i)("FieldName").ToString).ToString
                                End If

                                ret = Replace(ret, ds.Tables("Tags").Rows(i)("Tag").ToString, NewText)
                            End If
                        Else
                            ret = Replace(ret, ds.Tables("Tags").Rows(i)("Tag").ToString, "#nodata#")
                        End If
                    End If
                End If
            Next
            ret = "<html><head></head><body>" & ret & "</body></html>"
        Catch ex As Exception
            ret = ex.ToString
        End Try


        Return ret
    End Function

    Private Sub Set_Values()
        If Not (dr("LetterContent") Is System.DBNull.Value) Then _LetterContent = dr("LetterContent")
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
    End Sub


    Private Function Loop_Items(ByRef dtItems As DataTable, ByVal sFieldName As String, ByVal sSeparaterStartTag As String, ByVal sSeparaterEndTag As String) As String
        Dim ret As String = ""
        For Each dr As DataRow In dtItems.Rows
            ret &= sSeparaterStartTag & dr(sFieldName).ToString & sSeparaterEndTag
        Next
        Return ret
    End Function

    Private Function Get_Closing_Separater_Tag(ByVal sTag As String) As String
        Dim ret As String = ""
        For i = 0 To Len(sTag) - 1
            If sTag.Substring(i, 1) = " " Then
                ret = Left(sTag, 1) & "/" & Right(Left(sTag, i), Len(Left(sTag, i)) - 1) & ">"
                Exit For
            End If
        Next
        If ret = "" And Len(sTag) > 2 Then
            ret = Left(sTag, 1) & "/" & Right(sTag, Len(sTag) - 1)
        End If
        Return ret
    End Function

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Letters where LetterID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Letters")
            If ds.Tables("t_Letters").Rows.Count > 0 Then
                dr = ds.Tables("t_Letters").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LettersInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LetterContent", SqlDbType.text, 0, "LetterContent")
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@LetterID", SqlDbType.Int, 0, "LetterID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Letters").NewRow
            End If
            Update_Field("LetterContent", _LetterContent, dr)
            Update_Field("Name", _Name, dr)
            Update_Field("TypeID", _TypeID, dr)
            If ds.Tables("t_Letters").Rows.Count < 1 Then ds.Tables("t_Letters").Rows.Add(dr)
            da.Update(ds, "t_Letters")
            _ID = ds.Tables("t_Letters").Rows(0).Item("LetterID")
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
            oEvents.KeyField = "LetterID"
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

    Public Property LetterContent() As String
        Get
            Return _LetterContent
        End Get
        Set(ByVal value As String)
            _LetterContent = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property LetterID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
