CREATE TABLE IF NOT EXISTS "Merchant" (
  "Id" uuid NOT NULL DEFAULT gen_random_uuid (),
  "Name" varchar(50) NOT NULL,
  -- Timestamp columns
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  "DeletedAt" TIMESTAMP WITH TIME ZONE,
  -- Constraints
  CONSTRAINT "merchant_id_pkey" PRIMARY KEY ("Id")
);

-- We create an index to speed up queries that search by Id
CREATE UNIQUE INDEX IF NOT EXISTS "merchant_id_idx" ON "Merchant" ("Id");