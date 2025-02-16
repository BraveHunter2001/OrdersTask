using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class initValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO public."Items" ("Id", "Code", "Name", "Price", "Category")
                VALUES
                    (gen_random_uuid(), '00-0000-AA01', 'Toster', 55, 'Tech'),
                    (gen_random_uuid(), '00-0000-AA02', 'Microwave', 75, 'Tech'),
                    (gen_random_uuid(), '00-0000-AA03', 'Blender', 40, 'Kitchen'),
                    (gen_random_uuid(), '00-0000-AA04', 'Vacuum Cleaner', 120, 'Home'),
                    (gen_random_uuid(), '00-0000-AA05', 'Laptop', 800, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA06', 'Smartphone', 600, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA07', 'Tablet', 300, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA08', 'Monitor', 200, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA09', 'Keyboard', 50, 'Accessories'),
                    (gen_random_uuid(), '00-0000-AA10', 'Mouse', 30, 'Accessories'),
                    (gen_random_uuid(), '00-0000-AA11', 'Headphones', 80, 'Accessories'),
                    (gen_random_uuid(), '00-0000-AA12', 'Smartwatch', 250, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA13', 'Speaker', 150, 'Audio'),
                    (gen_random_uuid(), '00-0000-AA14', 'Gaming Console', 500, 'Gaming'),
                    (gen_random_uuid(), '00-0000-AA15', 'Router', 100, 'Networking'),
                    (gen_random_uuid(), '00-0000-AA16', 'External HDD', 120, 'Storage'),
                    (gen_random_uuid(), '00-0000-AA17', 'Power Bank', 40, 'Accessories'),
                    (gen_random_uuid(), '00-0000-AA18', 'Webcam', 60, 'Accessories'),
                    (gen_random_uuid(), '00-0000-AA19', 'Printer', 300, 'Office'),
                    (gen_random_uuid(), '00-0000-AA20', 'Scanner', 250, 'Office'),
                    (gen_random_uuid(), '00-0000-AA21', 'Camera', 500, 'Photography'),
                    (gen_random_uuid(), '00-0000-AA22', 'Tripod', 100, 'Photography'),
                    (gen_random_uuid(), '00-0000-AA23', 'Smart Bulb', 25, 'Smart Home'),
                    (gen_random_uuid(), '00-0000-AA24', 'Smart Plug', 30, 'Smart Home'),
                    (gen_random_uuid(), '00-0000-AA25', 'Gaming Mouse', 70, 'Gaming'),
                    (gen_random_uuid(), '00-0000-AA26', 'Gaming Keyboard', 90, 'Gaming'),
                    (gen_random_uuid(), '00-0000-AA27', 'Desk Lamp', 45, 'Office'),
                    (gen_random_uuid(), '00-0000-AA28', 'Chair', 150, 'Furniture'),
                    (gen_random_uuid(), '00-0000-AA29', 'Desk', 250, 'Furniture'),
                    (gen_random_uuid(), '00-0000-AA30', 'E-Reader', 120, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA31', 'Fitness Tracker', 180, 'Health'),
                    (gen_random_uuid(), '00-0000-AA32', 'Projector', 400, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA33', 'Coffee Machine', 220, 'Kitchen'),
                    (gen_random_uuid(), '00-0000-AA34', 'Air Purifier', 350, 'Home'),
                    (gen_random_uuid(), '00-0000-AA35', 'Dishwasher', 900, 'Kitchen'),
                    (gen_random_uuid(), '00-0000-AA36', 'Iron', 60, 'Home'),
                    (gen_random_uuid(), '00-0000-AA37', 'TV', 1000, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA38', 'Sewing Machine', 250, 'Home'),
                    (gen_random_uuid(), '00-0000-AA39', 'Drone', 700, 'Photography'),
                    (gen_random_uuid(), '00-0000-AA40', 'Electric Scooter', 1200, 'Transport'),
                    (gen_random_uuid(), '00-0000-AA41', 'VR Headset', 600, 'Gaming'),
                    (gen_random_uuid(), '00-0000-AA42', 'Smart Doorbell', 150, 'Smart Home'),
                    (gen_random_uuid(), '00-0000-AA43', 'Bluetooth Adapter', 40, 'Accessories'),
                    (gen_random_uuid(), '00-0000-AA44', 'Wireless Charger', 35, 'Accessories'),
                    (gen_random_uuid(), '00-0000-AA45', 'Portable Speaker', 120, 'Audio'),
                    (gen_random_uuid(), '00-0000-AA46', 'Mic Stand', 70, 'Audio'),
                    (gen_random_uuid(), '00-0000-AA47', 'Graphic Tablet', 300, 'Electronics'),
                    (gen_random_uuid(), '00-0000-AA48', 'Security Camera', 200, 'Smart Home'),
                    (gen_random_uuid(), '00-0000-AA49', 'Smart Thermostat', 250, 'Smart Home'),
                    (gen_random_uuid(), '00-0000-AA50', 'Home Assistant', 180, 'Smart Home');
                
                ALTER SEQUENCE public."ItemSequence"
                	RESTART 51;
                
                
                INSERT INTO public."Users"
                ("Id", "Login", "Password", "Role")
                VALUES(gen_random_uuid(), 'manager', 'manager', 'Manager');
                
                
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
