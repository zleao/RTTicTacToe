using System;

namespace RTTicTacToe.WebApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddPlayerModel
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the player's name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string PlayerName { get; set; }
    }
}
