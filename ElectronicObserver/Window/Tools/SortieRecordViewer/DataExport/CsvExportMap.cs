using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CsvHelper;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public abstract class CsvExportMap<T> : CsvRecordWriter<T>
{
	private string ColumnNamePrefix { get; set; } = "";
	private List<CsvExportColumnModel> Columns { get; } = [];

	public override int FieldCount => Columns.Count;

	public override void WriteHeader(CsvWriter writer)
	{
		foreach (CsvExportColumnModel column in Columns)
		{
			WriteColumnName(writer, column.Name());
		}
	}

	public override void Write(CsvWriter writer, T record)
	{
		foreach (CsvExportColumnModel column in Columns)
		{
			column.Write(writer, record);
		}
	}

	protected CsvExportColumn<T, TValue> Map<TValue>(Expression<Func<T, TValue>> expression)
	{
		Func<T, TValue> getter = expression.Compile();

		CsvExportColumnModel column = new()
		{
			Name = () => GetMemberName(expression),

			Write = (writer, record) =>
			{
				TValue? value = record switch
				{
					null => default,
					_ => getter((T)record),
				};

				writer.WriteField(value);
			}
		};

		Columns.Add(column);

		return new(column);
	}

	protected void References<TReferenceMap, TReference>(
		Expression<Func<T, TReference>> expression,
		Action<TReferenceMap>? mapping = null)
		where TReferenceMap : CsvExportMap<TReference>, new()
	{
		TReferenceMap referenceMap = new();

		mapping?.Invoke(referenceMap);

		Func<T, TReference> getter = expression.Compile();

		foreach (CsvExportColumnModel nestedColumn in referenceMap.Columns)
		{
			Columns.Add(new()
			{
				Name = () => referenceMap.GetColumnName(nestedColumn),

				Write = (writer, record) =>
				{
					TReference? reference = record switch
					{
						null => default,
						_ => getter((T)record),
					};

					nestedColumn.Write(writer, reference);
				}
			});
		}
	}

	private string GetColumnName(CsvExportColumnModel column)
		=> string.IsNullOrWhiteSpace(ColumnNamePrefix) switch
		{
			true => column.Name(),
			_ => $"{ColumnNamePrefix}{column.Name()}",
		};

	public void Prefix(string prefix)
	{
		ColumnNamePrefix = string.IsNullOrEmpty(ColumnNamePrefix) switch
		{
			true => prefix,
			_ => $"{ColumnNamePrefix}{prefix}",
		};
	}

	private static string GetMemberName<TValue>(Expression<Func<T, TValue>> expression)
	{
		Expression body = expression.Body;

		if (body is UnaryExpression unary)
		{
			body = unary.Operand;
		}

		if (body is not MemberExpression member)
		{
			throw new ArgumentException("Expression must select a property.");
		}

		return member.Member.Name;
	}
}
