//---------------------------------------------------
// This file made by Calabonga SOFT at 10.02.2012
// Updated at 23.08.2016
//---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace STEPDAL
{
	public static class LinqExtensions
	{
		/// <summary>        
		/// Return specified number of random elements from collection        
		/// </summary>        
		/// <param name="count">number of random elements</param>        
		public static IQueryable<T> Randoms<T>(this IQueryable<T> source, int count) {
			return source.OrderBy(a => Guid.NewGuid()).Take(count);
		}

		/// <summary>        
		/// Return single random element from collection       
		/// </summary>   
		public static T Random<T>(this IQueryable<T> source) {
			if (source.Count() > 0) {
				return source.Randoms(1).First();
			}
			throw new ArgumentNullException("Requires more than one entry for a random selection");
		}

		/// <summary>       
		/// Return randomized collection     
		/// </summary>        
		public static IQueryable<T> Radomized<T>(this IQueryable<T> source) {
			return source.Randoms(source.Count());
		}

		/// <summary>
		/// Determines whether the collection is null or contains no elements.
		/// </summary>
		/// <typeparam name="T">The IEnumerable type.</typeparam>
		/// <param name="enumerable">The enumerable, which may be null or empty.</param>
		/// <returns>
		///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) {
			if (enumerable == null) return true;
			var collection = enumerable as ICollection<T>;
			if (collection != null) {
				return collection.Count < 1;
			}
			return enumerable.Any();
		}

		/// <summary>
		/// Determines whether the collection is null or contains no elements.
		/// </summary>
		/// <typeparam name="T">The ICollection type.</typeparam>
		/// <param name="collection">The collection, which may be null or empty.</param>
		/// <returns>
		///     <c>true</c> if the ICollection is null or empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
			if (collection == null) return true;
			return collection.Count < 1;
		}

				/// <summary>
		/// Возращает Следующий объект из коллекции
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="current"></param>
		/// <returns></returns>
		public static T GetNext<T>(IEnumerable<T> list, T current) {
			try {
				return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
			}
			catch {
				return default(T);
			}
		}

		/// <summary>
		/// Возвращает предыдущий объект из коллекции
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="current"></param>
		/// <returns></returns>
		public static T GetPrevious<T>(IEnumerable<T> list, T current) {
			try {
				return list.TakeWhile(x => !x.Equals(current)).Last();
			}
			catch {
				return default(T);
			}
		}
	}
}