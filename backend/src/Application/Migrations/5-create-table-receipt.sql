CREATE TABLE IF NOT EXISTS "Receipt" (
  "Id" uuid NOT NULL DEFAULT gen_random_uuid (),
  "Total" numeric(15, 4) NOT NULL,
  "Note" varchar(255),
  "PrintedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  -- Foreign key columns
  "CurrencyCode" varchar(3) NOT NULL,
  "UserId" uuid NOT NULL,
  "MerchantId" uuid NOT NULL,
  -- Timestamp columns
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  "DeletedAt" TIMESTAMP WITH TIME ZONE,
  -- Constraints
  CONSTRAINT "receipt_id_pkey" PRIMARY KEY ("Id"),
  CONSTRAINT "receipt_user_id_fkey" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE, -- Delete receipt if User is deleted
  CONSTRAINT "receipt_currency_code_fkey" FOREIGN KEY ("CurrencyCode") REFERENCES "Currency" ("Code"),
  CONSTRAINT "receipt_merchant_id_fkey" FOREIGN KEY ("MerchantId") REFERENCES "Merchant" ("Id")
);

-- We create an index to speed up queries that search by Id
CREATE UNIQUE INDEX IF NOT EXISTS "receipt_id_idx" ON "Receipt" ("Id");