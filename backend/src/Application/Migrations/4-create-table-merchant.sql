CREATE TABLE IF NOT EXISTS "Merchant" (
  "Id" uuid NOT NULL DEFAULT gen_random_uuid (),
  "Name" varchar(50) NOT NULL,
  "UserId" uuid NOT NULL,
  -- Timestamp columns
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  "DeletedAt" TIMESTAMP WITH TIME ZONE,
  -- Constraints
  CONSTRAINT "merchant_id_pkey" PRIMARY KEY ("Id"),
  CONSTRAINT "merchant_user_id_fkey" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE, -- Delete merchant if User is deleted
  CONSTRAINT "merchant_name_user_id_unique_key" UNIQUE ("Name", "UserId")
);

-- We create an index to speed up queries that search by Id
CREATE UNIQUE INDEX IF NOT EXISTS "merchant_id_idx" ON "Merchant" ("Id");